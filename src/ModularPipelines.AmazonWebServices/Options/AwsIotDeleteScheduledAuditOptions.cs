using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "delete-scheduled-audit")]
public record AwsIotDeleteScheduledAuditOptions(
[property: CommandSwitch("--scheduled-audit-name")] string ScheduledAuditName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}