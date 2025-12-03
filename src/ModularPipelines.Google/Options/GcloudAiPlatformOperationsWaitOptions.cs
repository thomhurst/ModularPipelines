using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "operations", "wait")]
public record GcloudAiPlatformOperationsWaitOptions(
[property: CliArgument] string Operation
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}