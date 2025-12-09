using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "cancel-mailbox-export-job")]
public record AwsWorkmailCancelMailboxExportJobOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}