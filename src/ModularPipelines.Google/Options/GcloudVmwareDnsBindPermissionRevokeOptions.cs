using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "dns-bind-permission", "revoke")]
public record GcloudVmwareDnsBindPermissionRevokeOptions(
[property: CliOption("--service-account")] string ServiceAccount,
[property: CliOption("--user")] string User
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}