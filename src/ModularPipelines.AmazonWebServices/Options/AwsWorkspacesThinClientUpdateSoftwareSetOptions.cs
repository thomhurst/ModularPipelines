using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-thin-client", "update-software-set")]
public record AwsWorkspacesThinClientUpdateSoftwareSetOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--validation-status")] string ValidationStatus
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}