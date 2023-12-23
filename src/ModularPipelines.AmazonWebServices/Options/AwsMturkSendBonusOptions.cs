using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "send-bonus")]
public record AwsMturkSendBonusOptions(
[property: CommandSwitch("--worker-id")] string WorkerId,
[property: CommandSwitch("--bonus-amount")] string BonusAmount,
[property: CommandSwitch("--assignment-id")] string AssignmentId,
[property: CommandSwitch("--reason")] string Reason
) : AwsOptions
{
    [CommandSwitch("--unique-request-token")]
    public string? UniqueRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}