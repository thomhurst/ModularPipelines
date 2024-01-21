using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetMsbuildOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "<MSBUILD_ARGUMENTS>")]
    public string? MsbuildArguments { get; set; }
}
