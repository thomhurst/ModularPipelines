using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-logger-definition-version")]
public record AwsGreengrassGetLoggerDefinitionVersionOptions(
[property: CliOption("--logger-definition-id")] string LoggerDefinitionId,
[property: CliOption("--logger-definition-version-id")] string LoggerDefinitionVersionId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}