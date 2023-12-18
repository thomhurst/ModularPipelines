using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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