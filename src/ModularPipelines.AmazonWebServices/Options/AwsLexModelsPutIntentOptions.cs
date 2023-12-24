using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "put-intent")]
public record AwsLexModelsPutIntentOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--slots")]
    public string[]? Slots { get; set; }

    [CommandSwitch("--sample-utterances")]
    public string[]? SampleUtterances { get; set; }

    [CommandSwitch("--confirmation-prompt")]
    public string? ConfirmationPrompt { get; set; }

    [CommandSwitch("--rejection-statement")]
    public string? RejectionStatement { get; set; }

    [CommandSwitch("--follow-up-prompt")]
    public string? FollowUpPrompt { get; set; }

    [CommandSwitch("--conclusion-statement")]
    public string? ConclusionStatement { get; set; }

    [CommandSwitch("--dialog-code-hook")]
    public string? DialogCodeHook { get; set; }

    [CommandSwitch("--fulfillment-activity")]
    public string? FulfillmentActivity { get; set; }

    [CommandSwitch("--parent-intent-signature")]
    public string? ParentIntentSignature { get; set; }

    [CommandSwitch("--checksum")]
    public string? Checksum { get; set; }

    [CommandSwitch("--kendra-configuration")]
    public string? KendraConfiguration { get; set; }

    [CommandSwitch("--input-contexts")]
    public string[]? InputContexts { get; set; }

    [CommandSwitch("--output-contexts")]
    public string[]? OutputContexts { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}