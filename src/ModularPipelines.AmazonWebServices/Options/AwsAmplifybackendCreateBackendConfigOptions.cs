using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifybackend", "create-backend-config")]
public record AwsAmplifybackendCreateBackendConfigOptions(
[property: CliOption("--app-id")] string AppId
) : AwsOptions
{
    [CliOption("--backend-manager-app-id")]
    public string? BackendManagerAppId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}