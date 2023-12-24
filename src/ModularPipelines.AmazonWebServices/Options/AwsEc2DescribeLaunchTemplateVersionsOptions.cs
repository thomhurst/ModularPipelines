using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-launch-template-versions")]
public record AwsEc2DescribeLaunchTemplateVersionsOptions : AwsOptions
{
    [CommandSwitch("--launch-template-id")]
    public string? LaunchTemplateId { get; set; }

    [CommandSwitch("--launch-template-name")]
    public string? LaunchTemplateName { get; set; }

    [CommandSwitch("--versions")]
    public string[]? Versions { get; set; }

    [CommandSwitch("--min-version")]
    public string? MinVersion { get; set; }

    [CommandSwitch("--max-version")]
    public string? MaxVersion { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}