using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "application-configuration-service")]
public class AzSpringApplicationConfigurationServiceGit
{
    public AzSpringApplicationConfigurationServiceGit(
        AzSpringApplicationConfigurationServiceGitRepo repo
    )
    {
        Repo = repo;
    }

    public AzSpringApplicationConfigurationServiceGitRepo Repo { get; }
}