using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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