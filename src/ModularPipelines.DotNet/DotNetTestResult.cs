using ModularPipelines.DotNet.Enums;
using ModularPipelines.DotNet.Parsers.NUnitTrx;

namespace ModularPipelines.DotNet;

public record DotNetTestResult(IReadOnlyList<UnitTestResult> UnitTestResults)
{
    public bool Successful => UnitTestResults.All(x => x.Outcome != TestOutcome.Failed);
}