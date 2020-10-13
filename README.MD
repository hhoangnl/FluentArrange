<div align="center">
    <img alt="dotnet-dash" src="logo.png" width="33%" />
</div>

<hr />

**FluentArrange** lets you write clean Arrange blocks in your unit tests _even_ when the constructor of the _class under test_ has a lot of (mocked) dependencies.

## Packages
| Package                     | Version | Description                                                             |
|-----------------------------|---------|-------------------------------------------------------------------------|
| `FluentArrange`             | [![Nuget version](https://img.shields.io/nuget/v/fluentarrange)](https://www.nuget.org/packages/fluentarrange) | Core package
| `FluentArrange.NSubstitute` | [![Nuget version](https://img.shields.io/nuget/v/fluentarrange.nsubstitute)](https://www.nuget.org/packages/fluentarrange.nsubstitute) | When you use [NSubstitute](https://github.com/nsubstitute/NSubstitute) for mocking

## Auto-Mocking Ctor Dependencies
Consider the following example where we use NSubstitute to mock dependencies:
~~~ C#
var sut = new ResetPasswordController(
    Substitute.For<IAccountService>(),
    Substitute.For<IAuditService>(),
    Substitute.For<IMailService>());
~~~

With FluentArrange, an instance `T` is instantiated using `Arrange.Sut<T>`, and all of its constructor dependencies are auto-mocked:
~~~ C#
var sut = Arrange.Sut<ResetPasswordController>();
~~~

Adding new dependencies will not break existing unit tests.

## Arranging Dependencies Fluently
Most of the time, we need to arrange some behavior for our mocked dependencies:
~~~ C#
var accountService = Substitute.For<IAccountService>();
accountService.FindEmail("foo@foo.com").Returns(new Account("foo"));

var mailService = Substitute.For<IMailService>();
mailService.SendMail("foo@foo.com").Returns(true);

var sut = new ResetPasswordController(
    accountService,
    Substitute.For<IAuditService>(),
    mailService);
~~~

With FluentArrange, you can use the Fluent API `WithDependency<T>` to achieve the exact same result as the code above:
~~~ C#
var sut = Arrange.Sut<ResetPasswordController>()
    .WithDependency<IAccountService>(x => x.FindEmail("foo@foo.com").Returns(new Account("foo")))
    .WithDependency<IMailService>(x => x.SendMail("foo@foo.com").Returns(true));
~~~

As you might have noticed, we did not need to write arrange code for `IAuditService`, as that will be automatically created for you.
The only time you need to call `WithDependency<T>` is when you need to arrange the behavior of type `T`.

## Passing an instance instead
Sometimes, you may want to use a fake implementation rather than letting FluentArrange automatically create mocked instances.

In that case, you can provide an instance using `WithDependency<T>(T)`:

~~~ C#
var context = Arrange.Sut<ResetPasswordController>()
    .WithDependency<IAccountService>(new InMemoryAccountService());
~~~

or `WithDependency<T>(T, Action<T>)` if you need to do more arranging:
~~~ C#
var context = Arrange.Sut<ResetPasswordController>()
    .WithDependency<IAccountService>(new InMemoryAccountService(), d =>
    {
        d.AddAccount("foo", "foo@foo.com");
        d.AddAccount("foobar", "foobar@foo.com");
    });
~~~

or `WithDependency<T, T2>(T2, Action<T2>)` if you need to call `T2`-specific methods:
~~~ C#
var context = Arrange.Sut<ResetPasswordController>()
    .WithDependency<IAccountService, InMemoryAccountService>(new InMemoryAccountService(), d =>
    {
        d.AddTestAccounts();
    });
~~~

## Asserting Dependencies
Suppose you need to assert that a dependency's method has been called.
Well, you simply arrange code with `Arrange.For<T>` instead of `Arrange.Sut<T>` to get a `FluentArrangeContext` object:

~~~ C#
var context = Arrange.For<ResetPasswordController>();
~~~

To get the SUT, call the `Sut` property of the context:
~~~ C#
var sut = context.Sut;
~~~

To get the Dependency, simply call `Dependency<T>`:
~~~
context.Dependency<IAccountService>();
~~~

**or** `Dependency<T, T2>` to get access to `T2` and its specific methods:
~~~
context.Dependency<IAccountService, AccountService>();
~~~

When used together, a unit test could look like this:
~~~ C#
// Arrange
var context = Arrange.For<ResetPasswordController>();

// Act
context.Sut.Reset("foo@foo.com");

// Assert
context.Dependency<IAccountService>().Received(1).FindEmail("foo@foo.com");
~~~
