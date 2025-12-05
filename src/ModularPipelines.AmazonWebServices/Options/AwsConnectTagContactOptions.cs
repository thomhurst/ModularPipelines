using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "tag-contact")]
public record AwsConnectTagContactOptions(
[property: CliOption("--contact-id")] string ContactId,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--tags")] IEnumerable<KeyValue> Tags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}