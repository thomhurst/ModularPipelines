using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "create-environment")]
public record AwsElasticbeanstalkCreateEnvironmentOptions(
[property: CliOption("--application-name")] string ApplicationName
) : AwsOptions
{
    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--cname-prefix")]
    public string? CnamePrefix { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--version-label")]
    public string? VersionLabel { get; set; }

    [CliOption("--template-name")]
    public string? TemplateName { get; set; }

    [CliOption("--solution-stack-name")]
    public string? SolutionStackName { get; set; }

    [CliOption("--platform-arn")]
    public string? PlatformArn { get; set; }

    [CliOption("--option-settings")]
    public string[]? OptionSettings { get; set; }

    [CliOption("--options-to-remove")]
    public string[]? OptionsToRemove { get; set; }

    [CliOption("--operations-role")]
    public string? OperationsRole { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}