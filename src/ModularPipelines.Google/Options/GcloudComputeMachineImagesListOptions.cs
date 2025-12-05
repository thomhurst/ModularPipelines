using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "machine-images", "list")]
public record GcloudComputeMachineImagesListOptions : GcloudOptions;