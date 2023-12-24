using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifybackend", "update-backend-api")]
public record AwsAmplifybackendUpdateBackendApiOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--backend-environment-name")] string BackendEnvironmentName,
[property: CommandSwitch("--resource-name")] string ResourceName
) : AwsOptions
{
    [CommandSwitch("--resource-config")]
    public string? ResourceConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}