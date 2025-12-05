using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "instruction", "update")]
public record AzBillingInstructionUpdateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--profile-name")] string ProfileName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--amount")]
    public string? Amount { get; set; }

    [CliOption("--creation-date")]
    public string? CreationDate { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }
}