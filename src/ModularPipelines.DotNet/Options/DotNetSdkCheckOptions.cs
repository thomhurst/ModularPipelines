using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("sdk", "check")]
[ExcludeFromCodeCoverage]
public record DotNetSdkCheckOptions : DotNetOptions
{
}
