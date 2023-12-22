using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "instruction", "list")]
public record AzBillingInstructionListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--profile-name")] string ProfileName
) : AzOptions;