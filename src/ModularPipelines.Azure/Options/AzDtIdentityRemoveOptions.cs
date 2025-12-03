using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "identity", "remove")]
public record AzDtIdentityRemoveOptions(
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}