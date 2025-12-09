using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "get-export")]
public record AwsLexModelsGetExportOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--export-type")] string ExportType,
[property: CliOption("--resource-version")] string ResourceVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}