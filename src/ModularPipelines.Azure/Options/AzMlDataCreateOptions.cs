using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "data", "create")]
public record AzMlDataCreateOptions : AzOptions
{
    [CliOption("--datastore")]
    public string? Datastore { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--registry-name")]
    public string? RegistryName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--skip-validation")]
    public bool? SkipValidation { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}