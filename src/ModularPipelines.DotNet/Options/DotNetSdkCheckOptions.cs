using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CliCommand("sdk", "check")]
[ExcludeFromCodeCoverage]
public record DotNetSdkCheckOptions : DotNetOptions
{
}
