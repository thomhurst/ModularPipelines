using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudhsmv2", "tag-resource")]
public record AwsCloudhsmv2TagResourceOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--tag-list")] string[] TagList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}