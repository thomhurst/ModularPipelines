using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-studio-lifecycle-config")]
public record AwsSagemakerCreateStudioLifecycleConfigOptions(
[property: CliOption("--studio-lifecycle-config-name")] string StudioLifecycleConfigName,
[property: CliOption("--studio-lifecycle-config-content")] string StudioLifecycleConfigContent,
[property: CliOption("--studio-lifecycle-config-app-type")] string StudioLifecycleConfigAppType
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}