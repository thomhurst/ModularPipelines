using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "update-environment")]
public record AwsElasticbeanstalkUpdateEnvironmentOptions : AwsOptions
{
    [CommandSwitch("--application-name")]
    public string? ApplicationName { get; set; }

    [CommandSwitch("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--version-label")]
    public string? VersionLabel { get; set; }

    [CommandSwitch("--template-name")]
    public string? TemplateName { get; set; }

    [CommandSwitch("--solution-stack-name")]
    public string? SolutionStackName { get; set; }

    [CommandSwitch("--platform-arn")]
    public string? PlatformArn { get; set; }

    [CommandSwitch("--option-settings")]
    public string[]? OptionSettings { get; set; }

    [CommandSwitch("--options-to-remove")]
    public string[]? OptionsToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}