using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "tag-resource")]
public record AwsGlueTagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--tags-to-add")] IEnumerable<KeyValue> TagsToAdd
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}