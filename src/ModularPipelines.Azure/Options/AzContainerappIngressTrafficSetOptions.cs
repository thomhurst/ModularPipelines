using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "ingress", "traffic", "set")]
public record AzContainerappIngressTrafficSetOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--label-weight")]
    public string? LabelWeight { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--revision-weight")]
    public string? RevisionWeight { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}