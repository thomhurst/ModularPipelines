using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "workspace-hub", "delete")]
public record AzMlWorkspaceHubDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--all-resources")]
    public bool? AllResources { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--permanently-delete")]
    public bool? PermanentlyDelete { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

