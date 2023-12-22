using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "check-name-availability")]
public record AzAccountManagementGroupCheckNameAvailabilityOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;