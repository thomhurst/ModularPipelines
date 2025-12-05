using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "dns-bind-permission", "describe")]
public record GcloudVmwareDnsBindPermissionDescribeOptions : GcloudOptions;