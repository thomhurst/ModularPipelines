using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.PipelineWriters;

namespace ModularPipelines.Build;

public class GitHubYamlWriter : GitHubPipelineFileWriter
{
    public override async Task<GitHubPipelineFileWriterOptions> GetGitHubPipelineFileWriterOptions(IPipelineHookContext pipelineHookContext)
    {
        await Task.Yield();

        return new GitHubPipelineFileWriterOptions
        {
            Name = "Test",
            TriggerCondition = new TriggerCondition
            {
                Push = new() { Branches = ["main"] },
                PullRequest = new() { Branches = ["main"] },
                WorkflowDispatch = new WorkflowDispatch
                {
                    Inputs = new Dictionary<string, WorkflowDispatchInputObject>
                    {
                        ["publish-packages"] = new()
                        {
                            Description = "Publish packages?",
                            Type = "boolean",
                            Required= true,
                            Default = false,
                        },
                        ["is-alpha"] = new()
                        {
                            Description = "Alpha version?",
                            Type = "boolean",
                            Required= true,
                            Default = true,
                        },
                    },
                },
            },
            OutputPath = pipelineHookContext.Git().RootDirectory.GetFolder(".github").GetFolder("workflows")
                .GetFile("test.yml"),
            PipelineProjectPath = pipelineHookContext.Git().RootDirectory
                .FindFile(x => x.Name == "ModularPipelines.Build.csproj")!,
            Environment = "${{ github.ref == 'refs/heads/main' && 'Production' || 'Pull Requests' }}",
            CacheNuGet = true,
            DotNetRunFramework = "net7.0",
            ValuesToMask = new[]
            {
                "${{ secrets.DOTNET_FORMAT_PUSH_TOKEN }}", "${{ secrets.NuGet__ApiKey }}", "${{ secrets.ADMIN_TOKEN }}",
                "${{ secrets.CODACY_APIKEY }}",
            },
            EnvironmentVariables = new Dictionary<string, string>
            {
                ["DOTNET_ENVIRONMENT"] = "${{ github.ref == 'refs/heads/main' && 'Production' || 'Development' }}",
                ["NuGet__ApiKey"] = "${{ secrets.NuGet__ApiKey }}",
                ["GitHub__Actor"] = "${{ github.actor }}",
                ["GitHub__Repository__Id"] = "${{ github.repository_id }}",
                ["GitHub__StandardToken"] = "${{ secrets.DOTNET_FORMAT_PUSH_TOKEN }}",
                ["GitHub__AdminToken"] = "${{ secrets.ADMIN_TOKEN }}",
                ["GitHub__PullRequest__Number"] = "${{ github.event.number }}",
                ["GitHub__PullRequest__Branch"] = "${{ github.event.pull_request.head.ref }}",
                ["GitHub__PullRequest__Sha"] = "${{ github.event.pull_request.head.sha }}",
                ["GitHub__PullRequest__Author"] = "${{ github.event.pull_request.user.login }}",
                ["Publish__ShouldPublish"] = "${{ github.event.inputs.publish-packages }}",
                ["Publish__IsAlpha"] = "${{ github.event.inputs.is-alpha }}",
                ["Codacy__ApiKey"] = "${{ secrets.CODACY_APIKEY }}",
                ["CodeCov__Token"] = "${{ secrets.CODECOV_TOKEN }}",
                ["EMAIL_PASSWORD"] = "${{ secrets.EMAIL_PASSWORD }}",
            },
        };
    }
}