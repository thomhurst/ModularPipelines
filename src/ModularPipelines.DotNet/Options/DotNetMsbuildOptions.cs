using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetMsbuildOptions : DotNetOptions
{
    public DotNetMsbuildOptions(
        string msbuildArguments
    )
    {
        CommandParts = ["msbuild", "<MSBUILD_ARGUMENTS>"];

        MsbuildArguments = msbuildArguments;
    }

    public DotNetMsbuildOptions()
    {
        CommandParts = ["msbuild", "<MSBUILD_ARGUMENTS>"];
    }

    [PositionalArgument(PlaceholderName = "<MSBUILD_ARGUMENTS>")]
    public string? MsbuildArguments { get; set; }

    [BooleanCommandSwitch("-h")]
    public virtual bool? H { get; set; }
}
