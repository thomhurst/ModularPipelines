using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-runtime", "post-text")]
public record AwsLexRuntimePostTextOptions(
[property: CliOption("--bot-name")] string BotName,
[property: CliOption("--bot-alias")] string BotAlias,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--input-text")] string InputText
) : AwsOptions
{
    [CliOption("--session-attributes")]
    public IEnumerable<KeyValue>? SessionAttributes { get; set; }

    [CliOption("--request-attributes")]
    public IEnumerable<KeyValue>? RequestAttributes { get; set; }

    [CliOption("--active-contexts")]
    public string[]? ActiveContexts { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}