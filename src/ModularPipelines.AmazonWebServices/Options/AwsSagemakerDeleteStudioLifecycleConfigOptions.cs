using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "delete-studio-lifecycle-config")]
public record AwsSagemakerDeleteStudioLifecycleConfigOptions(
[property: CommandSwitch("--studio-lifecycle-config-name")] string StudioLifecycleConfigName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}