using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "consent-stores", "list")]
public record GcloudHealthcareConsentStoresListOptions(
[property: CliOption("--dataset")] string Dataset,
[property: CliOption("--location")] string Location
) : GcloudOptions;