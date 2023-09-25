using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

public enum Configuration
{
    [EnumValue("Debug")]
    Debug,

    [EnumValue("Release")]
    Release,
}
