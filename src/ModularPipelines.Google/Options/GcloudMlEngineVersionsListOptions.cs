using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "versions", "list")]
public record GcloudMlEngineVersionsListOptions(
[property: CliOption("--model")] string Model
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}