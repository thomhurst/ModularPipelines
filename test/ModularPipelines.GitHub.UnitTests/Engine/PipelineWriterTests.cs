using ModularPipelines.Context;
using ModularPipelines.GitHub;
using ModularPipelines.Interfaces;
using ModularPipelines.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using YamlDotNet.RepresentationModel;

namespace ModularPipelines.GitHub.UnitTests.Engine;

public class PipelineWriterTests : TestBase
{
    public static readonly ModularPipelines.FileSystem.FilePath RandomFilePath = ModularPipelines.FileSystem.FilePath.GetNewTemporaryFilePath();

    private class DummyModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class ConfiguredGitHubYamlWriter(GitHubPipelineFileWriterOptions options) : GitHubPipelineFileWriter
    {
        internal override Task<GitHubPipelineFileWriterOptions> GetGitHubPipelineFileWriterOptions(
            IPipelineContext pipelineHookContext) => Task.FromResult(options);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ResolvesMaskSecretNamesThroughEnvironment(int count)
    {
        var names = new[] { "TOKEN", "NuGet__ApiKey", "_TOKEN2", "GITHUB_TOKEN" }[..count];
        var steps = await GenerateStepsAsync(names);
        var maskSteps = steps.Children.Cast<YamlMappingNode>()
            .Where(step => step["name"].ToString() == "Mask Secret Values").ToArray();

        await Assert.That(maskSteps.Length).IsEqualTo(count == 0 ? 0 : 1);
        if (count == 0)
        {
            return;
        }

        var maskStep = maskSteps.Single();
        var run = maskStep["run"].ToString();
        var environment = (YamlMappingNode) maskStep["env"];
        await Assert.That(maskStep["shell"].ToString()).IsEqualTo("bash");
        await Assert.That(environment.Children.Count).IsEqualTo(count);
        for (var index = 0; index < count; index++)
        {
            await Assert.That(environment[$"MODULARPIPELINES_MASK_VALUE_{index}"].ToString()).IsEqualTo($"${{{{ secrets.{names[index]} }}}}");
            await Assert.That(run).Contains($"\"${{MODULARPIPELINES_MASK_VALUE_{index}}}\"");
            await Assert.That(run).DoesNotContain(names[index]);
        }

        await Assert.That(run).Contains("printf '::add-mask::%s\\n' \"$value\"");
        await Assert.That(run).Contains("${value//'%'/'%25'}");
        await Assert.That(run).Contains("${value//$'\\r'/'%0D'}");
        await Assert.That(run).Contains("${value//$'\\n'/'%0A'}");
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("literal-secret-value")]
    [Arguments("${{ secrets.TOKEN }}")]
    [Arguments("TOKEN }} ${{ github.token")]
    [Arguments("TOKEN\nOTHER")]
    [Arguments("TOKEN\r")]
    [Arguments("TOKEN'")]
    [Arguments("TÖKEN")]
    [Arguments("1TOKEN")]
    public async Task RejectsInvalidMaskSecretNames(string? name)
    {
        var outputPath = ModularPipelines.FileSystem.FilePath.GetNewTemporaryFilePath();
        var writer = new ConfiguredGitHubYamlWriter(new GitHubPipelineFileWriterOptions
        {
            Name = "Test",
            TriggerCondition = new TriggerCondition(),
            OutputPath = outputPath,
            PipelineProjectPath = new ModularPipelines.FileSystem.FilePath("src/Pipeline"),
            SecretNamesToMask = ["VALID_TOKEN", name!],
        });

        var exception = (await Assert.ThrowsAsync<ArgumentException>(() => writer.WriteAsync(Mock.Of<IPipelineContext>())))!;

        await Assert.That(exception.ParamName).IsEqualTo(nameof(GitHubPipelineFileWriterOptions.SecretNamesToMask));
        if (!string.IsNullOrWhiteSpace(name))
        {
            await Assert.That(exception.Message).DoesNotContain(name);
        }

        await Assert.That(outputPath.Exists).IsFalse();
    }

