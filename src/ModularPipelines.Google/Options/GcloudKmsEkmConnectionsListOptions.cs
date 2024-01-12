using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "ekm-connections", "list")]
public record GcloudKmsEkmConnectionsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;