using ModularPipelines.Context;
using ModularPipelines.GitHub.PipelineWriters;
using ModularPipelines.Interfaces;
using ModularPipelines.TestHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.GitHub.UnitTests.Engine;

public class PipelineWriterTests : TestBase
{
    public static readonly ModularPipelines.FileSystem.File RandomFilePath = ModularPipelines.FileSystem.File.GetNewTemporaryFilePath();

    private class DummyModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class GitHubYamlWriter : GitHubPipelineFileWriter
    {
        internal override async Task<GitHubPipelineFileWriterOptions> GetGitHubPipelineFileWriterOptions(
            IPipelineContext pipelineHookContext)
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
                DotNetRunFramework = "net10.0",
                ValuesToMask =
                [
                    "${{ secrets.DOTNET_FORMAT_PUSH_TOKEN }}", "${{ secrets.NuGet__ApiKey }}",
                    "${{ secrets.ADMIN_TOKEN }}",
                    "${{ secrets.CODACY_APIKEY }}"
                ],
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
        await TestPipelineBuilder.Create()
            .AddModule<DummyModule>()
            .ConfigureServices(services =>
                services.AddSingleton<IBuildSystemPipelineFileWriter>(new GitHubYamlWriter()))
            .RunAsync();
        // Normalize line endings for cross-platform consistency
        await Assert.That((await RandomFilePath.ReadAsync()).Trim().ReplaceLineEndings("\n")).
            IsEqualTo($$$"""
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
                             uses: actions/checkout@v7.0.1
                             with:
                               fetch-depth: 0
                               persist-credentials: false
                           - name: Setup .NET SDK
                             uses: actions/setup-dotnet@v6.0.0
                             with:
                               dotnet-version: 10.0.x
                           - name: Cache NuGet
                             uses: actions/cache@v6.1.0
                             with:
                               path: ~/.nuget/packages
                               key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
                               restore-keys: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
                           - name: Run Pipeline
                             run: dotnet run --project {{{RandomFilePath}}} -c Release --framework net10.0
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
                       """.Trim().ReplaceLineEndings("\n"));
    }
}
