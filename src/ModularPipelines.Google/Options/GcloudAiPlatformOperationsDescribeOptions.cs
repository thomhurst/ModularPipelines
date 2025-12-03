using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "operations", "describe")]
public record GcloudAiPlatformOperationsDescribeOptions(
[property: CliArgument] string Operation
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}