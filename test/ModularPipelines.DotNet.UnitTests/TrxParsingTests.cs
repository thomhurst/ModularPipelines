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

    private static async Task<DotNetTestResult> ParseFixture()
    {
        var contents = await System.IO.File.ReadAllTextAsync(TrxFixturePath);
        return new TrxParser().ParseTrxContents(contents);
    }
}
