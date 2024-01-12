using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments", "run")]
public record GcloudComposerEnvironmentsRunOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Subcommand,
[property: PositionalArgument] string SubcommandNested,
[property: PositionalArgument] string CmdArgs
) : GcloudOptions;