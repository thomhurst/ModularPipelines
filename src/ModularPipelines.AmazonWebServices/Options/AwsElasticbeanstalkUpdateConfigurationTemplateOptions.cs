using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "update-configuration-template")]
public record AwsElasticbeanstalkUpdateConfigurationTemplateOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--option-settings")]
    public string[]? OptionSettings { get; set; }

    [CliOption("--options-to-remove")]
    public string[]? OptionsToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}