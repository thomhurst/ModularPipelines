using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifybackend", "clone-backend")]
public record AwsAmplifybackendCloneBackendOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--backend-environment-name")] string BackendEnvironmentName,
[property: CommandSwitch("--target-environment-name")] string TargetEnvironmentName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}