using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-runtime", "post-text")]
public record AwsLexRuntimePostTextOptions(
[property: CommandSwitch("--bot-name")] string BotName,
[property: CommandSwitch("--bot-alias")] string BotAlias,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--input-text")] string InputText
) : AwsOptions
{
    [CommandSwitch("--session-attributes")]
    public IEnumerable<KeyValue>? SessionAttributes { get; set; }

    [CommandSwitch("--request-attributes")]
    public IEnumerable<KeyValue>? RequestAttributes { get; set; }

    [CommandSwitch("--active-contexts")]
    public string[]? ActiveContexts { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}