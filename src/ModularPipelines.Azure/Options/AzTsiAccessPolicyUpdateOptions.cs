using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("tsi", "access-policy", "update")]
public record AzTsiAccessPolicyUpdateOptions : AzOptions
{
    [CliOption("--access-policy-name")]
    public string? AccessPolicyName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--roles")]
    public string? Roles { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}