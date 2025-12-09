using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "identity", "assign")]
public record AzDtIdentityAssignOptions(
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scopes")]
    public string? Scopes { get; set; }
}