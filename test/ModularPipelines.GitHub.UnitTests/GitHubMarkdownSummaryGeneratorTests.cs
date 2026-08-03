using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.GitHub.UnitTests;

[TUnit.Core.NotInParallel]
public class GitHubMarkdownSummaryGeneratorTests
{
    private static int _skipConditionEvaluations;

    private sealed class DependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("dependency");
    }

    [ModularPipelines.Attributes.DependsOnAttribute<DependencyModule>]
    private sealed class TargetModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("target");
    }

    private sealed class SingleUseSkipConditionModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => Interlocked.Increment(ref _skipConditionEvaluations) == 1
                ? SkipDecision.DoNotSkip
                : throw new InvalidOperationException("Skip condition evaluated twice"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("executed");
    }

    private sealed class OversizedDependencyGraphRenderer : IPipelineSummaryDependencyGraphRenderer
    {
        public Task<string> RenderAsync(
            DependencyGraphFormat format,
            PipelineSummary pipelineSummary,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(new string('g', 1024 * 1024));
    }

    [Test]
    public async Task StepSummaryIncludesDependencyGraph()
    {
        var directory = Directory.CreateTempSubdirectory("modular-pipelines-github-summary-");
        var path = Path.Combine(directory.FullName, "summary.md");
        var previousPath = Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY");
        try
        {
            Environment.SetEnvironmentVariable("GITHUB_STEP_SUMMARY", path);
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<DependencyModule>();
            builder.AddModule<TargetModule>();

            await builder.ExecutePipelineAsync();

            var summary = await File.ReadAllTextAsync(path);
            using (Assert.Multiple())
            {
                await Assert.That(summary).Contains("### Dependency Graph");
                await Assert.That(summary).Contains("flowchart TD");
                await Assert.That(summary).Contains("DependencyModule");
                await Assert.That(summary).Contains("TargetModule");
                await Assert.That(summary).Contains(" --> ");
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("GITHUB_STEP_SUMMARY", previousPath);
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task StepSummaryDoesNotReevaluateSkipConditions()
    {
        var directory = Directory.CreateTempSubdirectory("modular-pipelines-github-summary-");
        var path = Path.Combine(directory.FullName, "summary.md");
        var previousPath = Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY");
        try
        {
            _skipConditionEvaluations = 0;
            Environment.SetEnvironmentVariable("GITHUB_STEP_SUMMARY", path);
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<SingleUseSkipConditionModule>();

            await builder.ExecutePipelineAsync();

            var summary = await File.ReadAllTextAsync(path);
            using (Assert.Multiple())
            {
                await Assert.That(_skipConditionEvaluations).IsEqualTo(1);
                await Assert.That(summary).Contains(nameof(SingleUseSkipConditionModule));
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("GITHUB_STEP_SUMMARY", previousPath);
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task OversizedDependencyGraphDoesNotSuppressExistingSummary()
    {
        var directory = Directory.CreateTempSubdirectory("modular-pipelines-github-summary-");
        var path = Path.Combine(directory.FullName, "summary.md");
        var previousPath = Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY");
        try
        {
            Environment.SetEnvironmentVariable("GITHUB_STEP_SUMMARY", path);
            using var builder = Pipeline.CreateBuilder();
            builder.Services.AddSingleton<IPipelineSummaryDependencyGraphRenderer, OversizedDependencyGraphRenderer>();
            builder.AddModule<DependencyModule>();

            await builder.ExecutePipelineAsync();

            var summary = await File.ReadAllTextAsync(path);
            using (Assert.Multiple())
            {
                await Assert.That(summary).Contains("Run Summary");
                await Assert.That(summary).Contains(nameof(DependencyModule));
                await Assert.That(summary).DoesNotContain("### Dependency Graph");
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("GITHUB_STEP_SUMMARY", previousPath);
            directory.Delete(recursive: true);
        }
    }
}
