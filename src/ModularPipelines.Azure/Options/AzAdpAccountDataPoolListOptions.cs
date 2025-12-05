using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("adp", "account", "data-pool", "list")]
public record AzAdpAccountDataPoolListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;