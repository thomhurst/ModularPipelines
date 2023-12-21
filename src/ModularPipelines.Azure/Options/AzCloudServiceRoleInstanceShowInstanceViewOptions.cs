using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-service", "role-instance", "show-instance-view")]
public record AzCloudServiceRoleInstanceShowInstanceViewOptions : AzOptions
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
    public new string? Subscription { get; set; }
}