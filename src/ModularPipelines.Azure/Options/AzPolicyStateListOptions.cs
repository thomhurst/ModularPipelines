using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("policy", "state", "list")]
public record AzPolicyStateListOptions : AzOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliOption("--apply")]
    public string? Apply { get; set; }

    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--from")]
    public string? From { get; set; }

    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--order-by")]
    public string? OrderBy { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }

    [CliOption("--policy-assignment")]
    public string? PolicyAssignment { get; set; }

    [CliOption("--policy-definition")]
    public string? PolicyDefinition { get; set; }

    [CliOption("--policy-set-definition")]
    public string? PolicySetDefinition { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--select")]
    public string? Select { get; set; }

    [CliOption("--to")]
    public string? To { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}