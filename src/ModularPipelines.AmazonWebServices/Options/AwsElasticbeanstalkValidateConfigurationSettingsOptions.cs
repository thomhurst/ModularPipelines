using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "validate-configuration-settings")]
public record AwsElasticbeanstalkValidateConfigurationSettingsOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--option-settings")] string[] OptionSettings
) : AwsOptions
{
    [CommandSwitch("--template-name")]
    public string? TemplateName { get; set; }

    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}