using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "links", "describe")]
public record GcloudLoggingLinksDescribeOptions(
[property: PositionalArgument] string LinkId,
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions
{
    [CommandSwitch("--billing-account")]
    public string? BillingAccount { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--project")]
    public new string? Project { get; set; }
}