using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "operations", "describe")]
public record GcloudSpannerOperationsDescribeOptions(
[property: PositionalArgument] string OperationId,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--instance-config")] string InstanceConfig
) : GcloudOptions
{
    [CommandSwitch("--backup")]
    public string? Backup { get; set; }

    [CommandSwitch("--database")]
    public string? Database { get; set; }
}