using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "associate-lex-bot")]
public record AwsConnectAssociateLexBotOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--lex-bot")] string LexBot
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}