using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "dicom-stores", "list")]
public record GcloudHealthcareDicomStoresListOptions(
[property: CliOption("--dataset")] string Dataset,
[property: CliOption("--location")] string Location
) : GcloudOptions;