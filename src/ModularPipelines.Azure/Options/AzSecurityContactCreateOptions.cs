using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "contact", "create")]
public record AzSecurityContactCreateOptions(
[property: CommandSwitch("--email")] string Email,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--alert-notifications")]
    public string? AlertNotifications { get; set; }

    [CommandSwitch("--alerts-admins")]
    public string? AlertsAdmins { get; set; }

    [CommandSwitch("--phone")]
    public string? Phone { get; set; }
}