using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "delete-application-version")]
public record AwsElasticbeanstalkDeleteApplicationVersionOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--version-label")] string VersionLabel
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}