using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-studio-lifecycle-config")]
public record AwsSagemakerDescribeStudioLifecycleConfigOptions(
[property: CliOption("--studio-lifecycle-config-name")] string StudioLifecycleConfigName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}