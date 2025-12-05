using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediastore", "get-lifecycle-policy")]
public record AwsMediastoreGetLifecyclePolicyOptions(
[property: CliOption("--container-name")] string ContainerName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}