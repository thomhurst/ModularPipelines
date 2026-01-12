using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliTool("npm")]
public record NpmOptions : CommandLineToolOptions;