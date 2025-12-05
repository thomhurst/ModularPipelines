using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifybackend", "get-backend")]
public record AwsAmplifybackendGetBackendOptions(
[property: CliOption("--app-id")] string AppId
) : AwsOptions
{
    [CliOption("--backend-environment-name")]
    public string? BackendEnvironmentName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}