using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "mg", "list")]
public record AzDeploymentMgListOptions(
[property: CommandSwitch("--management-group-id")] string ManagementGroupId
) : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}

