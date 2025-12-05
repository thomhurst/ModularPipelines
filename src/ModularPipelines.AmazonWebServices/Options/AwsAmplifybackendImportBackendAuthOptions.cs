using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifybackend", "import-backend-auth")]
public record AwsAmplifybackendImportBackendAuthOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--backend-environment-name")] string BackendEnvironmentName,
[property: CliOption("--native-client-id")] string NativeClientId,
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--web-client-id")] string WebClientId
) : AwsOptions
{
    [CliOption("--identity-pool-id")]
    public string? IdentityPoolId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}