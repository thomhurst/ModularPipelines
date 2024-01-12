using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "operations", "list")]
public record GcloudHealthcareOperationsListOptions(
[property: CommandSwitch("--dataset")] string Dataset,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;