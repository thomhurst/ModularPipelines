using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "update-run-group")]
public record AwsOmicsUpdateRunGroupOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--max-cpus")]
    public int? MaxCpus { get; set; }

    [CliOption("--max-runs")]
    public int? MaxRuns { get; set; }

    [CliOption("--max-duration")]
    public int? MaxDuration { get; set; }

    [CliOption("--max-gpus")]
    public int? MaxGpus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}