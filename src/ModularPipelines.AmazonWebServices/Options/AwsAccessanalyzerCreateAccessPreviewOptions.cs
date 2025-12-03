using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "create-access-preview")]
public record AwsAccessanalyzerCreateAccessPreviewOptions(
[property: CliOption("--analyzer-arn")] string AnalyzerArn,
[property: CliOption("--configurations")] IEnumerable<KeyValue> Configurations
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}