using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-service", "rebuild")]
public record AzCloudServiceRebuildOptions : AzOptions
{
    [CommandSwitch("--cloud-service-name")]
    public string? CloudServiceName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--role-instances")]
    public string? RoleInstances { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}