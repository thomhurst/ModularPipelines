using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "show-custom-domain-verification-id")]
public record AzContainerappShowCustomDomainVerificationIdOptions : AzOptions;