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

    [CliArgument(Name = "<MSBUILD_ARGUMENTS>")]
    public virtual string? MsbuildArguments { get; set; }

    [CliFlag("-h")]
    public virtual bool? H { get; set; }
}
