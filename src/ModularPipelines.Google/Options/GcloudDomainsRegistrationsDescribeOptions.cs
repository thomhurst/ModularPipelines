using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "describe")]
public record GcloudDomainsRegistrationsDescribeOptions(
[property: CliArgument] string Registration
) : GcloudOptions;