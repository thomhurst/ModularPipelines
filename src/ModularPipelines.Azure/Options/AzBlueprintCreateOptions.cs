using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blueprint", "create")]
public record AzBlueprintCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--target-scope")] string TargetScope
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--management-group")]
    public string? ManagementGroup { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--resource-groups")]
    public string? ResourceGroups { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}