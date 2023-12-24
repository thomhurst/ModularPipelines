using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "update-standards-control")]
public record AwsSecurityhubUpdateStandardsControlOptions(
[property: CommandSwitch("--standards-control-arn")] string StandardsControlArn
) : AwsOptions
{
    [CommandSwitch("--control-status")]
    public string? ControlStatus { get; set; }

    [CommandSwitch("--disabled-reason")]
    public string? DisabledReason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}