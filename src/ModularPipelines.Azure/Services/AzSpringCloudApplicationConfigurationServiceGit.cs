using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "application-configuration-service")]
public class AzSpringCloudApplicationConfigurationServiceGit
{
    public AzSpringCloudApplicationConfigurationServiceGit(
        AzSpringCloudApplicationConfigurationServiceGitRepo repo
    )
    {
        Repo = repo;
    }

    public AzSpringCloudApplicationConfigurationServiceGitRepo Repo { get; }
}