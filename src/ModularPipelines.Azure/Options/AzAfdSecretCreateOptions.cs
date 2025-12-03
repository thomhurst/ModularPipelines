using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("afd", "secret", "create")]
public record AzAfdSecretCreateOptions(
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--secret-name")] string SecretName,
[property: CliOption("--secret-source")] string SecretSource
) : AzOptions
{
    [CliOption("--secret-version")]
    public string? SecretVersion { get; set; }

    [CliFlag("--use-latest-version")]
    public bool? UseLatestVersion { get; set; }
}