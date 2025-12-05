using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-contact-flow-content")]
public record AwsConnectUpdateContactFlowContentOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-flow-id")] string ContactFlowId,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}