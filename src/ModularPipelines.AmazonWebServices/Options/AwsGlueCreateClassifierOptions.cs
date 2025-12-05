using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-classifier")]
public record AwsGlueCreateClassifierOptions : AwsOptions
{
    [CliOption("--grok-classifier")]
    public string? GrokClassifier { get; set; }

    [CliOption("--xml-classifier")]
    public string? XmlClassifier { get; set; }

    [CliOption("--json-classifier")]
    public string? JsonClassifier { get; set; }

    [CliOption("--csv-classifier")]
    public string? CsvClassifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}