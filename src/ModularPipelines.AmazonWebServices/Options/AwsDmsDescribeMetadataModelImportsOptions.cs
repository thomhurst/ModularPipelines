using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "describe-metadata-model-imports")]
public record AwsDmsDescribeMetadataModelImportsOptions(
[property: CliOption("--migration-project-identifier")] string MigrationProjectIdentifier
) : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--max-records")]
    public int? MaxRecords { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}