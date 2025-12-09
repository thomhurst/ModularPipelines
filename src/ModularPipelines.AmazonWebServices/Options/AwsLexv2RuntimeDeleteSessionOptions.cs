using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-runtime", "delete-session")]
public record AwsLexv2RuntimeDeleteSessionOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-alias-id")] string BotAliasId,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--session-id")] string SessionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}