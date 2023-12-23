using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "associate-alias")]
public record AwsCloudfrontAssociateAliasOptions(
[property: CommandSwitch("--target-distribution-id")] string TargetDistributionId,
[property: CommandSwitch("--alias")] string Alias
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}