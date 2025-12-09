using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "describe-effective-policy")]
public record AwsOrganizationsDescribeEffectivePolicyOptions(
[property: CliOption("--policy-type")] string PolicyType
) : AwsOptions
{
    [CliOption("--target-id")]
    public string? TargetId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}