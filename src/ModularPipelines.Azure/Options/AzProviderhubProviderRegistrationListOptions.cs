using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "provider-registration", "list")]
public record AzProviderhubProviderRegistrationListOptions : AzOptions;