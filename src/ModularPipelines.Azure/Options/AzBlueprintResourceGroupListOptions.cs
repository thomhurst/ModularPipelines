using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blueprint", "resource-group", "list")]
public record AzBlueprintResourceGroupListOptions(
[property: CliOption("--blueprint-name")] string BlueprintName
) : AzOptions
{
    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}