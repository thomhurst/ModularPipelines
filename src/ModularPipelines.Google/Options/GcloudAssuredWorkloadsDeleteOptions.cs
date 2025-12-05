using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("assured", "workloads", "delete")]
public record GcloudAssuredWorkloadsDeleteOptions(
[property: CliArgument] string Workload,
[property: CliArgument] string Location,
[property: CliArgument] string Organization
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }
}