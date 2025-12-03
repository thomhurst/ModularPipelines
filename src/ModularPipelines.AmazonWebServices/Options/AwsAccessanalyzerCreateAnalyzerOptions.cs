using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "create-analyzer")]
public record AwsAccessanalyzerCreateAnalyzerOptions(
[property: CliOption("--analyzer-name")] string AnalyzerName,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--archive-rules")]
    public string[]? ArchiveRules { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}