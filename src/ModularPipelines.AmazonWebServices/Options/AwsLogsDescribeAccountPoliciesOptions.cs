using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "describe-account-policies")]
public record AwsLogsDescribeAccountPoliciesOptions(
[property: CliOption("--policy-type")] string PolicyType
) : AwsOptions
{
    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--account-identifiers")]
    public string[]? AccountIdentifiers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}