# ModularPipelines.OptionsGenerator

`ModularPipelines.OptionsGenerator` turns CLI metadata into the strongly typed options,
services, extension methods, enums, and Markdown reference used by ModularPipelines tool
integrations.

The repository's first-party integrations are generated from installed CLI help or public
HTML documentation. The packaged .NET tool can also generate private integrations from a
versioned JSON definition. Generated files are outputs: change the scraper, generator, type
override, or JSON definition and regenerate instead of editing generated files by hand.

## Architecture

The first-party generation path is:

```text
OptionsGeneratorCommand
  -> CodeGeneratorOrchestrator
     -> ICliScraper (preferred) or ICliDocumentationScraper (fallback)
     -> OptionTypeEnhancer (HTML scraping only)
     -> CliToolDefinition
     -> ICodeGenerator implementations
     -> command-coverage validation
     -> generated files, deletion records, and coverage manifest
```

The main components are:

| Area | Responsibility |
| --- | --- |
| `OptionsGeneratorCommand.cs` | Parses command-line options, registers scrapers and generators, and selects first-party or external-definition generation. |
| `Scrapers/Cli/` | Runs installed executables, traverses their command trees, and parses help output. `CliScraperBase` supplies parallel traversal, caching, and common parsing hooks. |
| `Scrapers/Base/` and `Scrapers/*DocumentationScraper.cs` | Scrapes public documentation. This is the fallback for tools that cannot be fully described through CLI help. |
| `TypeDetection/` and `TypeOverrides/` | Improves scraped option types with help-text detectors, heuristics, and reviewed per-tool JSON overrides. |
| `Models/` | Holds the scraper/generator boundary, principally `CliToolDefinition`, `CliCommandDefinition`, and `CliOptionDefinition`. |
| `Generators/` | Emits options, enums, services, subdomains, dependency registration, assembly metadata, and Markdown documentation. |
| `Generators/CommandCoverageGuard.cs` | Compares the current command tree with its committed baseline before output is accepted. |
| `External/` | Loads and validates versioned JSON definitions for private or out-of-tree integrations. |
| `scripts/` | Restricts staging to declared generated paths. |
| `src/ModularPipelines.OptionsGenerator.Tests/` | Unit tests for scraping, parsing, type detection, generation, safety, and coverage behavior. |

`CodeGeneratorOrchestrator` prefers an `ICliScraper` when both scraper kinds are registered
for a tool and `--use-cli-first` is enabled. It falls back to the registered HTML scraper
when the executable is unavailable or CLI scraping discovers no commands. Exceptions during
CLI availability checks, scraping, validation, version lookup, or generation are reported
without HTML fallback. Type enhancement runs only after HTML scraping when
`--enhance-types` is enabled; CLI-first and external definitions bypass it. The generators
check for output collisions, then write files sequentially. A failed run can therefore
leave earlier outputs modified. Obsolete files are removed only when their generated
markers establish ownership.

External definitions bypass scraper registration. They enter at `CliToolDefinition`, use
the same generators and coverage guard, and track owned paths under
`.modular-pipelines-options/`. See
[Generate a private CLI integration](../../docs/docs/how-to/generate-private-cli-integration.md)
and [`examples/external-tool-definition.json`](examples/external-tool-definition.json).

## Run locally

Run commands from the repository root. The executable for a CLI-first scraper must be
installed and resolvable through `PATH`.

```powershell
pwsh scripts/Invoke-AgentDotNet.ps1 `
  -DotNetArguments @(
    'run',
    '--project',
    'tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj',
    '--framework',
    'net10.0',
    '--',
    '--tools',
    'helm',
    '--output-dir',
    '.'
  )
