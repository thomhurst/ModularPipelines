using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifybackend", "delete-backend")]
public record AwsAmplifybackendDeleteBackendOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--backend-environment-name")] string BackendEnvironmentName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}