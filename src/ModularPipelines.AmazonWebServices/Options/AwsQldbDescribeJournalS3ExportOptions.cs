using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qldb", "describe-journal-s3-export")]
public record AwsQldbDescribeJournalS3ExportOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--export-id")] string ExportId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}