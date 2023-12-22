using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("feature", "registration", "show")]
public record AzFeatureRegistrationShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions;