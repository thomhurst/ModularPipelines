using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("palo-alto", "cloudngfw", "local-rulestack", "create")]
public record AzPaloAltoCloudngfwLocalRulestackCreateOptions(
[property: CliOption("--local-rulestack-name")] string LocalRulestackName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--associated-subs")]
    public string? AssociatedSubs { get; set; }

    [CliOption("--default-mode")]
    public string? DefaultMode { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--min-app-id-version")]
    public string? MinAppIdVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pan-etag")]
    public string? PanEtag { get; set; }

    [CliOption("--pan-location")]
    public string? PanLocation { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--security-services")]
    public string? SecurityServices { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}