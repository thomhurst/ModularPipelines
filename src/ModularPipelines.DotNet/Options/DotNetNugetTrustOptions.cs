using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CliCommand("nuget", "trust", "[command]", "[Options]")]
[ExcludeFromCodeCoverage]
public record DotNetNugetTrustOptions : DotNetOptions
{
}
