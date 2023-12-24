using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "describe-configuration-settings")]
public record AwsElasticbeanstalkDescribeConfigurationSettingsOptions(
[property: CommandSwitch("--application-name")] string ApplicationName
) : AwsOptions
{
    [CommandSwitch("--template-name")]
    public string? TemplateName { get; set; }

    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}