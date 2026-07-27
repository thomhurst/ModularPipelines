using Microsoft.Extensions.Logging;
using ModularPipelines.Engine.BuildSystemFormatters;

namespace ModularPipelines.UnitTests.Engine;

public class AzurePipelinesFormatterTests
{
    [Test]
    [Arguments(LogLevel.Warning, "warning")]
    [Arguments(LogLevel.Error, "error")]
    [Arguments(LogLevel.Critical, "error")]
    public async Task GetLogIssueCommand_MapsIssueSeverities(LogLevel logLevel, string issueType)
    {
        var command = new AzurePipelinesFormatter().GetLogIssueCommand(logLevel, "message");

        await Assert.That(command).IsEqualTo($"##vso[task.logissue type={issueType};]message");
    }

    [Test]
    [Arguments(LogLevel.Trace)]
    [Arguments(LogLevel.Debug)]
    [Arguments(LogLevel.Information)]
    [Arguments(LogLevel.None)]
    public async Task GetLogIssueCommand_IgnoresNonIssueSeverities(LogLevel logLevel)
    {
        var command = new AzurePipelinesFormatter().GetLogIssueCommand(logLevel, "message");

        await Assert.That(command).IsNull();
    }

    [Test]
    public async Task GetLogIssueCommand_EscapesReservedCharacters()
    {
        var command = new AzurePipelinesFormatter()
            .GetLogIssueCommand(LogLevel.Warning, "50%;]\r\nnext");

        await Assert.That(command)
            .IsEqualTo("##vso[task.logissue type=warning;]50%AZP25%3B%5D%0D%0Anext");
    }
}
