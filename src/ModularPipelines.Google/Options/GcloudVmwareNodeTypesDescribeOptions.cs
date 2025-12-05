using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "node-types", "describe")]
public record GcloudVmwareNodeTypesDescribeOptions(
[property: CliArgument] string NodeType,
[property: CliArgument] string Location
) : GcloudOptions;