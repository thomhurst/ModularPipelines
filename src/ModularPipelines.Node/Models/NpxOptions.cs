using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliTool("npx")]
public record NpxOptions : CommandLineToolOptions;