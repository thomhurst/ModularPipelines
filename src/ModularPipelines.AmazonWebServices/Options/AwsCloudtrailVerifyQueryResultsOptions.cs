using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "verify-query-results")]
public record AwsCloudtrailVerifyQueryResultsOptions : AwsOptions
{
    [CliOption("--local-export-path")]
    public string? LocalExportPath { get; set; }

    [CliOption("--s3-bucket")]
    public string? S3Bucket { get; set; }

    [CliOption("--s3-prefix")]
    public string? S3Prefix { get; set; }
}