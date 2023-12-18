using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "connection", "list")]
public record AzMlConnectionListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--max-results")]
    public string? MaxResults { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}