using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "models", "list")]
public record GcloudAiPlatformModelsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}