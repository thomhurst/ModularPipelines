using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "secret", "update")]
public record AzAfdSecretUpdateOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secret-name")]
    public string? SecretName { get; set; }

    [CliOption("--secret-source")]
    public string? SecretSource { get; set; }

    [CliOption("--secret-version")]
    public string? SecretVersion { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--use-latest-version")]
    public bool? UseLatestVersion { get; set; }
}