using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedmachine", "extension", "image", "list")]
public record AzConnectedmachineExtensionImageListOptions(
[property: CliOption("--extension-type")] string ExtensionType,
[property: CliOption("--location")] string Location,
[property: CliOption("--publisher")] string Publisher
) : AzOptions;