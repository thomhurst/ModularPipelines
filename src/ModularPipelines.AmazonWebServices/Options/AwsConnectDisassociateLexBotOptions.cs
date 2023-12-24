using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "disassociate-lex-bot")]
public record AwsConnectDisassociateLexBotOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--bot-name")] string BotName,
[property: CommandSwitch("--lex-region")] string LexRegion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}