using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "dicom-stores", "list")]
public record GcloudHealthcareDicomStoresListOptions(
[property: CommandSwitch("--dataset")] string Dataset,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;