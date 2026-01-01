using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

public class FindProjectsModule : Module<IReadOnlyList<File>>, IAlwaysRun
{
    /// <summary>
    /// List of project names to publish. These correspond to projects in the src directory.
    /// </summary>
    private static readonly string[] ProjectNames =
    [
        "ModularPipelines",
        "ModularPipelines.AmazonWebServices",
        "ModularPipelines.Azure",
        "ModularPipelines.Azure.Pipelines",
        "ModularPipelines.Chocolatey",
        "ModularPipelines.Cmd",
        "ModularPipelines.Docker",
        "ModularPipelines.DotNet",
        "ModularPipelines.Email",
        "ModularPipelines.Ftp",
        "ModularPipelines.Yarn",
        "ModularPipelines.Node",
        "ModularPipelines.Git",
        "ModularPipelines.GitHub",
        "ModularPipelines.Google",
        "ModularPipelines.Helm",
        "ModularPipelines.Kubernetes",
        "ModularPipelines.MicrosoftTeams",
        "ModularPipelines.Slack",
        "ModularPipelines.TeamCity",
        "ModularPipelines.Terraform",
        "ModularPipelines.WinGet",
    ];

    public override Task<IReadOnlyList<File>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var gitRootDirectory = context.Git().RootDirectory;
        var srcDirectory = gitRootDirectory.GetFolder("src");

        var projects = ProjectNames
            .Select(name => srcDirectory.GetFolder(name).GetFile($"{name}.csproj"))
            .ToList();

        return Task.FromResult<IReadOnlyList<File>?>(projects);
    }
}