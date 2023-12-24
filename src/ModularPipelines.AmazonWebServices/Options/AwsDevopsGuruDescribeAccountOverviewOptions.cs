using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops-guru", "describe-account-overview")]
public record AwsDevopsGuruDescribeAccountOverviewOptions(
[property: CommandSwitch("--from-time")] long FromTime
) : AwsOptions
{
    [CommandSwitch("--to-time")]
    public long? ToTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}