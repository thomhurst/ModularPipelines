using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "put-lifecycle-configuration")]
public record AwsEfsPutLifecycleConfigurationOptions(
[property: CliOption("--file-system-id")] string FileSystemId,
[property: CliOption("--lifecycle-policies")] string[] LifecyclePolicies
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}