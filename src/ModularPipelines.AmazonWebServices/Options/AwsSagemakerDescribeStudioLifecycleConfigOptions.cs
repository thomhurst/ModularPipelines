using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-studio-lifecycle-config")]
public record AwsSagemakerDescribeStudioLifecycleConfigOptions(
[property: CommandSwitch("--studio-lifecycle-config-name")] string StudioLifecycleConfigName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}