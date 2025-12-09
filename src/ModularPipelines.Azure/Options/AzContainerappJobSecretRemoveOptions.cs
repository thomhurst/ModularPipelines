using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "job", "secret", "remove")]
public record AzContainerappJobSecretRemoveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--secret-names")] string SecretNames
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}