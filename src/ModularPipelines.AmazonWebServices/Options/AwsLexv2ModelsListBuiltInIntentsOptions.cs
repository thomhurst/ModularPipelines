using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "list-built-in-intents")]
public record AwsLexv2ModelsListBuiltInIntentsOptions(
[property: CommandSwitch("--locale-id")] string LocaleId
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