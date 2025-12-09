using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-contact-flow-module-metadata")]
public record AwsConnectUpdateContactFlowModuleMetadataOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-flow-module-id")] string ContactFlowModuleId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}