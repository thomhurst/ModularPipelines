using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "dicom-stores", "set-iam-policy")]
public record GcloudHealthcareDicomStoresSetIamPolicyOptions(
[property: CliArgument] string DicomStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;