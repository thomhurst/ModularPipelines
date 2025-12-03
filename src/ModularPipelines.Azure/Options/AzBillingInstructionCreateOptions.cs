using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "instruction", "create")]
public record AzBillingInstructionCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--profile-name")] string ProfileName
) : AzOptions
{
    [CliOption("--amount")]
    public string? Amount { get; set; }

    [CliOption("--creation-date")]
    public string? CreationDate { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }
}