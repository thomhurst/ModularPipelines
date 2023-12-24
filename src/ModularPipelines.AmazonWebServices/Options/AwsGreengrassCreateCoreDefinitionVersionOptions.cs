using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "create-core-definition-version")]
public record AwsGreengrassCreateCoreDefinitionVersionOptions(
[property: CommandSwitch("--core-definition-id")] string CoreDefinitionId
) : AwsOptions
{
    [CommandSwitch("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CommandSwitch("--cores")]
    public string[]? Cores { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}