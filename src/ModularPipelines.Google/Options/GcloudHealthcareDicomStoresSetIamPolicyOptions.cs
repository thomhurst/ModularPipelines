using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "dicom-stores", "set-iam-policy")]
public record GcloudHealthcareDicomStoresSetIamPolicyOptions(
[property: PositionalArgument] string DicomStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;