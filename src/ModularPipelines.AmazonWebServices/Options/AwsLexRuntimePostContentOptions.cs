using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-runtime", "post-content")]
public record AwsLexRuntimePostContentOptions(
[property: CommandSwitch("--bot-name")] string BotName,
[property: CommandSwitch("--bot-alias")] string BotAlias,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--content-type")] string ContentType,
[property: CommandSwitch("--input-stream")] string InputStream
) : AwsOptions
{
    [CommandSwitch("--session-attributes")]
    public string? SessionAttributes { get; set; }

    [CommandSwitch("--request-attributes")]
    public string? RequestAttributes { get; set; }

    [CommandSwitch("--accept")]
    public string? Accept { get; set; }

    [CommandSwitch("--active-contexts")]
    public string? ActiveContexts { get; set; }
}