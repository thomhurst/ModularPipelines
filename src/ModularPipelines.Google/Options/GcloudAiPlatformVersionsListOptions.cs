using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "versions", "list")]
public record GcloudAiPlatformVersionsListOptions(
[property: CliOption("--model")] string Model
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}