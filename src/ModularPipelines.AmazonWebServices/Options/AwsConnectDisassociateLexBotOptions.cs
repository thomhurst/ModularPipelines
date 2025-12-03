using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "disassociate-lex-bot")]
public record AwsConnectDisassociateLexBotOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--bot-name")] string BotName,
[property: CliOption("--lex-region")] string LexRegion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}