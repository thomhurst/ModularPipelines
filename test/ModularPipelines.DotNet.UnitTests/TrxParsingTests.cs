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

    private static async Task<DotNetTestResult> ParseFixture()
    {
        var contents = await System.IO.File.ReadAllTextAsync(TrxFixturePath);
        return new TrxParser().ParseTrxContents(contents);
    }
}
