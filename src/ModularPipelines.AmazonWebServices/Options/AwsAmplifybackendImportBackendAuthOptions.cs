using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifybackend", "import-backend-auth")]
public record AwsAmplifybackendImportBackendAuthOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--backend-environment-name")] string BackendEnvironmentName,
[property: CommandSwitch("--native-client-id")] string NativeClientId,
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--web-client-id")] string WebClientId
) : AwsOptions
{
    [CommandSwitch("--identity-pool-id")]
    public string? IdentityPoolId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}