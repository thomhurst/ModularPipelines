using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile", "create-project")]
public record AwsMobileCreateProjectOptions : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--contents")]
    public string? Contents { get; set; }

    [CliOption("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CliOption("--project-region")]
    public string? ProjectRegion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}