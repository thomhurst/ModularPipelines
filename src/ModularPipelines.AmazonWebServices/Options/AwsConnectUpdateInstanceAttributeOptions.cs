using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-instance-attribute")]
public record AwsConnectUpdateInstanceAttributeOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--attribute-type")] string AttributeType,
[property: CliOption("--value")] string Value
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}