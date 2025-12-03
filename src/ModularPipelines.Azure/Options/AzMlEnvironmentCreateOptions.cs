using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "environment", "create")]
public record AzMlEnvironmentCreateOptions : AzOptions
{
    [CliOption("--build-context")]
    public string? BuildContext { get; set; }

    [CliOption("--conda-file")]
    public string? CondaFile { get; set; }

    [CliOption("--datastore")]
    public string? Datastore { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dockerfile-path")]
    public string? DockerfilePath { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--registry-name")]
    public string? RegistryName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}