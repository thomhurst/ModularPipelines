using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "run")]
public record GcloudComposerEnvironmentsRunOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Location,
[property: CliArgument] string Subcommand,
[property: CliArgument] string SubcommandNested,
[property: CliArgument] string CmdArgs
) : GcloudOptions;