using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "patch", "list")]
public record AzContainerappPatchListOptions : AzOptions
{
    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--show-all")]
    public bool? ShowAll { get; set; }
}