using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "resource-type-registration", "list")]
public record AzProviderhubResourceTypeRegistrationListOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions;