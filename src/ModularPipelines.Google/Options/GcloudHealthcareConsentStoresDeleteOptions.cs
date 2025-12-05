using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "consent-stores", "delete")]
public record GcloudHealthcareConsentStoresDeleteOptions(
[property: CliArgument] string ConsentStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions;