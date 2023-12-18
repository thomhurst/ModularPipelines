using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "instruction", "create")]
public record AzBillingInstructionCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--profile-name")] string ProfileName
) : AzOptions
{
    [CommandSwitch("--amount")]
    public string? Amount { get; set; }

    [CommandSwitch("--creation-date")]
    public string? CreationDate { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }
}