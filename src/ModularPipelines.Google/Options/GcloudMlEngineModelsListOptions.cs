using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "models", "list")]
public record GcloudMlEngineModelsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}