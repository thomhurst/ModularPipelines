using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-resource-definition-version")]
public record AwsGreengrassGetResourceDefinitionVersionOptions(
[property: CliOption("--resource-definition-id")] string ResourceDefinitionId,
[property: CliOption("--resource-definition-version-id")] string ResourceDefinitionVersionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}