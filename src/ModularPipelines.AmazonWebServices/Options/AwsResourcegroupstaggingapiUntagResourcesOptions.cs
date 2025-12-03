using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resourcegroupstaggingapi", "untag-resources")]
public record AwsResourcegroupstaggingapiUntagResourcesOptions(
[property: CliOption("--resource-arn-list")] string[] ResourceArnList,
[property: CliOption("--tag-keys")] string[] TagKeys
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}