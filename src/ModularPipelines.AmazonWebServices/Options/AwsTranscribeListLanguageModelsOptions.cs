using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "list-language-models")]
public record AwsTranscribeListLanguageModelsOptions : AwsOptions
{
    [CliOption("--status-equals")]
    public string? StatusEquals { get; set; }

    [CliOption("--name-contains")]
    public string? NameContains { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}