using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "batch-get-security-controls")]
public record AwsSecurityhubBatchGetSecurityControlsOptions(
[property: CliOption("--security-control-ids")] string[] SecurityControlIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}