using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifybackend", "create-backend-storage")]
public record AwsAmplifybackendCreateBackendStorageOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--backend-environment-name")] string BackendEnvironmentName,
[property: CommandSwitch("--resource-config")] string ResourceConfig,
[property: CommandSwitch("--resource-name")] string ResourceName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}