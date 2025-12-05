using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "describe-slot")]
public record AwsLexv2ModelsDescribeSlotOptions(
[property: CliOption("--slot-id")] string SlotId,
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--intent-id")] string IntentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}