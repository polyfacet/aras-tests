# aras-tests

Project elaborating on tests in the context of Aras Innovator

- [Background](#background)
- [Aras General Integration XUnit Tests](#aras-general-integration-xunit-tests)
  - [Aras fixture](#aras-fixture)
    - [Example fixture configuration](#example-fixture-configuration)
  - [Arranging](#arranging)
    - [Use case example](#use-case-example)
  - [Extended Assertions](#extended-assertions)
  - [Test Example](#test-example)
  - [Traits](#traits)
    - [Suggested usage of Traits](#suggested-usage-of-traits)
  - [Known issues](#known-issues)
    - [Parallel running](#parallel-running)
      - [Solution](#solution)
  - [References and further reading](#references-and-further-reading)
- [Playwright for .NET](#playwright-for-net)
- [Stryker](#stryker)

## Background

[Aras Innovator](https://www.aras.com) is a "low-code" open PLM platform from [Aras Corp. (Wikipedia)](https://en.wikipedia.org/wiki/Aras_Corp). The platform is built to be customized, which is done by configurations and by code - primarily .NET for backend and Javascript for frontend. Due to this nature, unit testing are rarely applied during customizations. Most common approach to testing is the old-school PLM-manual-testing. Integration tests may be a way to apply automated testing for the purpose to avoid regression bugs and IMO ideally way to implicitly have some complementary documentation of the application.
With this background this project will focus on a integration tests for Aras. A framework for this will be the focus here.

## Aras General Integration XUnit Tests

The integration tests are using [XUnit](https://xunit.net) as the testing framework. There are several reasons for that:

- Well documented
- It is popular
- It is created by the creator of NUnit, as an successor of that.
- It runs test in parallel by default, which makes the tests run faster. Which is of high importance

Some functionality has been added to simplify writing tests and make them more maintainable.
Innovator.Client a 3rd party [innovator client](https://github.com/erdomke/Innovator.Client) - which replicates the IOM api is used. It has some benefits over the IOM(s) provided from Aras.

- You don't need the SDK IOM for as specific Aras version
- You can setup a session using an already MD5-hashed string. I.e. you don't need to write passwords in plain text in configuration files

### Aras fixture

When integration testing Aras at least one innovator session needs to be setup. This is done via configurations in **TestFixture.config**
The xunit fixturing will use this configuration. The label "admin" must exist, as a default. In the example below a standard OOTB installation is used.
When the tests spins up, the admin session is created and then other users sessions are created. As no Change Manager (CM) exist in OOTB, the user will also be created - as the configuration specifies it.
In this case we also have a OOTBTest class inheriting ArasTestBase class - making the CMInn session conveniently available for all Test using OOTBTest.

``` csharp

public class OOTBTest : ArasTestBase {
    protected readonly Innovator.Client.IOM.Innovator CMInn;

    public OOTBTest(ArasCollectionFixture fixture, ITestOutputHelper output) : base(fixture, output) {
        CMInn = fixture.GetInnovatorBySessionName("CM");
    }
}
```

#### Example fixture configuration

``` xml

<?xml version="1.0" encoding="utf-8" ?>
<Environment>
 <Url>http://localhost/2023</Url>
 <DatabaseName>2023</DatabaseName>
 <Users>
  <User label="admin">
   <Login>admin</Login>
   <Password>innovator</Password>
  </User>
  <User label="CM" firstName="Bruce" lastName="Wayne">
   <Login>Batman</Login>
   <Password>test</Password>
   <MemberOf>World</MemberOf>
   <MemberOf>All Employees</MemberOf>
   <MemberOf>CM</MemberOf>
  </User>
 </Users>
</Environment>

```

### Arranging

Failed tests due to errors in the arrange phase is in my experience the most common case for a failing integration test. By wrapping the arrange phase within an ArrangeWrapper it will provide information about the failed test, that it is an arrange error. I.e. it is not necessary a "failed test". This information combined with a well modularized "arrange library" will make the tests more maintainable.

#### Use case example

As a simple example to make the problem more concrete. If we change the criteria for creating a Part, that it must have a classification. Many test relying on creating a Part in their Arrange section will fail. In that case we ideally only want to change this Arrange-error at one place. See [test example](#test-example) further down.

### Extended Assertions

Often we want to assert that a result of an action (Act) does not return an "isError", or that it should return an "isError".
To make these assertions more convenient the following assertions has been added:

- AssertItem.IsError(item)
- AssertItem.IsNotError(item)
- AssertItem.IsInState(item, expectedState)

**Note:** Innovator.Client actually has some assertion built in/extensions to the Item, like item.AssertNoError(). But it does not have the IsError assertion. And as you might want add more custom assertions and have separation of concerns (SoC), this projects assertion library is to prefer. And for now the Innovator.Client is quite easy to replace with an SDK IOM from Aras.

### Test Example

Here we show an example of how to use the above addition to write tests.
We will have some PartTests on OOTB - which means we will have access to an admin and cm session.
We will have two tests checking if a user can manually release a part they have created.
It is basically the same test, but with two different users/sessions used. Accomplished by using the Theory and InlineData attributes.

**Note:**: The tests - attributed with Fact or Theory for XUnit - has also been attributed with Traits. See: [Traits](#traits)

``` csharp
public class PartTests : OOTBTest
{
    public PartTests(ArasCollectionFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    private const string ITEM_TYPE = "Part";

    [Theory]
    [InlineData("admin")]
    [InlineData("CM")]
    [Trait("Domain", "Part")]
    [Trait("Part", "Release")]
    private void Users_can_manually_Release_Part(string user) {
        Innovator.Client.IOM.Innovator inn = GetInnovatorBySessionName(user);
        User_can_manually_Release_Part(inn);
    }

    private void User_can_manually_Release_Part(Innovator.Client.IOM.Innovator inn) {
        // Arrange
        // We use the Arrange class to make use of a common way to create a default item of specified item type
        // Within OOTBTest an IArasArranger (Implementations of CreateDefault etc.) is defined and injected to the Arrange constructor
        Arrange arrange = NewArrange(inn);
        Item part = arrange.CreateDefault(ITEM_TYPE);

        // Act
        Item result = part.apply("PE_ManualRelease");

        // Assert
        // We use the AssertItem class to have a simple/clean assertion
        AssertItem.IsNotError(result);
    }
}

```

### Traits

We have some added Traits to test above ([About test filters](https://github.com/Microsoft/vstest-docs/blob/main/docs/filter.md))
These could be used to e.g. run different sub sets of the tests in the solution.
For example the test solution could be used to run smoke test in a production environment, which can be convenient/reassuring when updating such.
([Organizing xunit tests with traits](https://www.brendanconnolly.net/organizing-tests-with-xunit-traits/))

#### Suggested usage of Traits

As the application develops test gradually gets more an more complex. From beginning, handling basic things like Create/Read/Update/Delete (CRUD) tests. After a while more complex test may be added like: ReleaseNewPart-WithNewRelated-DocumentVia-ECO, which may involve creation of multiple different item (Part/Document/ECO) at the arrange phase, as well as assignments in the ECO, which may require more session-users for the test. I.e. an initiator and one or more approvers - people to sign off.

By grading the complexity of the test it is both possible to run the tests by their traits, i.e. only run the basic tests to run when doing everyday development. As the complex tests, may be more time consuming. On the day to day work we want fast feedback.
And it would be possible to run the more complex test less frequently.
We would also get a better overview with these traits, to see how many tests we have of a specific complexity, or easily find our test that are more complex.

Example of complexity traits:
Complexity 1,2,3,4,5

### Known issues

#### Parallel running

When running tests in parallel you can encounter "deadlock victim" issues, like below

``` log
 Aras.Core.Tests.Arranging.ArrangeException : Arrange exception:
---- System.Exception : Transaction (Process ID 58) was deadlocked on lock resources with another process and has been chosen as the deadlock victim. Rerun the transaction.
  Stack Trace:
```

This is something that end users also could encounter, when they are doing operations concurrently with other users.

##### Solution

Even though it is not recommended by "Aras", I think you should set the database in snapshot mode.  
We have had this enabled in production environments for years without any noteable downsides.

``` sql
ALTER DATABASE <db_name>
SET ALLOW_SNAPSHOT_ISOLATION ON 
```

``` sql
ALTER DATABASE <db_name>
SET READ_COMMITTED_SNAPSHOT ON
GO
```

With the query

``` sql
SELECT name, snapshot_isolation_state_desc, is_read_committed_snapshot_on from sys.databases 
```

you can check that/if they are enabled or not.

You could also avoid running the tests in parallel, but users will experience this in the wild anyway in that case.



### References and further reading

1. XUnit : <https://xunit.net>
2. [Organizing xunit tests with traits](https://www.brendanconnolly.net/organizing-tests-with-xunit-traits/)
3. [More on traits, how to run tests without a Trait](https://blog.somewhatabstract.com/2016/12/27/running-xunit-tests-using-traits-and-leveraging-parallelism/)
4. [Extend xUnit Categories](https://github.com/brendanconnolly/Xunit.Categories)
5. [About Test Naming conventions](https://enterprisecraftsmanship.com/posts/you-naming-tests-wrong/)
6. [Unit testing best practices (Microsoft)](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

## Playwright for .NET

End to end testing with [Playwright](https://playwright.dev/dotnet/) in Aras Innovator
**TBD**

## Stryker

Test your tests with [Stryker Mutator](https://stryker-mutator.io)
**TBD**
