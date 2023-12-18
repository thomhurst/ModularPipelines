using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "mg", "list")]
public record AzDeploymentMgListOptions(
[property: CommandSwitch("--management-group-id")] string ManagementGroupId
) : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}