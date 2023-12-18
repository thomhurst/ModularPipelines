using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("group", "exists")]
public record AzGroupExistsOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;