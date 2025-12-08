using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cheat-sheet")]
public record GcloudCheatSheetOptions : GcloudOptions;