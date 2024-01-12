using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "operations", "describe")]
public record GcloudApiGatewayOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Location
) : GcloudOptions;