using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "describe")]
public record GcloudDomainsRegistrationsDescribeOptions(
[property: PositionalArgument] string Registration
) : GcloudOptions;