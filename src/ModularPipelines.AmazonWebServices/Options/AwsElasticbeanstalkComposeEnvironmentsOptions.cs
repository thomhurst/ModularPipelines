using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "compose-environments")]
public record AwsElasticbeanstalkComposeEnvironmentsOptions : AwsOptions
{
    [CommandSwitch("--application-name")]
    public string? ApplicationName { get; set; }

    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--version-labels")]
    public string[]? VersionLabels { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}