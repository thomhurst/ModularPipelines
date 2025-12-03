using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "operations", "list")]
public record GcloudHealthcareOperationsListOptions(
[property: CliOption("--dataset")] string Dataset,
[property: CliOption("--location")] string Location
) : GcloudOptions;