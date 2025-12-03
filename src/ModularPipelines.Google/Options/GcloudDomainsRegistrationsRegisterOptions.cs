using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "register")]
public record GcloudDomainsRegistrationsRegisterOptions(
[property: CliArgument] string Registration
) : GcloudOptions;