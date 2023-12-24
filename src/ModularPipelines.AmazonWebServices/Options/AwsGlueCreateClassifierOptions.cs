using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-classifier")]
public record AwsGlueCreateClassifierOptions : AwsOptions
{
    [CommandSwitch("--grok-classifier")]
    public string? GrokClassifier { get; set; }

    [CommandSwitch("--xml-classifier")]
    public string? XmlClassifier { get; set; }

    [CommandSwitch("--json-classifier")]
    public string? JsonClassifier { get; set; }

    [CommandSwitch("--csv-classifier")]
    public string? CsvClassifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}