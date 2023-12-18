using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "instruction", "list")]
public record AzBillingInstructionListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--profile-name")] string ProfileName
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--amount")]
    public string? Amount { get; set; }

    [CommandSwitch("--creation-date")]
    public string? CreationDate { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }
}