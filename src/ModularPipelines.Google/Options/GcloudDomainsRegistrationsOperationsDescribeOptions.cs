using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "operations", "describe")]
public record GcloudDomainsRegistrationsOperationsDescribeOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;