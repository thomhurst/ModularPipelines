using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "msteams", "create")]
public record AzBotMsteamsCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CliOption("--calling-web-hook")]
    public string? CallingWebHook { get; set; }

    [CliFlag("--enable-calling")]
    public bool? EnableCalling { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}