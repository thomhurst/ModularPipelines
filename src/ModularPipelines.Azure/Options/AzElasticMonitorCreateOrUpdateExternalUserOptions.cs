using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("elastic", "monitor", "create-or-update-external-user")]
public record AzElasticMonitorCreateOrUpdateExternalUserOptions : AzOptions
{
    [CliOption("--email-id")]
    public string? EmailId { get; set; }

    [CliOption("--full-name")]
    public string? FullName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--monitor-name")]
    public string? MonitorName { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--roles")]
    public string? Roles { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--user-name")]
    public string? UserName { get; set; }
}