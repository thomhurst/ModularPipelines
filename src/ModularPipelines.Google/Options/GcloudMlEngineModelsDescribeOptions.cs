using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "models", "describe")]
public record GcloudMlEngineModelsDescribeOptions(
[property: CliArgument] string Model
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}