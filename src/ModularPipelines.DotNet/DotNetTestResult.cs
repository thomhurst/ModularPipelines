using ModularPipelines.DotNet.Parsers.Trx;

namespace ModularPipelines.DotNet;

public record DotNetTestResult(IReadOnlyList<UnitTestResult> UnitTestResults, ResultSummary ResultSummary)
{
    public bool Successful => ResultSummary.Outcome is "Passed" or "Completed";
}