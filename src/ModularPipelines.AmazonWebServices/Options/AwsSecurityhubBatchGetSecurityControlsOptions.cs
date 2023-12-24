using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "batch-get-security-controls")]
public record AwsSecurityhubBatchGetSecurityControlsOptions(
[property: CommandSwitch("--security-control-ids")] string[] SecurityControlIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}