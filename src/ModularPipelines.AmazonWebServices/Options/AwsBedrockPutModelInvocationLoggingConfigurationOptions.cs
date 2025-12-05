using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock", "put-model-invocation-logging-configuration")]
public record AwsBedrockPutModelInvocationLoggingConfigurationOptions(
[property: CliOption("--logging-config")] string LoggingConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}