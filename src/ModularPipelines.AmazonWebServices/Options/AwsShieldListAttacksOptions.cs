using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("shield", "list-attacks")]
public record AwsShieldListAttacksOptions : AwsOptions
{
    [CommandSwitch("--resource-arns")]
    public string[]? ResourceArns { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}