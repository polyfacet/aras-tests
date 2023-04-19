# aras-tests

Project elaborating on tests in the context of Aras Innovator

- [aras-tests](#aras-tests)
  - [Aras General Integration XUnit Tests](#aras-general-integration-xunit-tests)
    - [Aras fixture](#aras-fixture)
      - [Example fixture configuration](#example-fixture-configuration)
    - [Arranging](#arranging)
      - [Use case example](#use-case-example)
    - [Extended Assertions](#extended-assertions)
    - [Test Example {#test-example}](#test-example-test-example)
    - [Traits](#traits)
      - [Suggested usage of Traits](#suggested-usage-of-traits)
    - [References](#references)
  - [Playwright for .NET](#playwright-for-net)
  - [Stryker](#stryker)


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

When integration testing aras at least one innovator session needs to be setup. This is done via configurations in **TestFixtureConfig.xml**
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

- Assert.IsError(item)
- Assert.IsNotError(item)

**Note:** Innovator.Client actually has some assertion built in/extensions to the Item, like item.AssertNoError(). But it does not have the IsError assertion. And as you might want add more custom assertions and have separation of concerns (SoC), this projects assertion library is to prefer. And for now the Innovator.Client is quite easy to replace with an SDK IOM from Aras.

### Test Example {#test-example}

Here we show an example of how to use the above addition to write tests.
We will have some PartTests on OOTB - which means we will have access to an admin and cm session.
We will have two tests checking if a user can manually release a part they have created.
It is basically the same test, but with two different users/sessions used.

**Note:**: The tests - attributed with Fact for XUnit - has also been attributed with Traits. See: [Traits](#traits)

``` csharp
public class PartTests : OOTBTest
{
    public PartTests(ArasCollectionFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    private const string ITEM_TYPE = "Part";

    [Fact]
    [Trait("Domain", "Part")]
    [Trait("Part", "Release")]
    [Trait("Business", "OOTB")]
    public void Admin_ShouldBeAbleToManuallyReleasePart()
    {
        User_ShouldBeAbleToManuallyReleasePart(AdminInn);
    }

    [Fact]
    [Trait("Domain", "Part")]
    [Trait("Part", "Release")]
    [Trait("Business", "OOTB")]
    public void CM_ShouldBeAbleToManuallyReleasePart() {
        User_ShouldBeAbleToManuallyReleasePart(CMInn);
    }

    private void User_ShouldBeAbleToManuallyReleasePart(Innovator.Client.IOM.Innovator inn) {
        // Arrange
        // We use the Arrange class to make use of a common way to create a default item of specified item type
        Arrange arrange = new Arrange(inn);
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

### References

1. XUnit : <https://xunit.net>
2. [Organizing xunit tests with traits](https://www.brendanconnolly.net/organizing-tests-with-xunit-traits/)
3. [More on traits, how to run tests without a Trait](https://blog.somewhatabstract.com/2016/12/27/running-xunit-tests-using-traits-and-leveraging-parallelism/)

## Playwright for .NET

End to end testing with [Playwright](https://playwright.dev/dotnet/) in Aras Innovator
**TBD**

## Stryker

Test your tests with [Stryker Mutator](https://stryker-mutator.io)
**TBD**
