using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "products", "describe")]
public record GcloudApigeeProductsDescribeOptions(
[property: PositionalArgument] string Product,
[property: PositionalArgument] string Organization
) : GcloudOptions;