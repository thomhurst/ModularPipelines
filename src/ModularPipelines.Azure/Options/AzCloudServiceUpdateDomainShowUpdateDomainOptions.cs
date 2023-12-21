using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-service", "update-domain", "show-update-domain")]
public record AzCloudServiceUpdateDomainShowUpdateDomainOptions : AzOptions
{
    [CommandSwitch("--cloud-service-name")]
    public string? CloudServiceName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--update-domain")]
    public string? UpdateDomain { get; set; }
}