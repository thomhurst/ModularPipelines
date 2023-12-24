using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "list-bot-versions")]
public record AwsLexv2ModelsListBotVersionsOptions(
[property: CommandSwitch("--bot-id")] string BotId
) : AwsOptions
{
    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}