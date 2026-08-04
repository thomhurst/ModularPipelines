using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Compatibility;

public class StatusCompatibilityTests
{
    [Test]
    [Arguments(Status.NotYetStarted, 0)]
    [Arguments(Status.Processing, 1)]
    [Arguments(Status.Successful, 2)]
    [Arguments(Status.UsedHistory, 3)]
    [Arguments(Status.Failed, 4)]
    [Arguments(Status.IgnoredFailure, 5)]
    [Arguments(Status.PipelineTerminated, 6)]
    [Arguments(Status.TimedOut, 7)]
    [Arguments(Status.Skipped, 8)]
    [Arguments(Status.Retried, 9)]
    [Arguments(Status.Unknown, 10)]
    [Arguments(Status.CachedResult, 11)]
    [Arguments(Status.DependencyFailed, 12)]
    public async Task NumericValuesRemainStable(Status status, int expectedValue)
    {
        await Assert.That((int)status).IsEqualTo(expectedValue);
    }
}
