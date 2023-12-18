using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "tag", "create-absolute-criteria")]
public record AzDataprotectionBackupPolicyTagCreateAbsoluteCriteriaOptions(
[property: CommandSwitch("--absolute-criteria")] string AbsoluteCriteria
) : AzOptions
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