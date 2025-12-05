using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "associate-lex-bot")]
public record AwsConnectAssociateLexBotOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--lex-bot")] string LexBot
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}