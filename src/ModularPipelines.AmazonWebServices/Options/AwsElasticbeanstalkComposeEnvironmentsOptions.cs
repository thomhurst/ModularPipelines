using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "compose-environments")]
public record AwsElasticbeanstalkComposeEnvironmentsOptions : AwsOptions
{
    [CliOption("--application-name")]
    public string? ApplicationName { get; set; }

    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--version-labels")]
    public string[]? VersionLabels { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}