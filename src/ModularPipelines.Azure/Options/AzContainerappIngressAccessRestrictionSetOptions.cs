using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "ingress", "access-restriction", "set")]
public record AzContainerappIngressAccessRestrictionSetOptions(
[property: CliOption("--action")] string Action,
[property: CliOption("--ip-address")] string IpAddress,
[property: CliOption("--rule-name")] string RuleName
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}