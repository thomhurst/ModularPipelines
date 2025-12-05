using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "update-intent")]
public record AwsLexv2ModelsUpdateIntentOptions(
[property: CliOption("--intent-id")] string IntentId,
[property: CliOption("--intent-name")] string IntentName,
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--locale-id")] string LocaleId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--parent-intent-signature")]
    public string? ParentIntentSignature { get; set; }

    [CliOption("--sample-utterances")]
    public string[]? SampleUtterances { get; set; }

    [CliOption("--dialog-code-hook")]
    public string? DialogCodeHook { get; set; }

    [CliOption("--fulfillment-code-hook")]
    public string? FulfillmentCodeHook { get; set; }

    [CliOption("--slot-priorities")]
    public string[]? SlotPriorities { get; set; }

    [CliOption("--intent-confirmation-setting")]
    public string? IntentConfirmationSetting { get; set; }

    [CliOption("--intent-closing-setting")]
    public string? IntentClosingSetting { get; set; }

    [CliOption("--input-contexts")]
    public string[]? InputContexts { get; set; }

    [CliOption("--output-contexts")]
    public string[]? OutputContexts { get; set; }

    [CliOption("--kendra-configuration")]
    public string? KendraConfiguration { get; set; }

    [CliOption("--initial-response-setting")]
    public string? InitialResponseSetting { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}