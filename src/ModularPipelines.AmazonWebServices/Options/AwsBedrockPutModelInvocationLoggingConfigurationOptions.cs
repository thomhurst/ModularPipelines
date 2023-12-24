using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock", "put-model-invocation-logging-configuration")]
public record AwsBedrockPutModelInvocationLoggingConfigurationOptions(
[property: CommandSwitch("--logging-config")] string LoggingConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}