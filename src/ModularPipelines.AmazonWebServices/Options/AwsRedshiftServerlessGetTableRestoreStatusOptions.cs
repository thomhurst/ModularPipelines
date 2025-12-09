using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "get-table-restore-status")]
public record AwsRedshiftServerlessGetTableRestoreStatusOptions(
[property: CliOption("--table-restore-request-id")] string TableRestoreRequestId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}