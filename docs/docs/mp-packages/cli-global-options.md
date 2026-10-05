---
title: Global options in specialist CLI packages
---

# Global options in specialist CLI packages

Some tools have subcommands with inherited settings. Others accept one invocation with
files, goals, tasks, or a filter. An empty abstract base class does not mean options are
missing when the executable has one generated `Execute` command.

The following audit covers the specialist tools tracked in issue #5711. Versions identify
the checked-in generation manifests at the start of the audit; official references were
checked on 4 October 2026. Snyk and Flyway also have captured help fixtures in the generator
tests, from Snyk 1.1302.0 and the hosted Flyway 10.20.1 installation.

| Tool | Audited representation | Global-option result and evidence |
| --- | --- | --- |
| Maven 3.9.16 | `MavenExecuteOptions`, including `Define`, `Quiet`, `Debug`, and `GoalsAndPhases` | Goals and lifecycle phases are operands of one invocation. Root options already belong to the concrete class. [Maven CLI reference](https://maven.apache.org/ref/3.9.11/maven-embedder/cli.html). |
| Gradle 9.8.0 | `GradleExecuteOptions`, including `Tasks` and build settings | Tasks are operands. Gradle-wide options already belong to the concrete class; task-specific options must remain associated with their task. [Gradle command-line reference](https://docs.gradle.org/current/userguide/command_line_interface.html). |
| SonarScanner CLI 8.0.1.6346 | `SonarScannerExecuteOptions`, including repeated `Define` properties and `Debug` | One scanner invocation has no child command tree requiring inherited flags. [SonarScanner CLI](https://docs.sonarsource.com/sonarqube-cloud/analyzing-source-code/scanners/sonarscanner-cli). |
| Snyk 1.1302.0 help | `SnykOptions.Debug` inherited by command classes | Root help documents `-d`; command help repeats it. Generate it once on the base class. Do not promote `--unmanaged` from a root-help example: it is limited to scanning commands. [Snyk CLI help](https://docs.snyk.io/developer-tools/snyk-cli/commands), [debugging](https://docs.snyk.io/developer-tools/snyk-cli/debugging-the-snyk-cli). |
| Hadolint 2.15.1 | `HadolintExecuteOptions`, including `Config`, `Format`, and file operands | One Dockerfile-linting invocation; options are already available on its concrete class. [Hadolint CLI](https://github.com/hadolint/hadolint#cli). |
| jq 1.8.2 | `JqExecuteOptions`, including `Filter` and input/output options | Filters are jq programs, not CLI subcommands. Root options stay on the execution class. [jq manual](https://jqlang.org/manual/). |
| Flyway 10.20.1 | `FlywayOptions` inherited by migration and other command classes | Parse the root configuration table and `-X`, `-q`, and `-n` flags. Root configuration uses `-key=value`; list settings accept a comma-delimited string. Keep command-only settings on their command. [Flyway command-line parameters](https://documentation.red-gate.com/flyway/reference/command-line-parameters). |
| Liquibase 5.0.3 | `LiquibaseOptions`, including `SearchPath`, `LogLevel`, and supplemental licensed settings | Existing parsed and supplemental globals already share the base class. Preserve them, their value syntax, secret marking, and command-level deduplication. [Liquibase parameters](https://docs.liquibase.com/reference-guide/parameters/what-are-parameters). |
| Ansible core 2.21.4 | `AnsibleExecuteOptions`, including `Pattern`, repeated `Inventory`, and `Verbose` | `ansible` is one executable; `ansible-playbook` and other executables are not its subcommands. [Ansible CLI](https://docs.ansible.com/projects/ansible/latest/cli/ansible.html). |
| ShellCheck 0.11.0 | `ShellcheckExecuteOptions`, including `Format` and `Files` | One file-analysis invocation; no child command requires inherited settings. [ShellCheck manual](https://github.com/koalaman/shellcheck/blob/master/shellcheck.1.md). |
| Newman 6.2.2 | `NewmanRunOptions` | Root options are help/version control actions. Collection, reporter, timeout, and variable options belong to `run`; “global variables” do not mean executable-wide flags. A separate value-arity defect is tracked in #5721. [Newman command-line options](https://github.com/postmanlabs/newman#command-line-options). |

## Migrating callers

Snyk command initializers can continue setting `Debug`; the property is inherited from
`SnykOptions` instead of repeated on each generated command class. Reflection code that
uses `DeclaredOnly` must account for inherited properties.

Set Flyway connection and configuration settings directly on the chosen command options,
for example `Url`, `User`, `Password`, and `Locations`. These are inherited from
`FlywayOptions`. Supply `Locations` as one comma-delimited string. The generator emits
configuration before the subcommand using `-key=value` and emits the diagnostic and
non-interactive switches as flags. Passwords and license keys retain secret metadata.

Supply `Placeholders` and `JdbcProperties` as `KeyValue` collections. Each entry renders as
`-placeholders.name=value` or `-jdbcProperties.name=value`, respectively. JDBC properties
are marked as secrets because drivers can accept credentials through this map.
See [Flyway placeholders](https://documentation.red-gate.com/fd/flyway-placeholders-namespace-277579022.html)
and [JDBC properties](https://documentation.red-gate.com/flyway/reference/configuration/environments-namespace/environment-jdbc-properties-namespace).

The flat-tool execution classes and Liquibase's existing global properties keep their
current placement. Help/version actions are not configuration to apply to every command.
