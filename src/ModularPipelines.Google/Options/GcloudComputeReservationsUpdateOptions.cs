using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "reservations", "update")]
public record GcloudComputeReservationsUpdateOptions(
[property: PositionalArgument] string Reservation
) : GcloudOptions
{
    [CommandSwitch("--add-share-with")]
    public string[]? AddShareWith { get; set; }

    [CommandSwitch("--remove-share-with")]
    public string[]? RemoveShareWith { get; set; }

    [CommandSwitch("--vm-count")]
    public string? VmCount { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}