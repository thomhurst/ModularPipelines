using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "delete-blue-green-deployment")]
public record AwsRdsDeleteBlueGreenDeploymentOptions(
[property: CliOption("--blue-green-deployment-identifier")] string BlueGreenDeploymentIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}