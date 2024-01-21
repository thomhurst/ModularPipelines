using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("list", "[<PROJECT>]", "reference")]
[ExcludeFromCodeCoverage]
public record DotNetListReferenceOptions : DotNetOptions
{
}
