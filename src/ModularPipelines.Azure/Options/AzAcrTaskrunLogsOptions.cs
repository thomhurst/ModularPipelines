using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "taskrun", "logs")]
public record AzAcrTaskrunLogsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliFlag("--no-format")]
    public bool? NoFormat { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}