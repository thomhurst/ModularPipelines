using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "operations", "wait")]
public record GcloudDomainsRegistrationsOperationsWaitOptions(
[property: CliArgument] string Operation
) : GcloudOptions;