using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "get-transfer-parameters")]
public record GcloudDomainsRegistrationsGetTransferParametersOptions(
[property: CliArgument] string Domain
) : GcloudOptions;