```

Use a comma-separated list for `--tools`, or `all`. Useful options are:

| Option | Purpose |
| --- | --- |
| `--use-cli-first <true\|false>` | Prefer installed CLI help over HTML documentation. Defaults to `true`. |
| `--enhance-types <true\|false>` | Run type detection after HTML scraping. Defaults to `true`. |
| `--change-manifest <path>` | Record every repository-relative generated or deleted path for safe automation. |
| `--approve-command-coverage-shrinkage` | Acknowledge reviewed command-set changes for this run, including same-version additions or removals. It does not bypass minimum counts or sentinel commands. |
| `--input <path>` | Generate an external integration from JSON. This cannot be combined with `--tools`. |

For reproducible CI scraping, set `MODULARPIPELINES_CLI_EXECUTABLE` to the verified absolute
path of the requested tool. The generator rejects an executable override when more than one
tool is requested. The override applies only when that tool is executed; helper programs used
by its scraper continue to resolve independently.

## Add a scraper

Prefer a CLI-first scraper because installed help is normally the authoritative source.
Use an HTML scraper only when the executable cannot expose the required command metadata.

### CLI-first scraper

1. Add `Scrapers/Cli/<Tool>CliScraper.cs`. Usually derive from `CliScraperBase`; implement
   `ToolName`, `NamespacePrefix`, `TargetNamespace`, and repository-relative
   `OutputDirectory`.
2. Reuse the base traversal and parsing hooks where possible. Override help invocation,
   subcommand extraction, option parsing, command normalization, or skip rules only for
   behavior specific to that CLI.
3. Override `CreateToolDefinition()` when the tool needs global options, executable
   prerequisite metadata, documentation metadata, or a command-coverage policy.
4. Register the scraper as `ICliScraper` in
   `OptionsGeneratorCommand.RegisterCliScrapers`.
5. Add the tool to `DocumentationExampleCatalog` and any generator catalogs that require
   explicit per-tool metadata. Startup validation deliberately fails when a registered tool
   is missing required catalog data.
6. Add focused tests under
   `src/ModularPipelines.OptionsGenerator.Tests/Scrapers/`. Use a controlled
   `ICliCommandExecutor`; tests must not depend on a developer's installed CLI or network.
   Cover the root, nested command discovery, options, positional arguments, global options,
   malformed/empty help, and tool-specific normalization.
7. Add the tool to the correct Linux or Windows matrix in
   `.github/workflows/generate-cli-options.yml`, including installation and version
   verification. Keep downloaded archives and binaries under runner temporary storage.
8. Generate once, review all output, and commit the new
   `Generated/<NamespacePrefix>.CommandCoverage.json` baseline with the generated API.

Some CLIs use a specialized scraper instead of `CliScraperBase`. Implement `ICliScraper`
directly when the base traversal model does not fit, but preserve streaming command output,
availability/version checks, cancellation, and deterministic definitions.

### HTML documentation scraper

1. Derive from `CliDocumentationScraperBase` or `HeadlessBrowserScraperBase`, or implement
   `ICliDocumentationScraper`.
2. Return a complete, deterministic `CliToolDefinition`; do not leak page ordering into
   generated output.
3. Register it in `OptionsGeneratorCommand.RegisterDocumentationScrapers`.
4. Add fixture-based parsing tests. Keep network and browser calls outside unit tests.
5. If both scraper kinds exist, test CLI-first and HTML-fallback selection.

### Type corrections

First improve the general detector or scraper when the source metadata can be interpreted
reliably. Use `TypeOverrides/<tool>.json` only for stable, reviewed exceptions. Add an
`OptionTypeEnhancerTests` case for new detector behavior and a scraper test for tool-local
behavior.

## Command-coverage guard

Each successful generation writes
`<integration>/Generated/<NamespacePrefix>.CommandCoverage.json`. The committed manifest
contains the normalized command list, inferred command groups, tool version, exclusions,
count, and SHA-256 fingerprint. It is both generated output and the baseline for the next
run; do not edit it manually.

Generation fails when:

- a generated integration has lost its committed coverage manifest;
- the current command count is below `MinimumCommandCount`;
- a configured sentinel command disappeared;
- the command set changed while the resolved tool version stayed the same, without explicit approval;
- a previously generated command disappeared without explicit approval;
- a known command group lost all children without explicit approval; or
- an exclusion lacks a full command and non-empty reason.

Configure invariants in `CliToolDefinition.CommandCoverage`. Sentinels should represent
stable commands from important branches of the CLI. Set a conservative minimum that catches
truncated or empty scraping without failing on normal upstream evolution. Every intentional
exclusion needs a durable explanation.

When command coverage changes:

1. Inspect the installed tool version and raw help output. A parser regression, missing
   plugin, authentication prompt, preview flag, or wrong executable must not be approved as
   upstream shrinkage.
2. Fix the scraper when the commands still exist, then regenerate.
3. If the change is expected, run once with
   `--approve-command-coverage-shrinkage` and review the generated API and coverage diff.
4. Add a documented exclusion instead when a command intentionally remains unsupported.

Approval permits same-version command-set changes, removed commands, and groups losing all
children. Minimum counts and sentinel requirements always remain enforced. Additions after
an intentional tool-version change need no approval.

## Validate a change

Run the generator test project:

```powershell
pwsh scripts/Invoke-AgentDotNet.ps1 `
  -DotNetArguments @(
    'run',
    '--project',
    'tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator.Tests/ModularPipelines.OptionsGenerator.Tests.csproj',
    '--framework',
    'net10.0',
    '--'
  )
```

After regeneration, build only the affected integration's solution. For example:

```powershell
pwsh scripts/Invoke-AgentDotNet.ps1 `
  -DotNetArguments @(
    'build',
    'src/ModularPipelines.Docker/ModularPipelines.Docker.slnx',
    '-c',
    'Release'
  )
```

Review:

- every generated and deleted path;
- command additions/removals and the coverage fingerprint;
- option names, types, arity, ordering, and secret metadata;
- generated documentation and examples;
- unexpected API removals or renames; and
- deterministic output from a second generation with the same inputs.

Do not build `ModularPipelines.All.slnx` for generator work.

## CI generation workflow

`.github/workflows/generate-cli-options.yml` runs weekly and on demand:

This regenerate-and-diff workflow is the authoritative staleness check. Generated-code
version attributes identify the generator release that produced a file, but matching a
version attribute does not prove that the committed content matches the current parser.
Regenerate outputs instead of changing version attributes in place.

1. Test the exact-path staging safety script.
2. Fan out one installed CLI per Linux or Windows matrix job.
3. install and verify the requested executable, then pin its resolved path;
4. run the generator with a change manifest;
5. allow `Stage-GeneratedChanges.ps1` to stage only declared generated paths and reject
   binary, oversized, root-level, or checkout-escape artifacts;
6. build only the affected integration solution;
7. create an automated pull request. Breaking generated API changes are accepted because
   the installed CLI is the source of truth.

Breaking generated API changes remain visible for human review. The workflow's manual
`approve-command-coverage-shrinkage` input is an explicit acknowledgement for a single run,
not a permanent weakening of the coverage policy.
