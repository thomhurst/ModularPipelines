using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifybackend", "create-backend")]
public record AwsAmplifybackendCreateBackendOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--app-name")] string AppName,
[property: CommandSwitch("--backend-environment-name")] string BackendEnvironmentName
) : AwsOptions
{
    [CommandSwitch("--resource-config")]
    public string? ResourceConfig { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}