using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "describe-environment-health")]
public record AwsElasticbeanstalkDescribeEnvironmentHealthOptions : AwsOptions
{
    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CommandSwitch("--attribute-names")]
    public string[]? AttributeNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}