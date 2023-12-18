using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "configuration-profile", "version", "list")]
public record AzAutomanageConfigurationProfileVersionListOptions(
[property: CommandSwitch("--configuration-profile-name")] string ConfigurationProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}