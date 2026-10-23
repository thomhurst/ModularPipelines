using System.Xml.Linq;
using ModularPipelines.DotNet;
using ModularPipelines.DotNet.Enums;
using ModularPipelines.DotNet.Parsers.Trx;

namespace ModularPipelines.DotNet.UnitTests;

public class TrxParsingTests
{
    private static readonly string TrxFixturePath = Path.Combine(
        AppContext.BaseDirectory,
        "Data",
        "test-results.trx");

    [Test]
    public async Task Parses_Skipped_Test_Reason()
    {
        var result = await ParseFixture();

        await Assert.That(result.UnitTestResults.Single(x => x.Outcome == TestOutcome.NotExecuted).Output?.DebugTrace)
            .IsEqualTo("Skipped: Linux only test");
    }

    [Test]
    public async Task Parses_Mixed_Test_Outcomes()
    {
        var testResult = await ParseFixture();

        await Assert.That(testResult.Successful).IsFalse();

        await Assert.That(testResult.UnitTestResults.Where(x => x.Outcome == TestOutcome.Failed))
            .Count().IsEqualTo(1);

        await Assert.That(testResult.UnitTestResults.Where(x => x.Outcome == TestOutcome.NotExecuted))
            .Count().IsEqualTo(1);

        await Assert.That(testResult.UnitTestResults.Where(x => x.Outcome == TestOutcome.Passed))
            .Count().IsEqualTo(2);
    }

    [Test]
    public async Task Parses_Failed_Test_Diagnostics()
    {
        var result = await ParseFixture();

        var failedTest = result.UnitTestResults.Single(x => x.Outcome == TestOutcome.Failed);

        await Assert.That(failedTest.TestName).IsEqualTo("Fail");
        await Assert.That(failedTest.Output?.ErrorInfo?.Message).IsEqualTo("This test is meant to fail");
        await Assert.That(failedTest.Output?.ErrorInfo?.StackTrace)
            .IsEqualTo("at ModularPipelines.TestsForTests.Tests.Fail()");
    }

    [Test]
    [Arguments("Passed", TestOutcome.Passed)]
    [Arguments("Failed", TestOutcome.Failed)]
    [Arguments("Error", TestOutcome.Error)]
    [Arguments("Timeout", TestOutcome.Timeout)]
    [Arguments("Aborted", TestOutcome.Aborted)]
    [Arguments("Inconclusive", TestOutcome.Inconclusive)]
    [Arguments("PassedButRunAborted", TestOutcome.PassedButRunAborted)]
    [Arguments("NotRunnable", TestOutcome.NotRunnable)]
    [Arguments("NotExecuted", TestOutcome.NotExecuted)]
    [Arguments("Disconnected", TestOutcome.Disconnected)]
    [Arguments("Warning", TestOutcome.Warning)]
    [Arguments("Completed", TestOutcome.Completed)]
    [Arguments("InProgress", TestOutcome.InProgress)]
    [Arguments("Pending", TestOutcome.Pending)]
    [Arguments("AdapterSpecificOutcome", TestOutcome.Unknown)]
    [Arguments("999", TestOutcome.Unknown)]
    public async Task Parses_Unit_Test_Outcomes_Defensively(string trxOutcome, TestOutcome expectedOutcome)
    {
        var document = XDocument.Load(TrxFixturePath);
        var passResult = document.Descendants()
            .Single(element => element.Name.LocalName == "UnitTestResult"
                && element.Attribute("testName")?.Value == "Pass");
        passResult.SetAttributeValue("outcome", trxOutcome);

        var result = new TrxParser().ParseTrxContents(document.ToString());

        await Assert.That(result.UnitTestResults).Count().IsEqualTo(4);
        await Assert.That(result.UnitTestResults.Single(x => x.TestName == "Pass").Outcome)
            .IsEqualTo(expectedOutcome);
        await Assert.That(result.UnitTestResults.Single(x => x.TestName == "Pass2").Outcome)
            .IsEqualTo(TestOutcome.Passed);
        await Assert.That(result.UnitTestResults.Single(x => x.TestName == "Fail").Outcome)
            .IsEqualTo(TestOutcome.Failed);
    }

    [Test]
    public async Task Parses_Only_Top_Level_Results()
    {
        var result = await ParseFixture("nested-results.trx");

        await Assert.That(result.UnitTestResults).Count().IsEqualTo(2);
        await Assert.That(result.UnitTestResults[0].TestName).IsEqualTo("ParentWithOutput");
        await Assert.That(result.UnitTestResults[1].TestName).IsEqualTo("ParentWithoutOutput");
    }

    [Test]
    public async Task Parses_Parents_Own_Output()
    {
        var result = await ParseFixture("nested-results.trx");
        var parent = result.UnitTestResults.Single(x => x.TestName == "ParentWithOutput");

        await Assert.That(parent.Output?.StdOut).IsEqualTo("Parent stdout");
        await Assert.That(parent.Output?.DebugTrace).IsEqualTo("Parent debug trace");
        await Assert.That(parent.Output?.ErrorInfo?.Message).IsEqualTo("Parent error");
        await Assert.That(parent.Output?.ErrorInfo?.StackTrace).IsEqualTo("Parent stack trace");
        await Assert.That(parent.Duration).IsEqualTo(TimeSpan.FromSeconds(1));
        await Assert.That(parent.StartTime).IsEqualTo(new DateTimeOffset(2026, 10, 23, 12, 0, 0, TimeSpan.Zero));
        await Assert.That(parent.EndTime).IsEqualTo(new DateTimeOffset(2026, 10, 23, 12, 0, 1, TimeSpan.Zero));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Does_Not_Inherit_Child_Output(bool addEmptyOutput)
    {
        var document = XDocument.Load(Path.Combine(AppContext.BaseDirectory, "Data", "nested-results.trx"));
        var parentElement = document.Descendants()
            .Single(x => x.Name.LocalName == "UnitTestResult" && x.Attribute("testName")?.Value == "ParentWithoutOutput");
        if (addEmptyOutput)
        {
            parentElement.Add(new XElement(parentElement.Name.Namespace + "Output"));
        }

        var result = new TrxParser().ParseTrxContents(document.ToString());
        var parent = result.UnitTestResults.Single(x => x.TestName == "ParentWithoutOutput");

        await Assert.That(parent.Output?.StdOut).IsNull();
        await Assert.That(parent.Output?.DebugTrace).IsNull();
        await Assert.That(parent.Output?.ErrorInfo).IsNull();
    }

    [Test]
    public async Task Parses_Missing_Result_Attributes_As_Null()
    {
        var result = await ParseFixture("missing-attributes.trx");
        var test = result.UnitTestResults.Single();

        await Assert.That(test.ExecutionId).IsNull();
        await Assert.That(test.TestId).IsNull();
        await Assert.That(test.TestName).IsNull();
        await Assert.That(test.ComputerName).IsNull();
        await Assert.That(test.Duration).IsNull();
        await Assert.That(test.StartTime).IsNull();
        await Assert.That(test.EndTime).IsNull();
        await Assert.That(test.TestType).IsNull();
        await Assert.That(test.TestListId).IsNull();
        await Assert.That(test.RelativeResultsDirectory).IsNull();
        await Assert.That(test.Outcome).IsEqualTo(TestOutcome.Unknown);
    }

    private static async Task<DotNetTestResult> ParseFixture(string fileName = "test-results.trx")
    {
        var contents = await System.IO.File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Data", fileName));
        return new TrxParser().ParseTrxContents(contents);
    }
}
