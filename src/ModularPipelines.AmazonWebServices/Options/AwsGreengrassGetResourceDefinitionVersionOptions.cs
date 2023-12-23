using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "get-resource-definition-version")]
public record AwsGreengrassGetResourceDefinitionVersionOptions(
[property: CommandSwitch("--resource-definition-id")] string ResourceDefinitionId,
[property: CommandSwitch("--resource-definition-version-id")] string ResourceDefinitionVersionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}