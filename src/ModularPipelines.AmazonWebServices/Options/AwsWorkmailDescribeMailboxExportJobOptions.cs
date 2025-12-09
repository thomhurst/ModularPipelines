using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "describe-mailbox-export-job")]
public record AwsWorkmailDescribeMailboxExportJobOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}