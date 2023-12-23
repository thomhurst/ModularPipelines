using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "modify-client-properties")]
public record AwsWorkspacesModifyClientPropertiesOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--client-properties")] string ClientProperties
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}