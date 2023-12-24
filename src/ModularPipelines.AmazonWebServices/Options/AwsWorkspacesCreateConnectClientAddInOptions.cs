using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "create-connect-client-add-in")]
public record AwsWorkspacesCreateConnectClientAddInOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--url")] string Url
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}