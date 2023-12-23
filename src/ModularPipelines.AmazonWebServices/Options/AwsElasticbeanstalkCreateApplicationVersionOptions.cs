using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "create-application-version")]
public record AwsElasticbeanstalkCreateApplicationVersionOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--version-label")] string VersionLabel
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--source-build-information")]
    public string? SourceBuildInformation { get; set; }

    [CommandSwitch("--source-bundle")]
    public string? SourceBundle { get; set; }

    [CommandSwitch("--build-configuration")]
    public string? BuildConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}