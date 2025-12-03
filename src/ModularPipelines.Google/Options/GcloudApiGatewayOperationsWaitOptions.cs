using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "operations", "wait")]
public record GcloudApiGatewayOperationsWaitOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;