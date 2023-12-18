using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "job", "disable")]
public record AzBatchJobDisableOptions(
[property: CommandSwitch("--job-id")] string JobId
) : AzOptions
{
    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--disable-tasks")]
    public bool? DisableTasks { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }
}