using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "start-resource-scan")]
public record AwsAccessanalyzerStartResourceScanOptions(
[property: CommandSwitch("--analyzer-arn")] string AnalyzerArn,
[property: CommandSwitch("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CommandSwitch("--resource-owner-account")]
    public string? ResourceOwnerAccount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}