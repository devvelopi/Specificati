# Specificati

An implementation of the specification pattern useful for applying filters, projections and ordering to entities allow
interoperability across in memory and remote collections and promote ease of testing.

This is a simple package that provides the following benefits:

- Improved testability / single responsibility of LINQ expressions
- Improved aggregation (including logical OR, AND and NOT)
- Reduction of duplicated LINQ expressions
- Ability to apply filters / projections / ordering which are collection agnostic
- Clearer and more domain focused filtering language

## Installation

```bash
dotnet add package Specificati
```

## Changelog

See [Changelog](./CHANGELOG.md)

## Usage

All examples listed can be found in the `Specificati.Example` project.

To be as concise as possible, all of the following will be acting upon this example class that represents a domain /
database model:

See [Order](./src/Specificati.Example/Models/Order.cs)

```c#
public class Order {
     public string Name { get; set; }
     public long Amount { get; set; }
     public DateTime ProcessingDate { get; set; }
}
```

All specifications are agnostic across different forms of collections as they implicitly cast to and from the expected
expression or func types. This means that applying a specification to a `IQueryable` will look the same as applying to
an `IEnumerable` or other collection.

### Filters

Filters are simply encapsulated LINQ expressions, for example, acting on the above `Order` definition:

See [RecentOrderSpecification](./src/Specificati.Example/Specifications/Filters/RecentOrderSpecification.cs)

```c#
public class RecentOrderSpecification : FilterSpecification<Order>
{
    public override Expression<Func<Order, bool>> FilterExpression =>
        o => o.ProcessingDate <= DateTime.UtcNow.AddDays(-7);
}
```

#### Composition

It is possible to compose many filter specifications together such as the following:

```c#
.Where(new RecentOrderSpecification() & (new LargeOrderSpecification() | new OrderWithNameSpecification("X")))
```

This provides a nice middle-ground of flexibility and testability. However you can also create a specific composed
specification for maximum testability

```c#
public class XSpecification : FilterSpecification<Order>
{
    public override Expression<Func<Order, bool>> FilterExpression => new RecentOrderSpecification() & (new LargeOrderSpecification() | new OrderWithNameSpecification("X"));
}
```

#### Logical Operations

As seen above, filters use quite a neat little 'hacky' method of applying different rules by overriding operators
otherwise used for bitwise operations. Their usage is depicted in the table below:

| Operator        | Operation           | Example  |
| ------------- |-------------| -----|
| !    | Not | <code>!Order.Recent</code> |
| &      | And      |  <code>Order.Recent & Order.Large</code> |
| (pipe) | Or      |   <code>Order.Recent (pipe) Order.Large</code> |

#### Satisfaction

In addition to collection filtration, it is also possible to equate satisfaction on individual objects in an easy to
understand way.

```c#
var order = new Order {
   ProcessingDate = DateTime.UtcNow.AddDays(-7),
   Amount = 1
};

var specification = Order.Recent & !Order.Large;
var satisfied = specification.IsSatisfiedBy(order);
```

#### Generic Specification

Specificati comes pre-bundled with a generic specification that can be used to create custom filters on the
fly `GenericFilterSpecification`

```c#
    new GenericFilterSpecification(order => order.Amount == 10)
```

Generally, it would be advised to not use this specification unless in testing / quick development scenarios, as it is
more testable and clear to be specific with specifications,

I.e. A better way to write such a specification

```c#
public class OrderWithAmountSpecification : FilterSpecification<Order>
{
    private readonly long _amount;
    
    public OrderWithAmountSpecification(long amount) {
       _amount = amount;
    }

    public override Expression<Func<Order, bool>> FilterExpression =>
        o => o.Amount == _amount;
}
```

Which could be called in a similar way:

```c#
.Where(new OrderWithAmountSpecification(10))
```

### Projections

Projection specifications are used to transform one collection / object to another through a defined expression. This is
useful when there is mapping required from a source.

Specificati projections can also be applied in the same way regardless of backing collection.

See [OrderToOrderDtoProjection](./src/Specificati.Example/Specifications/Projections/OrderToOrderDtoProjectionSpecification.cs)

```c#
public class OrderToOrderDtoProjectionSpecification : ProjectionSpecification<Order, OrderDto>
{
    public override Expression<Func<Order, OrderDto>> ProjectionExpression => o => new OrderDto
    {
        Date = o.ProcessingDate,
        Name = o.Name,
        TotalAmount = o.Amount
    };
}
```

which is then applied

```c#
.Select<Order, OrderDto>(new OrderToOrderDtoProjectionSpecification())
```

#### Pitfalls

There are a couple of pitfalls when using this methodology.

- **Conditional ternary access** - Expressions don't allow the usage of  (I.e. object?.Value ?? ""), this can be
  overcome but is a little frustrating
- **Null access** - As in memory access expressions act differently to that of for example EntityFramework expressions,
  null access needs to be thought of well in advance, it is aided by enabling Nullable reference types
- **Explicit select typing** - Unfortunately, when using the implicit casting that comes with Projection and Ordering
  specifications, you must fully qualify the select method. We are still searching for a way around this, but are
  currently boiling it down to a limitation of the framework.

### Ordering

Ordering works in a very similar fashion to filtering.

See [OrderByProcessingDateSpecification](./src/Specificati.Example/Specifications/Orders/OrderByProcessingDateSpecification.cs)

```c#
  public class OrderByProcessingDateSpecification : OrderSpecification<Order, DateTime>
  {
      public override Expression<Func<Order, DateTime>> OrderExpression => o => o.ProcessingDate;
  }
```

which is then applied

```c#
.OrderBy<Order, DateTime>(new OrderByProcessingDateSpecification())
```

#### TODO
- Allow logical & to allow multiple ordering `.ThenBy(...)`

### Pagination

Pagination is very simple, and is more of a utility than a specification. It can be applied to any collection.

```c#
var pagination = new PagingSpecification {Skip = 5, Take = 15};
var paginated = pagination.Apply(Orders);
```

## Tips and Tricks

### Brevity

A trick that I use often is partial classes for the entity that is being acted upon. This allows for specifications to
be tightly scoped against the entity and also read more fluently. It is especially useful when you have specifications
that may be shared among different entities (I.e. HasIdSpecification).

As seen previously this takes the form

```c#
public partial class Order
{
    public static RecentOrderSpecification Recent => new RecentOrderSpecification();
    public static LargeOrderSpecification Large => new LargeOrderSpecification();
    public static OrderWithNameSpecification WithName(string name) => new OrderWithNameSpecification(name);
    
    public static OrderByProcessingDateSpecification ByProcessingDate => new OrderByProcessingDateSpecification();
}
```

which reads like

```c#
Order.Recent
Order.Large
Order.WithName("name")
Order.ByProcessingDate
```

## Works well with

### Repositori
A shameless plug for another project [Repositori](https://github.com/devvelopi/Repositori), an implementation of the Repository pattern

This allows a generic thin CRUD abstraction layer over any data source.