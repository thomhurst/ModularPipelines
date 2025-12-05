using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "put-bot")]
public record AwsLexModelsPutBotOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--locale")] string Locale
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--intents")]
    public string[]? Intents { get; set; }

    [CliOption("--nlu-intent-confidence-threshold")]
    public double? NluIntentConfidenceThreshold { get; set; }

    [CliOption("--clarification-prompt")]
    public string? ClarificationPrompt { get; set; }

    [CliOption("--abort-statement")]
    public string? AbortStatement { get; set; }

    [CliOption("--idle-session-ttl-in-seconds")]
    public int? IdleSessionTtlInSeconds { get; set; }

    [CliOption("--voice-id")]
    public string? VoiceId { get; set; }

    [CliOption("--checksum")]
    public string? Checksum { get; set; }

    [CliOption("--process-behavior")]
    public string? ProcessBehavior { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}