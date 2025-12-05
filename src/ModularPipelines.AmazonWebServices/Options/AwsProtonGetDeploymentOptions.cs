using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "get-deployment")]
public record AwsProtonGetDeploymentOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--component-name")]
    public string? ComponentName { get; set; }

    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--service-instance-name")]
    public string? ServiceInstanceName { get; set; }

    [CliOption("--service-name")]
    public string? ServiceName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}