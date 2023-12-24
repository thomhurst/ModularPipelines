using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "verify-query-results")]
public record AwsCloudtrailVerifyQueryResultsOptions : AwsOptions
{
    [CommandSwitch("--local-export-path")]
    public string? LocalExportPath { get; set; }

    [CommandSwitch("--s3-bucket")]
    public string? S3Bucket { get; set; }

    [CommandSwitch("--s3-prefix")]
    public string? S3Prefix { get; set; }
}