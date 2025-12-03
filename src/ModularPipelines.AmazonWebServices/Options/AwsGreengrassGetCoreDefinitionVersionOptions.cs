using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-core-definition-version")]
public record AwsGreengrassGetCoreDefinitionVersionOptions(
[property: CliOption("--core-definition-id")] string CoreDefinitionId,
[property: CliOption("--core-definition-version-id")] string CoreDefinitionVersionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}