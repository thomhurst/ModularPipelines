using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-service", "role-instance", "show-remote-desktop-file")]
public record AzCloudServiceRoleInstanceShowRemoteDesktopFileOptions : AzOptions
{
    [CommandSwitch("--cloud-service-name")]
    public string? CloudServiceName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--role-instance-name")]
    public string? RoleInstanceName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

