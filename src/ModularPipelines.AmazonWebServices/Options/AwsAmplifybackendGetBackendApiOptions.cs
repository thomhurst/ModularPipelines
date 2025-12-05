using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifybackend", "get-backend-api")]
public record AwsAmplifybackendGetBackendApiOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--backend-environment-name")] string BackendEnvironmentName,
[property: CliOption("--resource-name")] string ResourceName
) : AwsOptions
{
    [CliOption("--resource-config")]
    public string? ResourceConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}