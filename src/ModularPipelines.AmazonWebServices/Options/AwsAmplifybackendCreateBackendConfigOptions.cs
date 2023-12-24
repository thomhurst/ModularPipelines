using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifybackend", "create-backend-config")]
public record AwsAmplifybackendCreateBackendConfigOptions(
[property: CommandSwitch("--app-id")] string AppId
) : AwsOptions
{
    [CommandSwitch("--backend-manager-app-id")]
    public string? BackendManagerAppId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}