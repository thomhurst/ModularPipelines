using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "hl7v2-stores", "list")]
public record GcloudHealthcareHl7v2StoresListOptions(
[property: CommandSwitch("--dataset")] string Dataset,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;