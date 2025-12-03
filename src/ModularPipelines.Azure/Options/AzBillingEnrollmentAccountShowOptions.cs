using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "enrollment-account", "show")]
public record AzBillingEnrollmentAccountShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;