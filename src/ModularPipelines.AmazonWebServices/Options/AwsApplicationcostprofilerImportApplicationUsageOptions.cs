using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("applicationcostprofiler", "import-application-usage")]
public record AwsApplicationcostprofilerImportApplicationUsageOptions(
[property: CliOption("--source-s3-location")] string SourceS3Location
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}