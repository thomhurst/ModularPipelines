using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "describe-managed-rule-group")]
public record AwsWafv2DescribeManagedRuleGroupOptions(
[property: CliOption("--vendor-name")] string VendorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope
) : AwsOptions
{
    [CliOption("--version-name")]
    public string? VersionName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}