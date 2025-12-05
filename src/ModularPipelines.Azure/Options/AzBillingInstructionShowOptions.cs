using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "instruction", "show")]
public record AzBillingInstructionShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--profile-name")] string ProfileName
) : AzOptions;