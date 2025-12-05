using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "describe-slot-type")]
public record AwsLexv2ModelsDescribeSlotTypeOptions(
[property: CliOption("--slot-type-id")] string SlotTypeId,
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--locale-id")] string LocaleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}