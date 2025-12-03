using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "describe-configuration-recorder-status")]
public record AwsConfigserviceDescribeConfigurationRecorderStatusOptions : AwsOptions
{
    [CliOption("--configuration-recorder-names")]
    public string[]? ConfigurationRecorderNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}