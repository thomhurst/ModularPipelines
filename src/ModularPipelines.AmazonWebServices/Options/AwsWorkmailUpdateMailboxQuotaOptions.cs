using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "update-mailbox-quota")]
public record AwsWorkmailUpdateMailboxQuotaOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--mailbox-quota")] int MailboxQuota
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}