using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "operations", "describe")]
public record GcloudSpannerOperationsDescribeOptions(
[property: CliArgument] string OperationId,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--instance-config")] string InstanceConfig
) : GcloudOptions
{
    [CliOption("--backup")]
    public string? Backup { get; set; }

    [CliOption("--database")]
    public string? Database { get; set; }
}