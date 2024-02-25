*   If no collapsible log mechanisms detected, still segregate logs by a simple text separator
*   `[SkipIfNoGitHubToken]` attribute, useful on modules that will fail without access to the GitHub token, which can happen for PRs from non-maintiners or dependabot
*   `ICommandLogger` and `ILogoPrinter` interfaces are now public, to allow for custom loggers and printers.
*   `Command`, `CommandLogger`, and `LogoPrinter` are now `public sealed` to allow for custom implementations to encapsulate them.
*   `CommandResult` now has a public constructor. 
*   Added a `Default` option to the `CommandLogging` enum.
*   Added a `PrintLogo` option to `PipelineOptions` to allow logo printing to be disabled.