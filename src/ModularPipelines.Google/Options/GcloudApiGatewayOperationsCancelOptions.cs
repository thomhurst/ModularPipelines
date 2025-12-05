using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "operations", "cancel")]
public record GcloudApiGatewayOperationsCancelOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;