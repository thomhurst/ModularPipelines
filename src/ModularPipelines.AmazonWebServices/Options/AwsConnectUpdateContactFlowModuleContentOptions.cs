using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-contact-flow-module-content")]
public record AwsConnectUpdateContactFlowModuleContentOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-flow-module-id")] string ContactFlowModuleId,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}