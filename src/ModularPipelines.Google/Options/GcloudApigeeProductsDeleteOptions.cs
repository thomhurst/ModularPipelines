using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "products", "delete")]
public record GcloudApigeeProductsDeleteOptions(
[property: PositionalArgument] string Product,
[property: PositionalArgument] string Organization
) : GcloudOptions;