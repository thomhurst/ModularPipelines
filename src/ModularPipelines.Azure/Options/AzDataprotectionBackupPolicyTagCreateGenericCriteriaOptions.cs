using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataprotection", "backup-policy", "tag", "create-generic-criteria")]
public record AzDataprotectionBackupPolicyTagCreateGenericCriteriaOptions : AzOptions
{
    [CliOption("--days-of-month")]
    public string? DaysOfMonth { get; set; }

    [CliOption("--days-of-week")]
    public string? DaysOfWeek { get; set; }

    [CliOption("--months-of-year")]
    public string? MonthsOfYear { get; set; }

    [CliOption("--weeks-of-month")]
    public string? WeeksOfMonth { get; set; }
}