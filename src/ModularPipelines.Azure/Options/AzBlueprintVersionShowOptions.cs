using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blueprint", "version", "show")]
public record AzBlueprintVersionShowOptions(
[property: CliOption("--blueprint-name")] string BlueprintName,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}