using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "describe-communications")]
public record AwsSupportDescribeCommunicationsOptions(
[property: CommandSwitch("--case-id")] string CaseId
) : AwsOptions
{
    [CommandSwitch("--before-time")]
    public string? BeforeTime { get; set; }

    [CommandSwitch("--after-time")]
    public string? AfterTime { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}