using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "set-endpoint-attributes")]
public record AwsSnsSetEndpointAttributesOptions(
[property: CliOption("--endpoint-arn")] string EndpointArn,
[property: CliOption("--attributes")] IEnumerable<KeyValue> Attributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}