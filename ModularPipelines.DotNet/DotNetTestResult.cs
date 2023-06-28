namespace ModularPipelines.DotNet;

public record DotNetTestResult(IReadOnlyList<UnitTestResult> UnitTestResults)
{
}