using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "update-connect-client-add-in")]
public record AwsWorkspacesUpdateConnectClientAddInOptions(
[property: CommandSwitch("--add-in-id")] string AddInId,
[property: CommandSwitch("--resource-id")] string ResourceId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--url")]
    public string? Url { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}