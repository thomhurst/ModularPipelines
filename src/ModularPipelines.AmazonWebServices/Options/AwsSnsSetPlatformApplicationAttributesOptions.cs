using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "set-platform-application-attributes")]
public record AwsSnsSetPlatformApplicationAttributesOptions(
[property: CliOption("--platform-application-arn")] string PlatformApplicationArn,
[property: CliOption("--attributes")] IEnumerable<KeyValue> Attributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}