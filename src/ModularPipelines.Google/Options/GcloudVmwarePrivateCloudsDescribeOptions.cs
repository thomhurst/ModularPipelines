using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "describe")]
public record GcloudVmwarePrivateCloudsDescribeOptions(
[property: PositionalArgument] string PrivateCloud,
[property: PositionalArgument] string Location
) : GcloudOptions;