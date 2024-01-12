using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "get-transfer-parameters")]
public record GcloudDomainsRegistrationsGetTransferParametersOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions;