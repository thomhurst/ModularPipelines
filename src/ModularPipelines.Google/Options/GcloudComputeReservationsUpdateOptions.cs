using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "reservations", "update")]
public record GcloudComputeReservationsUpdateOptions(
[property: CliArgument] string Reservation
) : GcloudOptions
{
    [CliOption("--add-share-with")]
    public string[]? AddShareWith { get; set; }

    [CliOption("--remove-share-with")]
    public string[]? RemoveShareWith { get; set; }

    [CliOption("--vm-count")]
    public string? VmCount { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}