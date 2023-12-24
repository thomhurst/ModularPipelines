using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "create-configuration-template")]
public record AwsElasticbeanstalkCreateConfigurationTemplateOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--template-name")] string TemplateName
) : AwsOptions
{
    [CommandSwitch("--solution-stack-name")]
    public string? SolutionStackName { get; set; }

    [CommandSwitch("--platform-arn")]
    public string? PlatformArn { get; set; }

    [CommandSwitch("--source-configuration")]
    public string? SourceConfiguration { get; set; }

    [CommandSwitch("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--option-settings")]
    public string[]? OptionSettings { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}