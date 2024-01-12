using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "ekm-config", "describe")]
public record GcloudKmsEkmConfigDescribeOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;