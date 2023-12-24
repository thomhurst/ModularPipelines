using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "create-analyzer")]
public record AwsAccessanalyzerCreateAnalyzerOptions(
[property: CommandSwitch("--analyzer-name")] string AnalyzerName,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--archive-rules")]
    public string[]? ArchiveRules { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}