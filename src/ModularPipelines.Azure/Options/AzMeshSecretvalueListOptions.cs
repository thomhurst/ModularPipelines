using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mesh", "secretvalue", "list")]
public record AzMeshSecretvalueListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--secret-name")] string SecretName
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}