using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "organization", "create")]
public record AzIotCentralOrganizationCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--org-id")] string OrgId
) : AzOptions
{
    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--org-name")]
    public string? OrgName { get; set; }

    [CommandSwitch("--parent-id")]
    public string? ParentId { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}