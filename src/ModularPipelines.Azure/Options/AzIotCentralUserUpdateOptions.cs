using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "user", "update")]
public record AzIotCentralUserUpdateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--assignee")] string Assignee
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--email")]
    public string? Email { get; set; }

    [CommandSwitch("--object-id")]
    public string? ObjectId { get; set; }

    [CommandSwitch("--roles")]
    public string? Roles { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}