using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "update-compute-environment")]
public record AwsBatchUpdateComputeEnvironmentOptions(
[property: CliOption("--compute-environment")] string ComputeEnvironment
) : AwsOptions
{
    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--unmanagedv-cpus")]
    public int? UnmanagedvCpus { get; set; }

    [CliOption("--compute-resources")]
    public string? ComputeResources { get; set; }

    [CliOption("--service-role")]
    public string? ServiceRole { get; set; }

    [CliOption("--update-policy")]
    public string? UpdatePolicy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}