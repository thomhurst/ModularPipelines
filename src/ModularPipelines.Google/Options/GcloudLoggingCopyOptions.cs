using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "copy")]
public record GcloudLoggingCopyOptions(
[property: PositionalArgument] string BucketId,
[property: PositionalArgument] string Destination,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions
{
    [CommandSwitch("--log-filter")]
    public string? LogFilter { get; set; }

    [CommandSwitch("--billing-account")]
    public string? BillingAccount { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--project")]
    public new string? Project { get; set; }
}