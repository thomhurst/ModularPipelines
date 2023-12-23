using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "get-connector-definition-version")]
public record AwsGreengrassGetConnectorDefinitionVersionOptions(
[property: CommandSwitch("--connector-definition-id")] string ConnectorDefinitionId,
[property: CommandSwitch("--connector-definition-version-id")] string ConnectorDefinitionVersionId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}