    [Test]
    public async Task OmitsMaskStepWhenSecretNamesAreNull()
    {
        var steps = await GenerateStepsAsync(null);
        await Assert.That(steps.Children.Cast<YamlMappingNode>()
            .Any(step => step["name"].ToString() == "Mask Secret Values")).IsFalse();
    }

    [Test]
    public async Task QuotesPortablePipelineProjectPath()
    {
        var steps = await GenerateStepsAsync(null, @"src\My Pipeline's $HOME\Pipeline.csproj");
        var runStep = steps.Children.Cast<YamlMappingNode>().Single(step => step["name"].ToString() == "Run Pipeline");

        await Assert.That(runStep["shell"].ToString()).IsEqualTo("bash");
        await Assert.That(runStep["run"].ToString()).IsEqualTo(
            "dotnet run --project 'src/My Pipeline'\"'\"'s $HOME/Pipeline.csproj' -c Release");
    }

    private static async Task<YamlSequenceNode> GenerateStepsAsync(IEnumerable<string>? secretNames, string projectPath = "src/Pipeline")
    {
        var outputPath = ModularPipelines.FileSystem.FilePath.GetNewTemporaryFilePath();
        var writer = new ConfiguredGitHubYamlWriter(new GitHubPipelineFileWriterOptions
        {
            Name = "Test",
            TriggerCondition = new TriggerCondition(),
            OutputPath = outputPath,
            PipelineProjectPath = new ModularPipelines.FileSystem.FilePath(projectPath),
            SecretNamesToMask = secretNames,
        });
        var builder = TestPipelineBuilder.Create().AddModule<DummyModule>();
        builder.Services.AddSingleton<IBuildSystemPipelineFileWriter>(writer);
        await builder.RunAsync();

        var yaml = new YamlStream();
        yaml.Load(new StringReader(await outputPath.ReadAsync()));
        return (YamlSequenceNode) yaml.Documents[0].RootNode["jobs"]["pipeline"]["steps"];
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
                DotNetRunFramework = "net11.0",
                SecretNamesToMask =
                [
                    "DOTNET_FORMAT_PUSH_TOKEN", "NuGet__ApiKey",
                    "ADMIN_TOKEN",
                    "CODACY_APIKEY"
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
        var builder = TestPipelineBuilder.Create()
            .AddModule<DummyModule>();
        builder.Services.AddSingleton<IBuildSystemPipelineFileWriter>(new GitHubYamlWriter());
        await builder.RunAsync();
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
                             shell: bash
                             run: >-
                               for value in "${MODULARPIPELINES_MASK_VALUE_0}" "${MODULARPIPELINES_MASK_VALUE_1}" "${MODULARPIPELINES_MASK_VALUE_2}" "${MODULARPIPELINES_MASK_VALUE_3}"; do
                                 value="${value//'%'/'%25'}"
                                 value="${value//$'\r'/'%0D'}"
                                 value="${value//$'\n'/'%0A'}"
                                 printf '::add-mask::%s\n' "$value"
                               done
                             env:
                               MODULARPIPELINES_MASK_VALUE_0: ${{ secrets.DOTNET_FORMAT_PUSH_TOKEN }}
                               MODULARPIPELINES_MASK_VALUE_1: ${{ secrets.NuGet__ApiKey }}
                               MODULARPIPELINES_MASK_VALUE_2: ${{ secrets.ADMIN_TOKEN }}
                               MODULARPIPELINES_MASK_VALUE_3: ${{ secrets.CODACY_APIKEY }}
                           - name: Checkout
                             uses: actions/checkout@v7.0.1
                             with:
                               fetch-depth: 0
                               persist-credentials: false
                           - name: Setup .NET SDK
                             uses: actions/setup-dotnet@v6.0.0
                             with:
                               dotnet-version: 11.0.x
                           - name: Cache NuGet
                             uses: actions/cache@v6.1.0
                             with:
                               path: ~/.nuget/packages
                               key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
                               restore-keys: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
                           - name: Run Pipeline
                             shell: bash
                             run: dotnet run --project '{{{RandomFilePath.OriginalPath.Replace('\\', '/')}}}' -c Release --framework net11.0
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
