using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "operations", "list")]
public record GcloudAiPlatformOperationsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}