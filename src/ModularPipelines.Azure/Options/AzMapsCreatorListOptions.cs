using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maps", "creator", "list")]
public record AzMapsCreatorListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;