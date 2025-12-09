using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "create")]
public record AzDtCreateOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pna")]
    public string? Pna { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scopes")]
    public string? Scopes { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}