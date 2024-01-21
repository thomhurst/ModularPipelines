using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("nuget", "trust", "[command]", "[Options]")]
[ExcludeFromCodeCoverage]
public record DotNetNugetTrustOptions : DotNetOptions
{
}
