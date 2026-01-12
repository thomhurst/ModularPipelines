using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Yarn.Models;

[CliTool("yarn")]
public record YarnOptions : CommandLineToolOptions;