using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("assured", "workloads", "list")]
public record GcloudAssuredWorkloadsListOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--organization")] string Organization
) : GcloudOptions;