using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "product", "show")]
public record AzBillingProductShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name
) : AzOptions;