using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-delivery-source")]
public record AwsLogsPutDeliverySourceOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--log-type")] string LogType
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}