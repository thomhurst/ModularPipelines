using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "tag", "create-generic-criteria")]
public record AzDataprotectionBackupPolicyTagCreateGenericCriteriaOptions : AzOptions
{
    [CommandSwitch("--days-of-month")]
    public string? DaysOfMonth { get; set; }

    [CommandSwitch("--days-of-week")]
    public string? DaysOfWeek { get; set; }

    [CommandSwitch("--months-of-year")]
    public string? MonthsOfYear { get; set; }

    [CommandSwitch("--weeks-of-month")]
    public string? WeeksOfMonth { get; set; }
}