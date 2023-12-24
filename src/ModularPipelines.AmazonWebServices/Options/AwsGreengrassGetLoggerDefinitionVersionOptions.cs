using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "get-logger-definition-version")]
public record AwsGreengrassGetLoggerDefinitionVersionOptions(
[property: CommandSwitch("--logger-definition-id")] string LoggerDefinitionId,
[property: CommandSwitch("--logger-definition-version-id")] string LoggerDefinitionVersionId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}