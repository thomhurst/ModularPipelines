using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "contact", "create")]
public record AzSecurityContactCreateOptions(
[property: CliOption("--email")] string Email,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--alert-notifications")]
    public string? AlertNotifications { get; set; }

    [CliOption("--alerts-admins")]
    public string? AlertsAdmins { get; set; }

    [CliOption("--phone")]
    public string? Phone { get; set; }
}