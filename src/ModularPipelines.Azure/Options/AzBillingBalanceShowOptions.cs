using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "balance", "show")]
public record AzBillingBalanceShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--profile-name")] string ProfileName
) : AzOptions;