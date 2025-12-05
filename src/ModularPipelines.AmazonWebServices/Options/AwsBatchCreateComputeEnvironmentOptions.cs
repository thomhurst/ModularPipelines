using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "create-compute-environment")]
public record AwsBatchCreateComputeEnvironmentOptions(
[property: CliOption("--compute-environment-name")] string ComputeEnvironmentName,
[property: CliOption("--type")] string Type
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

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--eks-configuration")]
    public string? EksConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}