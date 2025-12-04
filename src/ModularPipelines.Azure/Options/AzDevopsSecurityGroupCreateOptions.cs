using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "security", "group", "create")]
public record AzDevopsSecurityGroupCreateOptions : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--email-id")]
    public string? EmailId { get; set; }

    [CliOption("--groups")]
    public string? Groups { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--origin-id")]
    public string? OriginId { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}