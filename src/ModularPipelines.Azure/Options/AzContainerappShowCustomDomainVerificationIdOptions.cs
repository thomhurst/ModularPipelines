using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "show-custom-domain-verification-id")]
public record AzContainerappShowCustomDomainVerificationIdOptions : AzOptions;