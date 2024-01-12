using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "ekm-config", "update")]
public record GcloudKmsEkmConfigUpdateOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions
{
    [CommandSwitch("--default-ekm-connection")]
    public string? DefaultEkmConnection { get; set; }
}