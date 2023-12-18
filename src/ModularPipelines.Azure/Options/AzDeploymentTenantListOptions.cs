using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "tenant", "list")]
public record AzDeploymentTenantListOptions : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}