using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "application-configuration-service", "git", "repo", "add")]
public record AzSpringApplicationConfigurationServiceGitRepoAddOptions(
[property: CliOption("--label")] string Label,
[property: CliOption("--name")] string Name,
[property: CliOption("--patterns")] string Patterns,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service,
[property: CliOption("--uri")] string Uri
) : AzOptions
{
    [CliOption("--ca-cert-name")]
    public string? CaCertName { get; set; }

    [CliOption("--host-key")]
    public string? HostKey { get; set; }

    [CliOption("--host-key-algorithm")]
    public string? HostKeyAlgorithm { get; set; }

    [CliOption("--host-key-check")]
    public string? HostKeyCheck { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--private-key")]
    public string? PrivateKey { get; set; }

    [CliOption("--search-paths")]
    public string? SearchPaths { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}