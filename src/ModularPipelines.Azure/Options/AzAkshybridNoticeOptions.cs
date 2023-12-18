using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("akshybrid", "notice")]
public record AzAkshybridNoticeOptions(
[property: CommandSwitch("--output-filepath")] string OutputFilepath
) : AzOptions
{
}