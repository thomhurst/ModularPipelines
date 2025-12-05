using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "list-tags-for-resources")]
public record AwsRoute53ListTagsForResourcesOptions(
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--resource-ids")] string[] ResourceIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}