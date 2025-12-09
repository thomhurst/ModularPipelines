using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub", "resource-type-registration", "list")]
public record AzProviderhubResourceTypeRegistrationListOptions(
[property: CliOption("--provider-namespace")] string ProviderNamespace
) : AzOptions;