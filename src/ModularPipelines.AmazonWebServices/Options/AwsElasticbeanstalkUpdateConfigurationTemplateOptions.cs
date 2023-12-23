using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "update-configuration-template")]
public record AwsElasticbeanstalkUpdateConfigurationTemplateOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--template-name")] string TemplateName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--option-settings")]
    public string[]? OptionSettings { get; set; }

    [CommandSwitch("--options-to-remove")]
    public string[]? OptionsToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}