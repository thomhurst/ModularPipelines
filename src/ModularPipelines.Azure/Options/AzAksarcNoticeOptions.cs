using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aksarc", "notice")]
public record AzAksarcNoticeOptions(
[property: CommandSwitch("--output-filepath")] string OutputFilepath
) : AzOptions
{
}