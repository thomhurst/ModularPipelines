using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "operations", "describe")]
public record GcloudDomainsRegistrationsOperationsDescribeOptions(
[property: CliArgument] string Operation
) : GcloudOptions;