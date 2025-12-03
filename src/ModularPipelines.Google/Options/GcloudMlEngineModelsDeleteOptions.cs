using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "models", "delete")]
public record GcloudMlEngineModelsDeleteOptions(
[property: CliArgument] string Model
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}