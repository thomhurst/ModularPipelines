using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "dataplane-v2-encryption", "disable")]
public record GcloudContainerFleetDataplaneV2EncryptionDisableOptions : GcloudOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}