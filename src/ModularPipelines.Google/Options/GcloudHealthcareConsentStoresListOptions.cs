using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "consent-stores", "list")]
public record GcloudHealthcareConsentStoresListOptions(
[property: CommandSwitch("--dataset")] string Dataset,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;