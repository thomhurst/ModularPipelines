using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "describe-contact-flow-module")]
public record AwsConnectDescribeContactFlowModuleOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-flow-module-id")] string ContactFlowModuleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}