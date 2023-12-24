using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "put-bot")]
public record AwsLexModelsPutBotOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--locale")] string Locale
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--intents")]
    public string[]? Intents { get; set; }

    [CommandSwitch("--nlu-intent-confidence-threshold")]
    public double? NluIntentConfidenceThreshold { get; set; }

    [CommandSwitch("--clarification-prompt")]
    public string? ClarificationPrompt { get; set; }

    [CommandSwitch("--abort-statement")]
    public string? AbortStatement { get; set; }

    [CommandSwitch("--idle-session-ttl-in-seconds")]
    public int? IdleSessionTtlInSeconds { get; set; }

    [CommandSwitch("--voice-id")]
    public string? VoiceId { get; set; }

    [CommandSwitch("--checksum")]
    public string? Checksum { get; set; }

    [CommandSwitch("--process-behavior")]
    public string? ProcessBehavior { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}