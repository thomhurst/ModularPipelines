using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "dicom-stores", "get-iam-policy")]
public record GcloudHealthcareDicomStoresGetIamPolicyOptions(
[property: CliArgument] string DicomStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions;