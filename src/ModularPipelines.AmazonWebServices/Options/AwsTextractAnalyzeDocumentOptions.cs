using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("textract", "analyze-document")]
public record AwsTextractAnalyzeDocumentOptions(
[property: CliOption("--document")] string Document,
[property: CliOption("--feature-types")] string[] FeatureTypes
) : AwsOptions
{
    [CliOption("--human-loop-config")]
    public string? HumanLoopConfig { get; set; }

    [CliOption("--queries-config")]
    public string? QueriesConfig { get; set; }

    [CliOption("--adapters-config")]
    public string? AdaptersConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}