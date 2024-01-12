using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "node-types", "describe")]
public record GcloudVmwareNodeTypesDescribeOptions(
[property: PositionalArgument] string NodeType,
[property: PositionalArgument] string Location
) : GcloudOptions;