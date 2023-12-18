using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "origin", "list")]
public record AzCdnOriginListOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;