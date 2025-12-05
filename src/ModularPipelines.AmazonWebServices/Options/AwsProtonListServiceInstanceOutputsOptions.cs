using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "list-service-instance-outputs")]
public record AwsProtonListServiceInstanceOutputsOptions(
[property: CliOption("--service-instance-name")] string ServiceInstanceName,
[property: CliOption("--service-name")] string ServiceName
) : AwsOptions
{
    [CliOption("--deployment-id")]
    public string? DeploymentId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}