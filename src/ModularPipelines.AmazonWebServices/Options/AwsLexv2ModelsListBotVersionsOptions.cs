using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "list-bot-versions")]
public record AwsLexv2ModelsListBotVersionsOptions(
[property: CliOption("--bot-id")] string BotId
) : AwsOptions
{
    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}