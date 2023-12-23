using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "create-platform-version")]
public record AwsElasticbeanstalkCreatePlatformVersionOptions(
[property: CommandSwitch("--platform-name")] string PlatformName,
[property: CommandSwitch("--platform-version")] string PlatformVersion,
[property: CommandSwitch("--platform-definition-bundle")] string PlatformDefinitionBundle
) : AwsOptions
{
    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--option-settings")]
    public string[]? OptionSettings { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}