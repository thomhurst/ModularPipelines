using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "create-run-group")]
public record AwsOmicsCreateRunGroupOptions : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--max-cpus")]
    public int? MaxCpus { get; set; }

    [CliOption("--max-runs")]
    public int? MaxRuns { get; set; }

    [CliOption("--max-duration")]
    public int? MaxDuration { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--request-id")]
    public string? RequestId { get; set; }

    [CliOption("--max-gpus")]
    public int? MaxGpus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}