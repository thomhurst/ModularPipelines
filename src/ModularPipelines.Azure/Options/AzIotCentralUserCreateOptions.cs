using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "user", "create")]
public record AzIotCentralUserCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--assignee")] string Assignee,
[property: CommandSwitch("--role")] string Role
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--email")]
    public string? Email { get; set; }

    [CommandSwitch("--object-id")]
    public string? ObjectId { get; set; }

    [CommandSwitch("--org-id")]
    public string? OrgId { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}