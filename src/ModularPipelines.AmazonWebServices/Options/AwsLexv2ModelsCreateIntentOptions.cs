using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "create-intent")]
public record AwsLexv2ModelsCreateIntentOptions(
[property: CommandSwitch("--intent-name")] string IntentName,
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-version")] string BotVersion,
[property: CommandSwitch("--locale-id")] string LocaleId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--parent-intent-signature")]
    public string? ParentIntentSignature { get; set; }

    [CommandSwitch("--sample-utterances")]
    public string[]? SampleUtterances { get; set; }

    [CommandSwitch("--dialog-code-hook")]
    public string? DialogCodeHook { get; set; }

    [CommandSwitch("--fulfillment-code-hook")]
    public string? FulfillmentCodeHook { get; set; }

    [CommandSwitch("--intent-confirmation-setting")]
    public string? IntentConfirmationSetting { get; set; }

    [CommandSwitch("--intent-closing-setting")]
    public string? IntentClosingSetting { get; set; }

    [CommandSwitch("--input-contexts")]
    public string[]? InputContexts { get; set; }

    [CommandSwitch("--output-contexts")]
    public string[]? OutputContexts { get; set; }

    [CommandSwitch("--kendra-configuration")]
    public string? KendraConfiguration { get; set; }

    [CommandSwitch("--initial-response-setting")]
    public string? InitialResponseSetting { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}