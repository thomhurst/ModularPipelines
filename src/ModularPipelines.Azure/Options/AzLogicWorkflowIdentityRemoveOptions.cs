using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("logic", "workflow", "identity", "remove")]
public record AzLogicWorkflowIdentityRemoveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--system-assigned")]
    public bool? SystemAssigned { get; set; }

    [CliOption("--user-assigned")]
    public string? UserAssigned { get; set; }
}