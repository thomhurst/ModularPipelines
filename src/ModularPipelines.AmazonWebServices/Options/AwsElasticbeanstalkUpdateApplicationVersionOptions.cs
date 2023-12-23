using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "update-application-version")]
public record AwsElasticbeanstalkUpdateApplicationVersionOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--version-label")] string VersionLabel
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}