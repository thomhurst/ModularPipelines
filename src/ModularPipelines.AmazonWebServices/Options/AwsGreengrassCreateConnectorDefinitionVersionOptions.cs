using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "create-connector-definition-version")]
public record AwsGreengrassCreateConnectorDefinitionVersionOptions(
[property: CommandSwitch("--connector-definition-id")] string ConnectorDefinitionId
) : AwsOptions
{
    [CommandSwitch("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CommandSwitch("--connectors")]
    public string[]? Connectors { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}