using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "config", "access-restriction", "set")]
public record AzWebappConfigAccessRestrictionSetOptions(
[property: CliFlag("--use-same-restrictions-for-scm-site")] bool UseSameRestrictionsForScmSite
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}