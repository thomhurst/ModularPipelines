using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-studio-lifecycle-config")]
public record AwsSagemakerCreateStudioLifecycleConfigOptions(
[property: CommandSwitch("--studio-lifecycle-config-name")] string StudioLifecycleConfigName,
[property: CommandSwitch("--studio-lifecycle-config-content")] string StudioLifecycleConfigContent,
[property: CommandSwitch("--studio-lifecycle-config-app-type")] string StudioLifecycleConfigAppType
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}