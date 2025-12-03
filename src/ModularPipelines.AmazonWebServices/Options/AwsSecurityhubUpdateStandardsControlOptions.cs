using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "update-standards-control")]
public record AwsSecurityhubUpdateStandardsControlOptions(
[property: CliOption("--standards-control-arn")] string StandardsControlArn
) : AwsOptions
{
    [CliOption("--control-status")]
    public string? ControlStatus { get; set; }

    [CliOption("--disabled-reason")]
    public string? DisabledReason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}