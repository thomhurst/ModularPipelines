using ModularPipelines.Interfaces;
using ModularPipelines.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.PipelineCli;

internal static class PipelineCommandLineHelp
{
    public const string Usage = """
        ModularPipelines command-line options:
          --help, -h                     Show this help.
          --rerun-failed                 Rerun failures and dependents, reusing compatible successful results.
          --no-cache                     Disable cache and rerun result reuse.
          --dry-run                      Print the execution plan without running modules.
          --list-modules                 List registered modules and dependencies.
          --validate                     Validate the pipeline without running modules.
          --module <name>[,<name>...]    Run selected modules and their dependencies.
          --skip-module <name>[,<name>...] Exclude selected modules.
          --categories <name>[,<name>...]  Run selected categories.
          --ignore-categories <name>[,<name>...] Exclude selected categories.
          --tags <name>[,<name>...]        Run modules matching any selected tag.
          --ignore-tags <name>[,<name>...] Exclude modules matching any selected tag.
          --graph <mermaid|dot|json> [path] Export the dependency graph.
          --graph-path <path>              Set the graph output path explicitly.
          --                             Forward all remaining arguments to host configuration.

        Options accepting values may be repeated or written as --option=value.
        Module selectors accept module identifiers or type names, matched case-insensitively.
        A complete module selector takes precedence over a comma-separated list; repeat the option to combine identifiers containing commas.
        Unrecognized arguments are forwarded to host configuration.
        """;

    public static PipelineSummary Show(IConsoleWriter consoleWriter)
    {
        consoleWriter.WriteLine(Usage);
        var now = DateTimeOffset.UtcNow;
        return new PipelineSummary([], [], TimeSpan.Zero, now, now);
    }
}
