# ModularPipelines.Development.Analyzers

Repository-only Roslyn analyzers and code fixes for developing ModularPipelines.
This project is not packaged for consumers (`IsPackable` is `false`).
[Directory.Build.props](../../Directory.Build.props) references it as an analyzer
for repository projects.

The analyzers report:

- `MP0011`: CLI option, flag, and argument properties that are not virtual.
- `MP0012`: eligible command methods returning `Task<CommandResult>` that are not virtual.

The corresponding code fixes add the `virtual` modifier. See the analyzer source
and [release tracking](AnalyzerReleases.Unshipped.md) for the current rules.

For pipeline usage and current module examples, see the
[root README](../../README.md) and [documentation](https://modularpipelines.tomlonghurst.dev/).
