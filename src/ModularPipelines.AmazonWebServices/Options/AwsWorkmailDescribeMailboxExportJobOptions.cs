using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "describe-mailbox-export-job")]
public record AwsWorkmailDescribeMailboxExportJobOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}