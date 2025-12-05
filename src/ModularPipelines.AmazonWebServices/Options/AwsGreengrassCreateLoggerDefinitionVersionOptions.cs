using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "create-logger-definition-version")]
public record AwsGreengrassCreateLoggerDefinitionVersionOptions(
[property: CliOption("--logger-definition-id")] string LoggerDefinitionId
) : AwsOptions
{
    [CliOption("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CliOption("--loggers")]
    public string[]? Loggers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}