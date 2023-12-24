using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "get-core-definition-version")]
public record AwsGreengrassGetCoreDefinitionVersionOptions(
[property: CommandSwitch("--core-definition-id")] string CoreDefinitionId,
[property: CommandSwitch("--core-definition-version-id")] string CoreDefinitionVersionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}