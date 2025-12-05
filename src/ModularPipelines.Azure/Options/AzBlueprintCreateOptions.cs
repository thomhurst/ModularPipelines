using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blueprint", "create")]
public record AzBlueprintCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--target-scope")] string TargetScope
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--resource-groups")]
    public string? ResourceGroups { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}