using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "search-vocabularies")]
public record AwsConnectSearchVocabulariesOptions(
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--name-starts-with")]
    public string? NameStartsWith { get; set; }

    [CliOption("--language-code")]
    public string? LanguageCode { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}