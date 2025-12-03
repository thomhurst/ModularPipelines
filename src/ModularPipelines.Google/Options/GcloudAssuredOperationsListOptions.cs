using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("assured", "operations", "list")]
public record GcloudAssuredOperationsListOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--organization")] string Organization
) : GcloudOptions;