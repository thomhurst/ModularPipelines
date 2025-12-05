using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "tag-resource")]
public record AwsPinpointTagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--tags-model")] string TagsModel
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}