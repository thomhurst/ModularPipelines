using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "describe-applications")]
public record AwsElasticbeanstalkDescribeApplicationsOptions : AwsOptions
{
    [CommandSwitch("--application-names")]
    public string[]? ApplicationNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}