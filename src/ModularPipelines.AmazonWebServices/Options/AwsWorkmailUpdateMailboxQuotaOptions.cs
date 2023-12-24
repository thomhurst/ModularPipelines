using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "update-mailbox-quota")]
public record AwsWorkmailUpdateMailboxQuotaOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--mailbox-quota")] int MailboxQuota
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}