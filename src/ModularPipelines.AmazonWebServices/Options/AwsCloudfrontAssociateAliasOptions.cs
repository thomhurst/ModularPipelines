using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "associate-alias")]
public record AwsCloudfrontAssociateAliasOptions(
[property: CliOption("--target-distribution-id")] string TargetDistributionId,
[property: CliOption("--alias")] string Alias
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}