using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "dns-bind-permission", "grant")]
public record GcloudVmwareDnsBindPermissionGrantOptions(
[property: CommandSwitch("--service-account")] string ServiceAccount,
[property: CommandSwitch("--user")] string User
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}