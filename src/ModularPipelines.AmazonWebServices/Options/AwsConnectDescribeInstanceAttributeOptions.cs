using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "describe-instance-attribute")]
public record AwsConnectDescribeInstanceAttributeOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--attribute-type")] string AttributeType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}