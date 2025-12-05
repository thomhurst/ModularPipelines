using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "dicom-stores", "describe")]
public record GcloudHealthcareDicomStoresDescribeOptions(
[property: CliArgument] string DicomStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions;