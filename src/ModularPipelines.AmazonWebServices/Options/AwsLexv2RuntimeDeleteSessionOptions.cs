using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-runtime", "delete-session")]
public record AwsLexv2RuntimeDeleteSessionOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-alias-id")] string BotAliasId,
[property: CommandSwitch("--locale-id")] string LocaleId,
[property: CommandSwitch("--session-id")] string SessionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}