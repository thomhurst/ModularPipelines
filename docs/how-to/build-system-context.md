# Publish CI outputs and annotations

Use `context.Environment.BuildSystem` from a module or pipeline context. These operations are part of the core package; provider packages are not required.

```
using ModularPipelines;

using ModularPipelines.Context;



var build = context.Environment.BuildSystem;

await build.SetOutputAsync("version", "1.2.3", cancellationToken);

if (build.Is(BuildSystem.GitHubActions) || build.Is(BuildSystem.AzurePipelines))

{

    await build.AppendSummaryAsync("## Release\nBuilt version **1.2.3**.", cancellationToken);

}

await build.AnnotateAsync(BuildAnnotationLevel.Warning,

    "Review this configuration", "src/Settings.cs", 42, cancellationToken);



if (build.Is(BuildSystem.AzurePipelines) || build.Is(BuildSystem.TeamCity))

{

    await build.SetBuildNumberAsync("1.2.3", cancellationToken);

}
```

Register secrets with `context.Security.Secrets.AddSecret(...)` **before** publishing content. Values, messages, file names, and summaries are masked before encoding or writing. Outputs therefore cannot be used to transfer secret values. Output names must contain only ASCII letters, digits, `_`, `-`, or `.`, and cannot contain a registered secret. Line numbers are one-based and require a file name.

## Provider support[​](#provider-support "Direct link to Provider support")

| Operation    | GitHub Actions                                      | Azure Pipelines                                                   | TeamCity                                              |
| ------------ | --------------------------------------------------- | ----------------------------------------------------------------- | ----------------------------------------------------- |
| Output       | Step output in `GITHUB_OUTPUT`                      | `task.setvariable` with `isOutput=true`                           | `setParameter`                                        |
| Summary      | Append Markdown to `GITHUB_STEP_SUMMARY`            | Upload a Markdown attachment per call                             | Unsupported                                           |
| Annotation   | Notice, warning, or error; optional source location | Warning or error; optional source location; notice is unsupported | Log message with severity and source location as text |
| Build number | Unsupported                                         | `build.updatebuildnumber`                                         | `buildNumber`                                         |

Local execution does nothing, except argument validation and cancellation. Unsupported operations in CI (including other detected providers and unknown CI environments) throw `NotSupportedException`. Missing GitHub output or summary environment files throw `InvalidOperationException`. I/O failures propagate to the caller. Each method accepts a cancellation token; command writes are synchronous and check cancellation before writing.

GitHub outputs preserve multiline values using unique heredoc delimiters. Give the workflow step an `id` to read `${{ steps.<id>.outputs.version }}` later. Summary calls append a newline. The shared summary writer serializes writes with the automatic summary from `ModularPipelines.GitHub` and enforces the combined 1 MiB UTF-8 limit. A user summary that exceeds the remaining space throws `InvalidOperationException`. The automatic summary still skips content that does not fit and omits an oversized dependency graph when the rest fits.

Azure output variables require a named YAML task and are read through that task's output reference. A summary call creates a unique UTF-8 Markdown file under `AGENT_TEMPDIRECTORY` (or the system temporary directory). The file remains available for the agent to upload and is cleaned with the agent's temporary directory. Each call creates a separate attachment, rather than editing a previous attachment.

TeamCity outputs are build parameters. Use `env.` or `system.` prefixes when appropriate, and declare parameters in the build configuration when required by TeamCity. Annotations use service-message log severity; they do not create code inspection records or change the pipeline's module result. Provider configuration can affect how error messages influence the build result.

See the provider references for consuming outputs and configuring their UI: [GitHub workflow commands](https://docs.github.com/en/actions/reference/workflows-and-actions/workflow-commands), [Azure logging commands](https://learn.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands?view=azure-devops), and [TeamCity service messages](https://www.jetbrains.com/help/teamcity/service-messages.html).
