using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

public enum Verbosity
{
    [EnumValue("quiet")]
    Quiet,

    [EnumValue("minimal")]
    Minimal,

    [EnumValue("normal")]
    Normal,

    [EnumValue("detailed")]
    Detailed,

    [EnumValue("diagnostic")]
    Diagnostic,
}
