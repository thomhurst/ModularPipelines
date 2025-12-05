using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "os-images", "list")]
public record GcloudBmsOsImagesListOptions : GcloudOptions;