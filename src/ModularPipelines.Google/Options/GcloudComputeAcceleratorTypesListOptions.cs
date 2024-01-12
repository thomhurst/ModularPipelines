using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "accelerator-types", "list")]
public record GcloudComputeAcceleratorTypesListOptions : GcloudOptions;