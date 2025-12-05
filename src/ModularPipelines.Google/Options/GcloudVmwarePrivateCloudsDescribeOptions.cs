using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "describe")]
public record GcloudVmwarePrivateCloudsDescribeOptions(
[property: CliArgument] string PrivateCloud,
[property: CliArgument] string Location
) : GcloudOptions;