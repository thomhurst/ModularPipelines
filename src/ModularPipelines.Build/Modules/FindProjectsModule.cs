using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

public class FindProjectsModule : Module<IReadOnlyList<File>>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithAlwaysRun()
        .Build();

    public override Task<IReadOnlyList<File>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<File>?>(
        [
            Sourcy.DotNet.Projects.ModularPipelines,
            Sourcy.DotNet.Projects.ModularPipelines_AmazonWebServices,
            Sourcy.DotNet.Projects.ModularPipelines_Azure,
            Sourcy.DotNet.Projects.ModularPipelines_Azure_Pipelines,
            Sourcy.DotNet.Projects.ModularPipelines_Chocolatey,
            Sourcy.DotNet.Projects.ModularPipelines_Cmd,
            Sourcy.DotNet.Projects.ModularPipelines_Docker,
            Sourcy.DotNet.Projects.ModularPipelines_DotNet,
            Sourcy.DotNet.Projects.ModularPipelines_Email,
            Sourcy.DotNet.Projects.ModularPipelines_Ftp,
            Sourcy.DotNet.Projects.ModularPipelines_Yarn,
            Sourcy.DotNet.Projects.ModularPipelines_Node,
            Sourcy.DotNet.Projects.ModularPipelines_Git,
            Sourcy.DotNet.Projects.ModularPipelines_GitHub,
            Sourcy.DotNet.Projects.ModularPipelines_Google,
            Sourcy.DotNet.Projects.ModularPipelines_Helm,
            Sourcy.DotNet.Projects.ModularPipelines_Kubernetes,
            Sourcy.DotNet.Projects.ModularPipelines_MicrosoftTeams,
            Sourcy.DotNet.Projects.ModularPipelines_Slack,
            Sourcy.DotNet.Projects.ModularPipelines_TeamCity,
            Sourcy.DotNet.Projects.ModularPipelines_Terraform,
            Sourcy.DotNet.Projects.ModularPipelines_WinGet
        ]);
    }
}