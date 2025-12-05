using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "put-intent")]
public record AwsLexModelsPutIntentOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--slots")]
    public string[]? Slots { get; set; }

    [CliOption("--sample-utterances")]
    public string[]? SampleUtterances { get; set; }

    [CliOption("--confirmation-prompt")]
    public string? ConfirmationPrompt { get; set; }

    [CliOption("--rejection-statement")]
    public string? RejectionStatement { get; set; }

    [CliOption("--follow-up-prompt")]
    public string? FollowUpPrompt { get; set; }

    [CliOption("--conclusion-statement")]
    public string? ConclusionStatement { get; set; }

    [CliOption("--dialog-code-hook")]
    public string? DialogCodeHook { get; set; }

    [CliOption("--fulfillment-activity")]
    public string? FulfillmentActivity { get; set; }

    [CliOption("--parent-intent-signature")]
    public string? ParentIntentSignature { get; set; }

    [CliOption("--checksum")]
    public string? Checksum { get; set; }

    [CliOption("--kendra-configuration")]
    public string? KendraConfiguration { get; set; }

    [CliOption("--input-contexts")]
    public string[]? InputContexts { get; set; }

    [CliOption("--output-contexts")]
    public string[]? OutputContexts { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}