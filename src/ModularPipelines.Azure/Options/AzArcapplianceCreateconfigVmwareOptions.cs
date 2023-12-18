using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "createconfig", "vmware")]
public record AzArcapplianceCreateconfigVmwareOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--certificateFilePath")]
    public string? CertificateFilePath { get; set; }

    [CommandSwitch("--controlPlaneEndpoint")]
    public string? ControlPlaneEndpoint { get; set; }

    [CommandSwitch("--datacenter")]
    public string? Datacenter { get; set; }

    [CommandSwitch("--datastore")]
    public string? Datastore { get; set; }

    [CommandSwitch("--disksizegib")]
    public string? Disksizegib { get; set; }

    [CommandSwitch("--dnsservers")]
    public string? Dnsservers { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--gateway")]
    public string? Gateway { get; set; }

    [CommandSwitch("--http")]
    public string? Http { get; set; }

    [CommandSwitch("--https")]
    public string? Https { get; set; }

    [CommandSwitch("--ipaddressprefix")]
    public string? Ipaddressprefix { get; set; }

    [CommandSwitch("--k8snodeippoolend")]
    public string? K8snodeippoolend { get; set; }

    [CommandSwitch("--k8snodeippoolstart")]
    public string? K8snodeippoolstart { get; set; }

    [CommandSwitch("--memorymib")]
    public string? Memorymib { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--noproxy")]
    public string? Noproxy { get; set; }

    [CommandSwitch("--numcpus")]
    public string? Numcpus { get; set; }

    [CommandSwitch("--out-dir")]
    public string? OutDir { get; set; }

    [BooleanCommandSwitch("--overwrite-existing")]
    public bool? OverwriteExisting { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--resourcepool")]
    public string? Resourcepool { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--working-dir")]
    public string? WorkingDir { get; set; }
}

