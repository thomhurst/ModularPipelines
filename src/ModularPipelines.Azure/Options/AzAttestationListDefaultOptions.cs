using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("attestation", "list-default")]
public record AzAttestationListDefaultOptions : AzOptions;