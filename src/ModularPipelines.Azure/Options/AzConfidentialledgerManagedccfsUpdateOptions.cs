using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("confidentialledger", "managedccfs", "update")]
public record AzConfidentialledgerManagedccfsUpdateOptions : AzOptions
{
    [CliOption("--deployment-type")]
    public string? DeploymentType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--member-certificates")]
    public string? MemberCertificates { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}