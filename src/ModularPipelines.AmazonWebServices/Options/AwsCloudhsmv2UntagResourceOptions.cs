using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudhsmv2", "untag-resource")]
public record AwsCloudhsmv2UntagResourceOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--tag-key-list")] string[] TagKeyList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}