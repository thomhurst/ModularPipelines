using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("urestackhci", "storagepath", "create")]
public record AzAzurestackhciStoragepathCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}