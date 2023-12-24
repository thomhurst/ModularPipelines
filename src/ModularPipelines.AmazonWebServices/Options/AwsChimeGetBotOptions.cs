using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "get-bot")]
public record AwsChimeGetBotOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--bot-id")] string BotId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}