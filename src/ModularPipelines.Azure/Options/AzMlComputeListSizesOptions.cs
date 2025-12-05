using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "compute", "list-sizes")]
public record AzMlComputeListSizesOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}