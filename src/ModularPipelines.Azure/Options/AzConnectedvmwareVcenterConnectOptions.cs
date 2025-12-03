using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedvmware", "vcenter", "connect")]
public record AzConnectedvmwareVcenterConnectOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--fqdn")]
    public string? Fqdn { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}