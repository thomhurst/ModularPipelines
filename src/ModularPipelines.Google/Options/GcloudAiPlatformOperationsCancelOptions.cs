using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "operations", "cancel")]
public record GcloudAiPlatformOperationsCancelOptions(
[property: CliArgument] string Operation
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}