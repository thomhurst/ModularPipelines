# Threat model

## What this project does and where untrusted input enters

ModularPipelines is a .NET library for writing CI/CD pipelines in C#. A user writes a console app that defines
modules; the engine resolves dependencies between them and runs them in parallel. Modules usually run CLI tools
(dotnet, git, docker, kubectl and about 60 others), make HTTP calls, and read or write files.

The pipeline author is trusted. Code in the user's pipeline project, its configuration, and its command-line
arguments run with the author's privileges by design. "A module can run arbitrary commands" or "a pipeline can
delete files" is not a vulnerability.

The library must still be safe with data that the author does not control. Untrusted or semi-trusted input
enters through:

- **Values passed into command options.** Branch names, PR titles, commit messages, file names, and environment
  variables from CI often come from outside contributors. The library builds argument lists from typed options
  (`src/ModularPipelines/Options`, `src/ModularPipelines/Helpers/Internal/CommandLineArguments.cs`,
  `CliCommandFactory.cs`, `ProcessCommand.cs`). A value must never become a second argument, a different
  option, or shell syntax unless the author explicitly asked for shell execution (`Bash`/`Cmd` options).
- **Command output, HTTP responses, and downloaded files.** Processes and remote servers can return any bytes.
  This includes the HTTP stack (`src/ModularPipelines/Http`), downloads, and zip extraction
  (`src/ModularPipelines/Context/Zip.cs`). Archive entries must not escape the destination folder.
- **The module result cache** (`src/ModularPipelines/Caching`). Cache files on disk may be shared between CI runs
  or restored from a remote cache, so their contents are only semi-trusted.
- **Distributed mode** (`src/ModularPipelines/Distributed`, `src/ModularPipelines.Distributed.*`). A master
  process hands module assignments to worker processes over SignalR (optionally exposed through a Cloudflare
  tunnel), Redis, or S3. Messages, artifacts, and serialized module results cross a network. Anyone who can reach
  the SignalR hub without the access token, or who can write to the Redis or S3 store, is untrusted.

## Security goals

1. **Secret masking.** Values registered as secrets (`src/ModularPipelines/Secrets`, `[SecretValue]`,
   `BufferedSecretRemasker`, `HttpBodySecretRedactor`, `HttpUriRedactor`) must not appear in console output,
   log files, CI annotations, pipeline reports, exception messages, or telemetry (`src/ModularPipelines.OpenTelemetry`).
   This includes secrets split across output chunks or encoded in URLs and HTTP bodies.
2. **No argument or shell injection** from option values, as described above.
3. **Path safety.** Extraction, artifact download, and cache restore must stay inside their target folder.
   Temporary files must not be readable by other local users.
4. **Distributed-mode authentication.** A network-reachable master must reject workers without the access token,
   and workers must not run code or load types chosen by a peer that is not authenticated.
5. **Safe deserialization.** Module results, cache entries, and distributed messages must not allow arbitrary
   type instantiation (see `src/ModularPipelines/Distributed/Serialization` and
   `src/ModularPipelines/Serialization/StableTypeName.cs`).

## Components that matter most / least

- **Most:** `src/ModularPipelines` (core engine, command execution, secrets, HTTP, file system, caching,
  distributed core), `src/ModularPipelines.Distributed.*`, `src/ModularPipelines.OpenTelemetry`, and
  hand-written integrations that handle credentials or remote content: `Email`, `Ftp`, `Git`, `GitHub`,
  `Slack`, `MicrosoftTeams`, `Azure.Pipelines`, `TeamCity`.
- **Less:** generated CLI integrations (`src/ModularPipelines.<Tool>/Options/*.Generated.cs`). They are typed
  option records produced by `tools/ModularPipelines.OptionsGenerator`. A bug in how an option is rendered is
  most likely in the shared core argument builder or in the generator, so please report it there rather than
  per tool.
- **Out of scope:** `src/ModularPipelines.Build` (this repository's own build pipeline), `src/ModularPipelines.Examples`,
  `test/`, `docs/`, `tools/` (developer tooling that scrapes CLI help text), analyzers and the source generator
  (they run at compile time on the author's own code), and the `.github/` workflows.

## How to exercise it

- The image builds `ModularPipelines.slnx` and the test projects listed in `.oss-scanner/Dockerfile`.
- `test/ModularPipelines.UnitTests` contains most behaviour tests, including secret masking, command
  building, zip extraction, and caching. Run it with
  `dotnet run --project test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj -c Debug --framework net11.0 --no-build`.
  Add `-- --treenode-filter "/*/*/<ClassName>/*"` to run one class.
- `test/ModularPipelines.Distributed.*UnitTests` cover the distributed protocol. Some tests need Redis or other
  services that are not available offline and will fail or skip.
- A minimal pipeline is a console app that calls `Pipeline.CreateBuilder(args)`, adds modules with
  `builder.AddModule<T>()`, and calls `await builder.RunAsync()`. The template in
  `src/ModularPipelines.Templates/templates/modularpipeline` is a complete example.

## How we rate severity

- **Critical:** remote code execution or authentication bypass in distributed mode by a network attacker
  without the access token.
- **High:** a registered secret leaking into output, logs, reports, or telemetry in a normal configuration;
  argument or shell injection from an option value; path traversal that writes outside the target folder.
- **Medium:** secret leaks that need an unusual configuration; unsafe temp-file permissions; unsafe
  deserialization that needs write access to the local cache folder.
- **Low:** denial of service (memory or CPU exhaustion, hangs) from command output, HTTP responses, or archives.
  A pipeline that runs for too long is visible to its author and is bounded by CI timeouts.

## How we would like reports

- Name the public API involved and include a small pipeline or unit test (TUnit) that reproduces the issue.
- Patches should match the surrounding code style. Library code under `src/` uses `ConfigureAwait(false)`.
- Do not propose hand edits to `*.Generated.cs` files. Fix the generator in `tools/ModularPipelines.OptionsGenerator` instead.

## Anything to leave alone

- The pipeline author running arbitrary commands, reading environment variables, or accessing the network.
- Secrets the author prints themselves without registering them as secrets.
- Vulnerabilities in the external CLI tools that the integrations wrap.
- Prerelease SDK usage: the repository targets .NET 11 RC on purpose ahead of the .NET 11 GA release.
