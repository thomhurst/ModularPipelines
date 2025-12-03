using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifybackend", "create-backend")]
public record AwsAmplifybackendCreateBackendOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--app-name")] string AppName,
[property: CliOption("--backend-environment-name")] string BackendEnvironmentName
) : AwsOptions
{
    [CliOption("--resource-config")]
    public string? ResourceConfig { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}