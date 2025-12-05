using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "describe-contact-flow")]
public record AwsConnectDescribeContactFlowOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-flow-id")] string ContactFlowId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}