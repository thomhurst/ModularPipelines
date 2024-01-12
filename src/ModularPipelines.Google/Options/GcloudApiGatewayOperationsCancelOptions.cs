using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "operations", "cancel")]
public record GcloudApiGatewayOperationsCancelOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Location
) : GcloudOptions;