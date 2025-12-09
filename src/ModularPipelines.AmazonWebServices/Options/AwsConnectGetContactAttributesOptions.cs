using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "get-contact-attributes")]
public record AwsConnectGetContactAttributesOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--initial-contact-id")] string InitialContactId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}