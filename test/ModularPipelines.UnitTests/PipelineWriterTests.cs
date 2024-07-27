using ModularPipelines.Context;
using ModularPipelines.GitHub.PipelineWriters;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class PipelineWriterTests : TestBase
{
    public static readonly FileSystem.File RandomFilePath = FileSystem.File.GetNewTemporaryFilePath();
    public class GitHubYamlWriter : GitHubPipelineFileWriter
    {
        public override async Task<GitHubPipelineFileWriterOptions> GetGitHubPipelineFileWriterOptions(
            IPipelineHookContext pipelineHookContext)
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
                                Required = true,
                                Default = false,
                            },
                            ["is-alpha"] = new()
                            {
                                Description = "Alpha version?",
                                Type = "boolean",
                                Required = true,
                                Default = true,
                            },
                        },
                    },
                },
                OutputPath = RandomFilePath.Path!,
                PipelineProjectPath = RandomFilePath.Path!,
                Environment = "${{ github.ref == 'refs/heads/main' && 'Production' || 'Pull Requests' }}",
                CacheNuGet = true,
                DotNetRunFramework = "net8.0",
                ValuesToMask = new[]
                {
                    "${{ secrets.DOTNET_FORMAT_PUSH_TOKEN }}", "${{ secrets.NuGet__ApiKey }}",
                    "${{ secrets.ADMIN_TOKEN }}",
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

    [Test]
    public async Task GitHubWriter()
    {
        await TestPipelineHostBuilder.Create()
            .AddModule<NotInParallelTests.Module1>()
            .AddPipelineFileWriter<GitHubYamlWriter>()
            .ExecutePipelineAsync();
        await Assert.That((await RandomFilePath.ReadAsync()).Trim()).
            Is.EqualTo($$$"""
                       name: Test
                       on:
                         push:
                           branches:
                           - main
                         pull_request:
                           branches:
                           - main
                         workflow_dispatch:
                           inputs:
                             publish-packages:
                               description: Publish packages?
                               type: boolean
                               required: true
                               default: false
                             is-alpha:
                               description: Alpha version?
                               type: boolean
                               required: true
                               default: true
                       jobs:
                         pipeline:
                           environment: ${{ github.ref == 'refs/heads/main' && 'Production' || 'Pull Requests' }}
                           runs-on: ubuntu-latest
                           steps:
                           - name: Mask Secret Values
                             run: >-
                               echo "::add-mask::${{ secrets.DOTNET_FORMAT_PUSH_TOKEN }}
                       
                               echo "::add-mask::${{ secrets.NuGet__ApiKey }}
                       
                               echo "::add-mask::${{ secrets.ADMIN_TOKEN }}
                       
                               echo "::add-mask::${{ secrets.CODACY_APIKEY }}
                           - name: Checkout
                             uses: actions/checkout@v3
                             with:
                               fetch-depth: 0
                               persist-credentials: false
                           - name: Setup .NET SDK
                             uses: actions/setup-dotnet@v3
                             with:
                               dotnet-version: 8.0.x
                           - name: Cache NuGet
                             uses: actions/cache@v3
                             with:
                               path: ~/.nuget/packages
                               key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
                               restore-keys: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
                           - name: Run Pipeline
                             run: dotnet run -c Release --framework net8.0 {{{RandomFilePath}}}
                             env:
                               DOTNET_ENVIRONMENT: ${{ github.ref == 'refs/heads/main' && 'Production' || 'Development' }}
                               NuGet__ApiKey: ${{ secrets.NuGet__ApiKey }}
                               GitHub__Actor: ${{ github.actor }}
                               GitHub__Repository__Id: ${{ github.repository_id }}
                               GitHub__StandardToken: ${{ secrets.DOTNET_FORMAT_PUSH_TOKEN }}
                               GitHub__AdminToken: ${{ secrets.ADMIN_TOKEN }}
                               GitHub__PullRequest__Number: ${{ github.event.number }}
                               GitHub__PullRequest__Branch: ${{ github.event.pull_request.head.ref }}
                               GitHub__PullRequest__Sha: ${{ github.event.pull_request.head.sha }}
                               GitHub__PullRequest__Author: ${{ github.event.pull_request.user.login }}
                               Publish__ShouldPublish: ${{ github.event.inputs.publish-packages }}
                               Publish__IsAlpha: ${{ github.event.inputs.is-alpha }}
                               Codacy__ApiKey: ${{ secrets.CODACY_APIKEY }}
                               CodeCov__Token: ${{ secrets.CODECOV_TOKEN }}
                               EMAIL_PASSWORD: ${{ secrets.EMAIL_PASSWORD }}
                       """.Trim());
    }
}
