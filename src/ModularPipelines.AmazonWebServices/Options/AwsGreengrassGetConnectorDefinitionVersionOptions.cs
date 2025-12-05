using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-connector-definition-version")]
public record AwsGreengrassGetConnectorDefinitionVersionOptions(
[property: CliOption("--connector-definition-id")] string ConnectorDefinitionId,
[property: CliOption("--connector-definition-version-id")] string ConnectorDefinitionVersionId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}