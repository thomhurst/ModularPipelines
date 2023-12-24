using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "create-access-preview")]
public record AwsAccessanalyzerCreateAccessPreviewOptions(
[property: CommandSwitch("--analyzer-arn")] string AnalyzerArn,
[property: CommandSwitch("--configurations")] IEnumerable<KeyValue> Configurations
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}