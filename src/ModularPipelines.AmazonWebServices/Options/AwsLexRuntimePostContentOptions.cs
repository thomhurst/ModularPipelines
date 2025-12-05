using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-runtime", "post-content")]
public record AwsLexRuntimePostContentOptions(
[property: CliOption("--bot-name")] string BotName,
[property: CliOption("--bot-alias")] string BotAlias,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--content-type")] string ContentType,
[property: CliOption("--input-stream")] string InputStream
) : AwsOptions
{
    [CliOption("--session-attributes")]
    public string? SessionAttributes { get; set; }

    [CliOption("--request-attributes")]
    public string? RequestAttributes { get; set; }

    [CliOption("--accept")]
    public string? Accept { get; set; }

    [CliOption("--active-contexts")]
    public string? ActiveContexts { get; set; }
}