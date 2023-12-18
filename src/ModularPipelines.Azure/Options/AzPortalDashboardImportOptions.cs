using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("portal", "dashboard", "import")]
public record AzPortalDashboardImportOptions(
[property: CommandSwitch("--input-path")] string InputPath,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}