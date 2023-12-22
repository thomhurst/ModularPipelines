using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack", "mg", "list")]
public record AzStackMgListOptions(
[property: CommandSwitch("--management-group-id")] string ManagementGroupId
) : AzOptions;