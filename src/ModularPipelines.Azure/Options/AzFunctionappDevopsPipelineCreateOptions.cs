using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "devops-pipeline", "create")]
public record AzFunctionappDevopsPipelineCreateOptions : AzOptions
{
    [CliFlag("--allow-force-push")]
    public bool? AllowForcePush { get; set; }

    [CliOption("--functionapp-name")]
    public string? FunctionappName { get; set; }

    [CliOption("--github-pat")]
    public string? GithubPat { get; set; }

    [CliOption("--github-repository")]
    public string? GithubRepository { get; set; }

    [CliOption("--organization-name")]
    public string? OrganizationName { get; set; }

    [CliFlag("--overwrite-yaml")]
    public bool? OverwriteYaml { get; set; }

    [CliOption("--project-name")]
    public string? ProjectName { get; set; }

    [CliOption("--repository-name")]
    public string? RepositoryName { get; set; }
}