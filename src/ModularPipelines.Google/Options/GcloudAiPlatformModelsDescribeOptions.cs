using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "models", "describe")]
public record GcloudAiPlatformModelsDescribeOptions(
[property: CliArgument] string Model
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}