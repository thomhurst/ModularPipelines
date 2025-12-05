using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "put-logging-configuration")]
public record AwsWafv2PutLoggingConfigurationOptions(
[property: CliOption("--logging-configuration")] string LoggingConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}