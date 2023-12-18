using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "devops-pipeline", "create")]
public record AzFunctionappDevopsPipelineCreateOptions : AzOptions
{
    [BooleanCommandSwitch("--allow-force-push")]
    public bool? AllowForcePush { get; set; }

    [CommandSwitch("--functionapp-name")]
    public string? FunctionappName { get; set; }

    [CommandSwitch("--github-pat")]
    public string? GithubPat { get; set; }

    [CommandSwitch("--github-repository")]
    public string? GithubRepository { get; set; }

    [CommandSwitch("--organization-name")]
    public string? OrganizationName { get; set; }

    [BooleanCommandSwitch("--overwrite-yaml")]
    public bool? OverwriteYaml { get; set; }

    [CommandSwitch("--project-name")]
    public string? ProjectName { get; set; }

    [CommandSwitch("--repository-name")]
    public string? RepositoryName { get; set; }
}

