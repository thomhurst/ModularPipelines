using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack", "mg", "create")]
public record AzStackMgCreateOptions(
[property: CommandSwitch("--deny-settings-mode")] string DenySettingsMode,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--management-group-id")] string ManagementGroupId,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--cs")]
    public bool? Cs { get; set; }

    [BooleanCommandSwitch("--delete-all")]
    public bool? DeleteAll { get; set; }

    [BooleanCommandSwitch("--delete-resource-groups")]
    public bool? DeleteResourceGroups { get; set; }

    [BooleanCommandSwitch("--delete-resources")]
    public bool? DeleteResources { get; set; }

    [CommandSwitch("--deny-settings-excluded-actions")]
    public string? DenySettingsExcludedActions { get; set; }

    [CommandSwitch("--deny-settings-excluded-principals")]
    public string? DenySettingsExcludedPrincipals { get; set; }

    [CommandSwitch("--deployment-subscription")]
    public string? DeploymentSubscription { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--query-string")]
    public string? QueryString { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--template-file")]
    public string? TemplateFile { get; set; }

    [CommandSwitch("--template-spec")]
    public string? TemplateSpec { get; set; }

    [CommandSwitch("--template-uri")]
    public string? TemplateUri { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

