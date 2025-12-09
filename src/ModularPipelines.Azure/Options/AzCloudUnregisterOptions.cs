using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloud", "unregister")]
public record AzCloudUnregisterOptions(
[property: CliOption("--name")] string Name
) : AzOptions;