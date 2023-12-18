using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "organization", "update")]
public record AzIotCentralOrganizationUpdateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--org-id")] string OrgId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--org-name")]
    public string? OrgName { get; set; }

    [CommandSwitch("--parent-id")]
    public string? ParentId { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}