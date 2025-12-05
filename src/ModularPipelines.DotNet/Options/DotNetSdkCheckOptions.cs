using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CliSubCommand("sdk", "check")]
[ExcludeFromCodeCoverage]
public record DotNetSdkCheckOptions : DotNetOptions
{
}
