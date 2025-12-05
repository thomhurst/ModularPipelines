using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifybackend", "delete-backend-auth")]
public record AwsAmplifybackendDeleteBackendAuthOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--backend-environment-name")] string BackendEnvironmentName,
[property: CliOption("--resource-name")] string ResourceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}