using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "cancel-service-pipeline-deployment")]
public record AwsProtonCancelServicePipelineDeploymentOptions(
[property: CliOption("--service-name")] string ServiceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}