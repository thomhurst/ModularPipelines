using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Git.Options;

[ExcludeFromCodeCoverage]
[CliTool("git")]
public record GitOptions : CommandLineToolOptions;