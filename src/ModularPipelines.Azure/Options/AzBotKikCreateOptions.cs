using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "kik", "create")]
public record AzBotKikCreateOptions(
[property: CliOption("--key")] string Key,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--user-name")] string UserName
) : AzOptions
{
    [CliFlag("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CliFlag("--is-validated")]
    public bool? IsValidated { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}