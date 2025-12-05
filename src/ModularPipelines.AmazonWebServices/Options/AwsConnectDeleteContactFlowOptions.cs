using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-contact-flow")]
public record AwsConnectDeleteContactFlowOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-flow-id")] string ContactFlowId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}