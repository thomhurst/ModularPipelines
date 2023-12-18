using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "createconfig", "scvmm")]
public record AzArcapplianceCreateconfigScvmmOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--certificateFilePath")]
    public string? CertificateFilePath { get; set; }

    [CommandSwitch("--cloudname")]
    public string? Cloudname { get; set; }

    [CommandSwitch("--controlPlaneEndpoint")]
    public string? ControlPlaneEndpoint { get; set; }

    [CommandSwitch("--dnsservers")]
    public string? Dnsservers { get; set; }

    [CommandSwitch("--gateway")]
    public string? Gateway { get; set; }

    [CommandSwitch("--highlyavailable")]
    public string? Highlyavailable { get; set; }

    [CommandSwitch("--hostgroupid")]
    public string? Hostgroupid { get; set; }

    [CommandSwitch("--http")]
    public string? Http { get; set; }

    [CommandSwitch("--https")]
    public string? Https { get; set; }

    [CommandSwitch("--ipaddressprefix")]
    public string? Ipaddressprefix { get; set; }

    [CommandSwitch("--ippool")]
    public string? Ippool { get; set; }

    [CommandSwitch("--k8snodeippoolend")]
    public string? K8snodeippoolend { get; set; }

    [CommandSwitch("--k8snodeippoolstart")]
    public string? K8snodeippoolstart { get; set; }

    [CommandSwitch("--libshare")]
    public string? Libshare { get; set; }

    [CommandSwitch("--macaddress")]
    public string? Macaddress { get; set; }

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

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--working-dir")]
    public string? WorkingDir { get; set; }
}