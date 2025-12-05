using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "products", "delete")]
public record GcloudApigeeProductsDeleteOptions(
[property: CliArgument] string Product,
[property: CliArgument] string Organization
) : GcloudOptions;