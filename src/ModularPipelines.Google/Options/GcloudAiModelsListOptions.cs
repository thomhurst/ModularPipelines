using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "models", "list")]
public record GcloudAiModelsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}