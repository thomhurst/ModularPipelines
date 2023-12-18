using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "setting", "create")]
public record AzSentinelSettingCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--entity-analytics")]
    public string? EntityAnalytics { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--ueba")]
    public string? Ueba { get; set; }
}