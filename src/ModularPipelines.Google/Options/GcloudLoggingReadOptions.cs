using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "read")]
public record GcloudLoggingReadOptions(
[property: PositionalArgument] string LogFilter
) : GcloudOptions
{
    [CommandSwitch("--freshness")]
    public string? Freshness { get; set; }

    [CommandSwitch("--order")]
    public string? Order { get; set; }

    [CommandSwitch("--billing-account")]
    public string? BillingAccount { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--project")]
    public new string? Project { get; set; }

    [CommandSwitch("--resource-names")]
    public string[]? ResourceNames { get; set; }

    [CommandSwitch("--bucket")]
    public string? Bucket { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--view")]
    public string? View { get; set; }
}