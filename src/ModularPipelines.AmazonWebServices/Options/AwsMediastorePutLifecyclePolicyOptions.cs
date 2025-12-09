using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediastore", "put-lifecycle-policy")]
public record AwsMediastorePutLifecyclePolicyOptions(
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--lifecycle-policy")] string LifecyclePolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}