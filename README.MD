# FluentArrange
FluentArrange lets you write clean Arrange blocks in your unit tests when the constructor of the _class under test_ has a lot of (mocked) dependencies.

## Packages
| Package                     | Version | Description                                                             |
|-----------------------------|---------|-------------------------------------------------------------------------|
| `FluentArrange`             | [![Nuget version](https://img.shields.io/nuget/v/fluentarrange)](https://www.nuget.org/packages/fluentarrange) | Core package
| `FluentArrange.NSubstitute` | [![Nuget version](https://img.shields.io/nuget/v/fluentarrange.nsubstitute)](https://www.nuget.org/packages/fluentarrange.nsubstitute) | When you use [NSubstitute](https://github.com/nsubstitute/NSubstitute) as the mocking framework

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

With FluentArrange, you can use Fluent API to achieve the exact same result as the code above:
~~~ C#
var sut = Arrange.Sut<ResetPasswordController>()
    .WithDependency<IAccountService>(x => x.FindEmail("foo@foo.com").Returns(new Account("foo")))
    .WithDependency<IMailService>(x => x.SendMail("foo@foo.com").Returns(true));
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

When used together, a unit test could look like this:
~~~ C#
// Arrange
var context = Arrange.For<ResetPasswordController>();

// Act
context.Sut.Reset("foo@foo.com");

// Assert
context.Dependency<IAccountService>().Received(1).FindEmail("foo@foo.com");
~~~