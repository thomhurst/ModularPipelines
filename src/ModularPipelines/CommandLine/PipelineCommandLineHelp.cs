namespace ModularPipelines.PipelineCli;

internal static class PipelineCommandLineHelp
{
    public const string Usage = """
        ModularPipelines command-line options:
          --help, -h                     Show this help.
          --dry-run                      Print the execution plan without running modules.
          --list-modules                 List registered modules and dependencies.
          --validate                     Validate the pipeline without running modules.
          --module <name>[,<name>...]    Run selected modules and their dependencies.
          --skip-module <name>[,<name>...] Exclude selected modules.
          --categories <name>[,<name>...]  Run selected categories.
          --ignore-categories <name>[,<name>...] Exclude selected categories.
          --                             Forward all remaining arguments to host configuration.

        Options accepting values may be repeated or written as --option=value.
        Unrecognized arguments are forwarded to host configuration.
        """;
}
