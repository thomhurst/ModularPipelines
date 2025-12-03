using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "instruction", "list")]
public record AzBillingInstructionListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--profile-name")] string ProfileName
) : AzOptions;