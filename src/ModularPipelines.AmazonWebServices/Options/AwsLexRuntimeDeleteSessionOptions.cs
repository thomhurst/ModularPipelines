using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-runtime", "delete-session")]
public record AwsLexRuntimeDeleteSessionOptions(
[property: CommandSwitch("--bot-name")] string BotName,
[property: CommandSwitch("--bot-alias")] string BotAlias,
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}