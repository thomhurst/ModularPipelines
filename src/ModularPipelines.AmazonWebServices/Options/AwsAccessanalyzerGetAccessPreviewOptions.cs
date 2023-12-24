using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "get-access-preview")]
public record AwsAccessanalyzerGetAccessPreviewOptions(
[property: CommandSwitch("--access-preview-id")] string AccessPreviewId,
[property: CommandSwitch("--analyzer-arn")] string AnalyzerArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}