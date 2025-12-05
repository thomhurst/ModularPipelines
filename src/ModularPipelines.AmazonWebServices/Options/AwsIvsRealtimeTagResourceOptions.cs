using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs-realtime", "tag-resource")]
public record AwsIvsRealtimeTagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--tags")] IEnumerable<KeyValue> Tags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}