using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "event", "list")]
public record AzPolicyEventListOptions : AzOptions
{
    [CommandSwitch("--apply")]
    public string? Apply { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--from")]
    public string? From { get; set; }

    [CommandSwitch("--management-group")]
    public string? ManagementGroup { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--order-by")]
    public string? OrderBy { get; set; }

    [CommandSwitch("--parent")]
    public string? Parent { get; set; }

    [CommandSwitch("--policy-assignment")]
    public string? PolicyAssignment { get; set; }

    [CommandSwitch("--policy-definition")]
    public string? PolicyDefinition { get; set; }

    [CommandSwitch("--policy-set-definition")]
    public string? PolicySetDefinition { get; set; }

    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--select")]
    public string? Select { get; set; }

    [CommandSwitch("--to")]
    public string? To { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}