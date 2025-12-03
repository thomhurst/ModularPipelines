using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "perimeter", "profile", "access-rule", "delete")]
public record AzNetworkPerimeterProfileAccessRuleDeleteOptions : AzOptions
{
    [CliOption("--access-rule-name")]
    public string? AccessRuleName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--perimeter-name")]
    public string? PerimeterName { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}