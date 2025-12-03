using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "enrollment-account", "list")]
public record AzBillingEnrollmentAccountListOptions : AzOptions;