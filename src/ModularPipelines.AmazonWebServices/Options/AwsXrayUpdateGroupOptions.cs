using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("xray", "update-group")]
public record AwsXrayUpdateGroupOptions : AwsOptions
{
    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--group-arn")]
    public string? GroupArn { get; set; }

    [CommandSwitch("--filter-expression")]
    public string? FilterExpression { get; set; }

    [CommandSwitch("--insights-configuration")]
    public string? InsightsConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}