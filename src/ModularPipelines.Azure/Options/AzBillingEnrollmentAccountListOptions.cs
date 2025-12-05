using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "enrollment-account", "list")]
public record AzBillingEnrollmentAccountListOptions : AzOptions;