using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "delete-compute-environment")]
public record AwsBatchDeleteComputeEnvironmentOptions(
[property: CliOption("--compute-environment")] string ComputeEnvironment
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}