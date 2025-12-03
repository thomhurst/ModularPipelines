using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "validate-configuration-settings")]
public record AwsElasticbeanstalkValidateConfigurationSettingsOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--option-settings")] string[] OptionSettings
) : AwsOptions
{
    [CliOption("--template-name")]
    public string? TemplateName { get; set; }

    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}