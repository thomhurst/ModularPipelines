using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "operations", "wait")]
public record GcloudDomainsRegistrationsOperationsWaitOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;