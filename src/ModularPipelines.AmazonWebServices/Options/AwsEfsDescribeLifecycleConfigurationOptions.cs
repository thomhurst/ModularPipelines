using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "describe-lifecycle-configuration")]
public record AwsEfsDescribeLifecycleConfigurationOptions(
[property: CliOption("--file-system-id")] string FileSystemId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}