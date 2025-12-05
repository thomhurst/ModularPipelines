using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "get-access-preview")]
public record AwsAccessanalyzerGetAccessPreviewOptions(
[property: CliOption("--access-preview-id")] string AccessPreviewId,
[property: CliOption("--analyzer-arn")] string AnalyzerArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}