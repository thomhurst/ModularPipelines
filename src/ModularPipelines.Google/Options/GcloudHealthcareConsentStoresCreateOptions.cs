using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "consent-stores", "create")]
public record GcloudHealthcareConsentStoresCreateOptions(
[property: PositionalArgument] string ConsentStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}