using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "list-available-managed-rule-group-versions")]
public record AwsWafv2ListAvailableManagedRuleGroupVersionsOptions(
[property: CliOption("--vendor-name")] string VendorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope
) : AwsOptions
{
    [CliOption("--next-marker")]
    public string? NextMarker { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}