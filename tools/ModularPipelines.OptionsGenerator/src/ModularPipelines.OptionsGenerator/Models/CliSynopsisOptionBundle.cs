namespace ModularPipelines.OptionsGenerator.Models;

internal sealed record CliSynopsisOptionBundle(
    string? PrimarySwitch,
    IReadOnlySet<string> OptionSwitches,
    IReadOnlySet<string> DirectOptionalSwitches,
    bool IsOptional);
