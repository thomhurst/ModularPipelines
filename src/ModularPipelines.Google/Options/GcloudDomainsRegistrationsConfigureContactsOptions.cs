using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "configure", "contacts")]
public record GcloudDomainsRegistrationsConfigureContactsOptions(
[property: PositionalArgument] string Registration
) : GcloudOptions;