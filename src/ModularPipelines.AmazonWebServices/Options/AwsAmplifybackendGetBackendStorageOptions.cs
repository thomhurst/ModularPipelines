using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifybackend", "get-backend-storage")]
public record AwsAmplifybackendGetBackendStorageOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--backend-environment-name")] string BackendEnvironmentName,
[property: CliOption("--resource-name")] string ResourceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}