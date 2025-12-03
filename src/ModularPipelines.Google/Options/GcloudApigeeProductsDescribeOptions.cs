using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "products", "describe")]
public record GcloudApigeeProductsDescribeOptions(
[property: CliArgument] string Product,
[property: CliArgument] string Organization
) : GcloudOptions;