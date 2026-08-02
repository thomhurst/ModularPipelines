using ModularPipelines.DotNet;
using ModularPipelines.DotNet.Enums;
using ModularPipelines.DotNet.Parsers.Trx;

namespace ModularPipelines.UnitTests.Models;

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

    private static async Task<DotNetTestResult> ParseFixture()
    {
        var contents = await System.IO.File.ReadAllTextAsync(TrxFixturePath);
        return new TrxParser().ParseTrxContents(contents);
    }
}
