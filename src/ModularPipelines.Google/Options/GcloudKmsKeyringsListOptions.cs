using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keyrings", "list")]
public record GcloudKmsKeyringsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;