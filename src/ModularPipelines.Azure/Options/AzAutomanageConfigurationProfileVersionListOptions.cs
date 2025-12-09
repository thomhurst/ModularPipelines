using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automanage", "configuration-profile", "version", "list")]
public record AzAutomanageConfigurationProfileVersionListOptions(
[property: CliOption("--configuration-profile-name")] string ConfigurationProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;