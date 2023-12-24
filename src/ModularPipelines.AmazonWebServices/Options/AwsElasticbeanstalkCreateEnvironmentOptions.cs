using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "create-environment")]
public record AwsElasticbeanstalkCreateEnvironmentOptions(
[property: CommandSwitch("--application-name")] string ApplicationName
) : AwsOptions
{
    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--cname-prefix")]
    public string? CnamePrefix { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

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

    [CommandSwitch("--operations-role")]
    public string? OperationsRole { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}