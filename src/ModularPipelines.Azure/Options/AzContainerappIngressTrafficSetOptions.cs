using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "ingress", "traffic", "set")]
public record AzContainerappIngressTrafficSetOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--label-weight")]
    public string? LabelWeight { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--revision-weight")]
    public string? RevisionWeight { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}