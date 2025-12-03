using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "send-bonus")]
public record AwsMturkSendBonusOptions(
[property: CliOption("--worker-id")] string WorkerId,
[property: CliOption("--bonus-amount")] string BonusAmount,
[property: CliOption("--assignment-id")] string AssignmentId,
[property: CliOption("--reason")] string Reason
) : AwsOptions
{
    [CliOption("--unique-request-token")]
    public string? UniqueRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}