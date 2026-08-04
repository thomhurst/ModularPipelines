using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Configuration;
using ModularPipelines.Console;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Enums;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;
using ModularPipelines.UnitTests.Attributes;
using ModularPipelines.UnitTests.Logging;
using ModularPipelines.Validation;
using Moq;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel(nameof(RunReportTests))]
public class RunReportTests
{
    private sealed class CommandModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var result = await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("report-command"),
                cancellationToken: cancellationToken);
            return result.StandardOutput;
        }
    }

    private sealed class FailingModule : Module<string>
    {
        private const string RegisteredSecret = "report-secret-value";
        private readonly ISecretRegistry _secretRegistry;

        public FailingModule(ISecretRegistry secretRegistry)
        {
            _secretRegistry = secretRegistry;
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            _secretRegistry.AddSecret(RegisteredSecret);
            throw new InvalidOperationException($"report failure {RegisteredSecret}");
        }
    }

    private sealed class SkippedModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("report skip reason"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Skipped module must not execute");
    }

    private sealed class SuccessfulModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("success");
    }

    private sealed class OutputExcerptModule(ISecretRegistry secretRegistry) : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            secretRegistry.AddSecret("module-output-secret");
            System.Console.WriteLine("stdout module-output-secret");
            System.Console.Error.WriteLine("stderr module-output-secret");
            return Task.FromResult<string?>("success");
        }
    }

    private sealed class GenericModule<T> : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>(typeof(T).FullName);
    }

    private sealed class ThrowingEndHook : IPipelineGlobalHooks
    {
        private const string RegisteredSecret = "hook-secret-value";
        private readonly ISecretRegistry _secretRegistry;

        public ThrowingEndHook(ISecretRegistry secretRegistry)
        {
            _secretRegistry = secretRegistry;
        }

        public Task OnPipelineEndAsync(
            IPipelineContext context,
            PipelineSummary pipelineSummary)
        {
            _secretRegistry.AddSecret(RegisteredSecret);
            throw new InvalidOperationException($"Pipeline end hook failed {RegisteredSecret}");
        }
    }

    private sealed class MixedFailureEndHook : IPipelineGlobalHooks
    {
        public Task OnPipelineEndAsync(
            IPipelineContext context,
            PipelineSummary pipelineSummary)
        {
            var moduleException = pipelineSummary.Results
                .Select(result => result.ExceptionOrDefault)
                .OfType<Exception>()
                .Single();
            throw new AggregateException(
                moduleException,
                new InvalidOperationException("Pipeline end hook failed"));
        }
    }

    private sealed class WrappedFailureEndHook : IPipelineGlobalHooks
    {
        public Task OnPipelineEndAsync(
            IPipelineContext context,
            PipelineSummary pipelineSummary)
        {
            var moduleException = pipelineSummary.Results
                .Select(result => result.ExceptionOrDefault)
                .OfType<Exception>()
                .Single();
            throw new InvalidOperationException("Pipeline end hook failed", moduleException);
        }
    }

    private sealed class DelayedEndHook : IPipelineGlobalHooks
    {
        public static DateTimeOffset CompletedAt { get; private set; }

        public async Task OnPipelineEndAsync(
            IPipelineContext context,
            PipelineSummary pipelineSummary)
        {
            await Task.Delay(100);
            CompletedAt = DateTimeOffset.UtcNow;
        }
    }

    public class DuplicateModuleBase : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(null);
    }

    private sealed class DuplicateAssemblyLoadContext(string name = "duplicate-report-context")
        : AssemblyLoadContext(name, isCollectible: true)
    {
        protected override Assembly? Load(AssemblyName assemblyName) => null;
    }

    private sealed class SuccessfulCommandInterceptor : ICommandInterceptor
    {
        public ValueTask<CommandResult?> InterceptAsync(
            CommandInvocation invocation,
            CancellationToken cancellationToken = default) =>
            ValueTask.FromResult<CommandResult?>(CommandResult.Ok("intercepted"));
    }

    private sealed class KnownBuildSystemDetector : IBuildSystemDetector
    {
        public BuildSystem GetCurrentBuildSystem() => BuildSystem.GitHubActions;
    }

    private sealed class PassthroughSecretObfuscator : ISecretObfuscator
    {
        public string Obfuscate(string? input, object? optionsObject) => input ?? string.Empty;
    }

    private sealed class ThrowingSecretObfuscator : ISecretObfuscator
    {
        public string Obfuscate(string? input, object? optionsObject) =>
            throw new InvalidOperationException("Obfuscation failed");
    }

    private sealed class RedactingSecretObfuscator : ISecretObfuscator
    {
        public string Obfuscate(string? input, object? optionsObject) =>
            input?.Replace("secret", "**********", StringComparison.Ordinal) ?? string.Empty;
    }

    public sealed class StaticRunReportEnricher : IRunReportEnricher
    {
        public ValueTask EnrichAsync(
            RunReportEnrichmentContext context,
            CancellationToken cancellationToken)
        {
            context.GitSha = "secret-sha";
            context.GitBranch = "secret-branch";
            context.CiRunUrl = "https://ci.example/secret-run";
            return ValueTask.CompletedTask;
        }
    }

    private sealed class NeverCompletingRunReportEnricher : IRunReportEnricher
    {
        private readonly TaskCompletionSource _completion = new(
            TaskCreationOptions.RunContinuationsAsynchronously);

        public ValueTask EnrichAsync(
            RunReportEnrichmentContext context,
            CancellationToken cancellationToken) =>
            new(_completion.Task);
    }

    private sealed class CommandCountingRunReportEnricher(
        ICommandExecutionCounter commandExecutionCounter) : IRunReportEnricher
    {
        public ValueTask EnrichAsync(
            RunReportEnrichmentContext context,
            CancellationToken cancellationToken)
        {
            commandExecutionCounter.Record(moduleType: null);
            return ValueTask.CompletedTask;
        }
    }

    [Test]
    public async Task WriteRunReportPersistsSchemaHistoryDeltasAndCommandCounts()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "artifacts", "run-report.json");
        var historyPath = Path.Combine(directory, "history");

        try
        {
            var firstSummary = await RunPipelineAsync(reportPath, historyPath);
            var firstReport = RunReportJsonSerializer.Deserialize(
                await File.ReadAllTextAsync(reportPath));

            using (Assert.Multiple())
            {
                await Assert.That(firstSummary.RunReport).IsNotNull();
                await Assert.That(firstReport).IsNotNull();
                await Assert.That(firstReport!.SchemaVersion).IsEqualTo(PipelineRunReport.CurrentSchemaVersion);
                await Assert.That(Guid.TryParseExact(firstReport.RunId, "N", out _)).IsTrue();
                await Assert.That(firstReport.PipelineIdentity).IsNotNullOrWhiteSpace();
                await Assert.That(firstReport.Correlation).IsNotNull();
                await Assert.That(firstReport.Correlation!.Hostname).IsNotNullOrWhiteSpace();
                await Assert.That(firstReport.Status).IsEqualTo(Status.Failed);
                await Assert.That(firstReport.CommandCount).IsEqualTo(1);
                await Assert.That(firstReport.Modules.Single(module => module.ModuleTypeName == ModuleTypeIdentifier.Get(typeof(CommandModule)))
                    .CommandCount).IsEqualTo(1);
                await Assert.That(firstReport.Modules.Single(module => module.ModuleTypeName == ModuleTypeIdentifier.Get(typeof(FailingModule)))
                    .Exception!.Message).IsEqualTo("report failure **********");
                await Assert.That(firstReport.Modules.Single(module => module.ModuleTypeName == ModuleTypeIdentifier.Get(typeof(SkippedModule)))
                    .SkipReason).IsEqualTo("report skip reason");
            }

            var secondSummary = await RunPipelineAsync(reportPath, historyPath);
            var secondReport = RunReportJsonSerializer.Deserialize(
                await File.ReadAllTextAsync(reportPath));

            using (Assert.Multiple())
            {
                await Assert.That(secondSummary.RunReport!.PreviousTotalDuration).IsNull();
                await Assert.That(secondReport!.TotalDurationDelta).IsNull();
                await Assert.That(secondReport.Modules
                        .Single(module => module.Status == Status.Successful)
                        .PreviousDuration)
                    .IsNotNull();
                await Assert.That(secondReport.Modules
                        .Where(module => module.Status != Status.Successful)
                        .All(module => module.PreviousDuration is null))
                    .IsTrue();
                await Assert.That(Directory.GetFiles(historyPath, "*.json")).Count().IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task RunHistoryPersistsAndCalculatesDeltasWithoutReportWriting()
    {
        var historyPath = CreateTemporaryDirectory();

        try
        {
            var firstSummary = await RunPipelineWithoutReportAsync(historyPath);
            var secondSummary = await RunPipelineWithoutReportAsync(historyPath);

            using (Assert.Multiple())
            {
                await Assert.That(firstSummary.RunReport!.PreviousTotalDuration).IsNull();
                await Assert.That(secondSummary.RunReport!.PreviousTotalDuration).IsNotNull();
                await Assert.That(Directory.GetFiles(historyPath, "*.json")).Count().IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(historyPath, recursive: true);
        }
    }

    [Test]
    public async Task RunReportSuppressesHistoryForCurrentIdentifierCollisions()
    {
        var firstType = CreateDynamicModuleType("RunReportAssembly", new Version(1, 0));
        var secondType = CreateDynamicModuleType("RunReportAssembly", new Version(2, 0));
        var firstModule = (IModule) Activator.CreateInstance(firstType)!;
        var secondModule = (IModule) Activator.CreateInstance(secondType)!;
        var firstTypeName = ModuleTypeIdentifier.Get(firstType);
        var secondTypeName = ModuleTypeIdentifier.Get(secondType);
        var start = new DateTimeOffset(2026, 8, 2, 12, 0, 0, TimeSpan.Zero);
        var summary = new PipelineSummary(
            [firstModule, secondModule],
            [
                CreateResult(firstModule, start, TimeSpan.FromSeconds(1)),
                CreateResult(secondModule, start, TimeSpan.FromSeconds(2)),
            ],
            TimeSpan.FromSeconds(2),
            start,
            start.AddSeconds(2),
            moduleTimelines:
            [
                CreateTimeline(firstType, start, TimeSpan.FromSeconds(1)),
                CreateTimeline(secondType, start, TimeSpan.FromSeconds(2)),
            ]);
        var previousReport = new PipelineRunReport
        {
            Modules =
            [
                new ModuleRunReport
                {
                    ModuleName = firstType.Name,
                    ModuleTypeName = firstTypeName,
                    Status = Status.Successful,
                    Duration = TimeSpan.FromSeconds(10),
                    DurationMeasured = true,
                    Start = start,
                    End = start.AddSeconds(10),
                },
                new ModuleRunReport
                {
                    ModuleName = secondType.Name,
                    ModuleTypeName = secondTypeName,
                    Status = Status.Successful,
                    Duration = TimeSpan.FromSeconds(20),
                    DurationMeasured = true,
                    Start = start,
                    End = start.AddSeconds(20),
                },
            ],
        };
        var report = new PipelineRunReportFactory(
                Mock.Of<ICommandExecutionCounter>(),
                new PassthroughSecretObfuscator())
            .Create(summary, previousReport, "duplicate-types");
        var firstModuleReport = report.Modules[0];
        var secondModuleReport = report.Modules[1];

        using (Assert.Multiple())
        {
            await Assert.That(firstType.FullName).IsEqualTo(secondType.FullName);
            await Assert.That(firstType.Assembly.GetName().Name).IsEqualTo(secondType.Assembly.GetName().Name);
            await Assert.That(firstTypeName).IsEqualTo(secondTypeName);
            await Assert.That(ModuleTypeIdentifier.GetRuntime(firstType))
                .IsNotEqualTo(ModuleTypeIdentifier.GetRuntime(secondType));
            await Assert.That(firstTypeName).DoesNotContain("Version=");
            await Assert.That(firstTypeName).DoesNotContain("RuntimeAssembly=");
            await Assert.That(firstModuleReport.Duration).IsEqualTo(TimeSpan.FromSeconds(1));
            await Assert.That(firstModuleReport.PreviousDuration).IsNull();
            await Assert.That(secondModuleReport.Duration).IsEqualTo(TimeSpan.FromSeconds(2));
            await Assert.That(secondModuleReport.PreviousDuration).IsNull();
            await Assert.That(SpectreResultsPrinter.CreateModulesTable(summary with { RunReport = report }).Rows)
                .Count().IsEqualTo(3);
        }
    }

    [Test]
    public async Task RunReportSuppressesAmbiguousPreviousIdentifierHistory()
    {
        var firstType = CreateDynamicModuleType("RunReportAssembly", new Version(1, 0));
        var secondType = CreateDynamicModuleType("RunReportAssembly", new Version(2, 0));
        var module = (IModule) Activator.CreateInstance(firstType)!;
        var typeName = ModuleTypeIdentifier.Get(firstType);
        var start = new DateTimeOffset(2026, 8, 2, 12, 0, 0, TimeSpan.Zero);
        var summary = new PipelineSummary(
            [module],
            [CreateResult(module, start, TimeSpan.FromSeconds(1))],
            TimeSpan.FromSeconds(1),
            start,
            start.AddSeconds(1),
            moduleTimelines: [CreateTimeline(firstType, start, TimeSpan.FromSeconds(1))]);
        var previousReport = new PipelineRunReport
        {
            Modules =
            [
                CreatePreviousModuleReport(firstType, typeName, start, TimeSpan.FromSeconds(10)),
                CreatePreviousModuleReport(secondType, typeName, start, TimeSpan.FromSeconds(20)),
            ],
        };

        var report = new PipelineRunReportFactory(
                Mock.Of<ICommandExecutionCounter>(),
                new PassthroughSecretObfuscator())
            .Create(summary, previousReport, "ambiguous-previous-type");

        await Assert.That(report.Modules.Single().PreviousDuration).IsNull();
    }

    [Test]
    public async Task ModuleTypeIdentifierNormalizesGenericArgumentAssemblies()
    {
        var firstArgument = CreateDynamicType("PayloadAssembly", new Version(1, 0), "Payload");
        var secondArgument = CreateDynamicType("PayloadAssembly", new Version(2, 0), "Payload");
        var firstModuleType = typeof(GenericModule<>).MakeGenericType(firstArgument);
        var secondModuleType = typeof(GenericModule<>).MakeGenericType(secondArgument);

        var firstIdentifier = ModuleTypeIdentifier.Get(firstModuleType);
        var secondIdentifier = ModuleTypeIdentifier.Get(secondModuleType);

        using (Assert.Multiple())
        {
            await Assert.That(firstIdentifier).IsEqualTo(secondIdentifier);
            await Assert.That(firstIdentifier).Contains("Payload, PayloadAssembly");
            await Assert.That(firstIdentifier).DoesNotContain("Version=");
            await Assert.That(firstIdentifier).DoesNotContain("Culture=");
            await Assert.That(firstIdentifier).DoesNotContain("PublicKeyToken=");
            await Assert.That(ModuleTypeIdentifier.GetRuntime(firstModuleType))
                .IsNotEqualTo(ModuleTypeIdentifier.GetRuntime(secondModuleType));
        }
    }

    [Test]
    public async Task RuntimeIdentifierDistinguishesGenericArgumentsAcrossLoadContexts()
    {
        var firstContext = new DuplicateAssemblyLoadContext();
        var secondContext = new DuplicateAssemblyLoadContext();
        try
        {
            var assemblyBytes = await File.ReadAllBytesAsync(typeof(RunReportTests).Assembly.Location);
            var firstAssembly = firstContext.LoadFromStream(new MemoryStream(assemblyBytes));
            var secondAssembly = secondContext.LoadFromStream(new MemoryStream(assemblyBytes));
            var argumentTypeName = typeof(DuplicateModuleBase).FullName!;
            var firstArgument = firstAssembly.GetType(argumentTypeName, throwOnError: true)!;
            var secondArgument = secondAssembly.GetType(argumentTypeName, throwOnError: true)!;
            var firstModuleType = typeof(GenericModule<>).MakeGenericType(firstArgument);
            var secondModuleType = typeof(GenericModule<>).MakeGenericType(secondArgument);

            using (Assert.Multiple())
            {
                await Assert.That(ModuleTypeIdentifier.Get(firstModuleType))
                    .IsEqualTo(ModuleTypeIdentifier.Get(secondModuleType));
                await Assert.That(ModuleTypeIdentifier.GetRuntime(firstModuleType))
                    .IsNotEqualTo(ModuleTypeIdentifier.GetRuntime(secondModuleType));
            }
        }
        finally
        {
            firstContext.Unload();
            secondContext.Unload();
        }
    }

    [Test]
    public async Task RunReportPreservesEveryAggregateExceptionBranch()
    {
        var factory = new PipelineRunReportFactory(
            Mock.Of<ICommandExecutionCounter>(),
            new PassthroughSecretObfuscator());

        var details = factory.CreateExceptionDetails(new AggregateException(
            new InvalidOperationException("first failure"),
            new ArgumentException("second failure")));

        using (Assert.Multiple())
        {
            await Assert.That(details!.InnerExceptions).Count().IsEqualTo(2);
            await Assert.That(details.InnerExceptions.Select(static exception => exception.Message))
                .IsEquivalentTo(["first failure", "second failure"]);
            await Assert.That(details.InnerException!.Message).IsEqualTo("first failure");
        }
    }

    [Test]
    public async Task RunReportUsesTimelineIntervalForMeasuredDuration()
    {
        var module = new SuccessfulModule();
        var start = new DateTimeOffset(2026, 8, 3, 12, 0, 0, TimeSpan.Zero);
        var timeline = CreateTimeline(typeof(SuccessfulModule), start, TimeSpan.FromSeconds(4)) with
        {
            ExecutionDuration = TimeSpan.FromSeconds(1),
        };
        var summary = new PipelineSummary(
            [module],
            [CreateResult(module, start, TimeSpan.FromSeconds(1))],
            TimeSpan.FromSeconds(4),
            start,
            start.AddSeconds(4),
            moduleTimelines: [timeline]);

        var moduleReport = new PipelineRunReportFactory(
                Mock.Of<ICommandExecutionCounter>(),
                new PassthroughSecretObfuscator())
            .Create(summary, null, "timeline-interval")
            .Modules.Single();

        await Assert.That(moduleReport.Duration).IsEqualTo(moduleReport.End - moduleReport.Start);
        await Assert.That(moduleReport.Duration).IsEqualTo(TimeSpan.FromSeconds(4));
    }

    [Test]
    public async Task RunReportSkipsDurationDeltaForUnmeasuredModules()
    {
        var module = new SuccessfulModule();
        var start = new DateTimeOffset(2026, 8, 3, 12, 0, 0, TimeSpan.Zero);
        var previousReport = new PipelineRunReport
        {
            Modules =
            [
                new ModuleRunReport
                {
                    ModuleName = nameof(SuccessfulModule),
                    ModuleTypeName = ModuleTypeIdentifier.Get(typeof(SuccessfulModule)),
                    Status = Status.Successful,
                    Duration = TimeSpan.FromSeconds(10),
                    DurationMeasured = true,
                    Start = start,
                    End = start.AddSeconds(10),
                },
            ],
        };
        var factory = new PipelineRunReportFactory(
            Mock.Of<ICommandExecutionCounter>(),
            new PassthroughSecretObfuscator());
        var failedInitializationReport = factory.Create(
            new PipelineSummary(
                [module],
                [],
                TimeSpan.FromSeconds(1),
                start,
                start.AddSeconds(1)),
            previousReport,
            "failed-initialization");
        var successfulReport = factory.Create(
            new PipelineSummary(
                [module],
                [CreateResult(module, start, TimeSpan.FromSeconds(1))],
                TimeSpan.FromSeconds(1),
                start,
                start.AddSeconds(1)),
            failedInitializationReport,
            "successful-retry");

        using (Assert.Multiple())
        {
            await Assert.That(failedInitializationReport.Modules.Single().PreviousDuration).IsNull();
            await Assert.That(failedInitializationReport.Modules.Single().DurationDelta).IsNull();
            await Assert.That(successfulReport.Modules.Single().PreviousDuration).IsNull();
            await Assert.That(successfulReport.Modules.Single().DurationDelta).IsNull();
        }
    }

    [Test]
    [Arguments(Status.Skipped)]
    [Arguments(Status.UsedHistory)]
    [Arguments(Status.CachedResult)]
    public async Task RunReportDoesNotCompareNonExecutedModuleDurations(Status status)
    {
        var module = new SkippedModule();
        var start = new DateTimeOffset(2026, 8, 3, 12, 0, 0, TimeSpan.Zero);
        var previousReport = new PipelineRunReport
        {
            Modules =
            [
                new ModuleRunReport
                {
                    ModuleName = nameof(SkippedModule),
                    ModuleTypeName = ModuleTypeIdentifier.Get(typeof(SkippedModule)),
                    Status = Status.Successful,
                    Duration = TimeSpan.FromSeconds(10),
                    DurationMeasured = true,
                    Start = start,
                    End = start.AddSeconds(10),
                },
            ],
        };
        var factory = new PipelineRunReportFactory(
            Mock.Of<ICommandExecutionCounter>(),
            new PassthroughSecretObfuscator());
        var nonExecutedReport = factory.Create(
            new PipelineSummary(
                [module],
                [status switch
                {
                    Status.Skipped => CreateSkippedResult(module),
                    Status.UsedHistory => CreateHistoricalResult(module, start),
                    Status.CachedResult => CreateCachedResult(module, start),
                    _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
                }],
                TimeSpan.Zero,
                start,
                start),
            previousReport,
            "non-executed-run");
        var successfulReport = factory.Create(
            new PipelineSummary(
                [module],
                [CreateResult(module, start, TimeSpan.FromSeconds(1))],
                TimeSpan.FromSeconds(1),
                start,
                start.AddSeconds(1),
                moduleTimelines:
                [
                    CreateTimeline(typeof(SkippedModule), start, TimeSpan.FromSeconds(1)),
                ]),
            nonExecutedReport,
            "successful-run");

        using (Assert.Multiple())
        {
            await Assert.That(nonExecutedReport.Modules.Single().Status).IsEqualTo(status);
            await Assert.That(nonExecutedReport.Modules.Single().DurationMeasured).IsFalse();
            await Assert.That(nonExecutedReport.Modules.Single().Start).IsNull();
            await Assert.That(nonExecutedReport.Modules.Single().End).IsNull();
            await Assert.That(nonExecutedReport.Modules.Single().PreviousDuration).IsNull();
            await Assert.That(nonExecutedReport.Modules.Single().DurationDelta).IsNull();
            await Assert.That(successfulReport.Modules.Single().PreviousDuration).IsNull();
            await Assert.That(successfulReport.Modules.Single().DurationDelta).IsNull();
        }
    }

    [Test]
    [Arguments(Status.Failed)]
    [Arguments(Status.TimedOut)]
    public async Task RunReportDoesNotCompareMeasuredUnsuccessfulCurrentDurations(Status status)
    {
        var previousReport = CreateMeasuredRunReport(Status.Successful, TimeSpan.FromSeconds(10));
        var report = CreateMeasuredRunReport(status, TimeSpan.FromSeconds(1), previousReport);

        using (Assert.Multiple())
        {
            await Assert.That(report.PreviousTotalDuration).IsNull();
            await Assert.That(report.TotalDurationDelta).IsNull();
            await Assert.That(report.Modules.Single().DurationMeasured).IsTrue();
            await Assert.That(report.Modules.Single().PreviousDuration).IsNull();
            await Assert.That(report.Modules.Single().DurationDelta).IsNull();
        }
    }

    [Test]
    [Arguments(Status.Failed)]
    [Arguments(Status.TimedOut)]
    public async Task RunReportDoesNotCompareAgainstMeasuredUnsuccessfulPreviousDurations(Status status)
    {
        var previousReport = CreateMeasuredRunReport(status, TimeSpan.FromSeconds(1));
        var report = CreateMeasuredRunReport(Status.Successful, TimeSpan.FromSeconds(10), previousReport);

        using (Assert.Multiple())
        {
            await Assert.That(report.Status).IsEqualTo(Status.Successful);
            await Assert.That(report.PreviousTotalDuration).IsNull();
            await Assert.That(report.TotalDurationDelta).IsNull();
            await Assert.That(report.Modules.Single().PreviousDuration).IsNull();
            await Assert.That(report.Modules.Single().DurationDelta).IsNull();
        }
    }

    [Test]
    public async Task RunReportDoesNotReusePartialResultForDuplicateModuleName()
    {
        var firstType = CreateDynamicModuleType("PartialRunReportAssembly", new Version(1, 0));
        var secondType = CreateDynamicModuleType("PartialRunReportAssembly", new Version(2, 0));
        var firstModule = (IModule) Activator.CreateInstance(firstType)!;
        var secondModule = (IModule) Activator.CreateInstance(secondType)!;
        var start = new DateTimeOffset(2026, 8, 3, 12, 0, 0, TimeSpan.Zero);
        var summary = new PipelineSummary(
            [firstModule, secondModule],
            [CreateResult(firstModule, start, TimeSpan.FromSeconds(1))],
            TimeSpan.FromSeconds(1),
            start,
            start.AddSeconds(1));

        var report = new PipelineRunReportFactory(
                Mock.Of<ICommandExecutionCounter>(),
                new PassthroughSecretObfuscator())
            .Create(summary, null, "partial-duplicate-types");
        var firstModuleReport = report.Modules[0];
        var secondModuleReport = report.Modules[1];

        using (Assert.Multiple())
        {
            await Assert.That(firstType.Name).IsEqualTo(secondType.Name);
            await Assert.That(firstModuleReport.Status)
                .IsEqualTo(Status.Successful);
            await Assert.That(secondModuleReport.Status)
                .IsEqualTo(Status.Unknown);
            await Assert.That(secondModuleReport.Duration)
                .IsEqualTo(TimeSpan.Zero);
        }
    }

    [Test]
    public async Task RunReportUsesStablePersistedIdentityAcrossLoadContexts()
    {
        var firstContext = new DuplicateAssemblyLoadContext();
        var secondContext = new DuplicateAssemblyLoadContext();
        try
        {
            var assemblyBytes = await File.ReadAllBytesAsync(typeof(RunReportTests).Assembly.Location);
            var firstAssembly = firstContext.LoadFromStream(new MemoryStream(assemblyBytes));
            var secondAssembly = secondContext.LoadFromStream(new MemoryStream(assemblyBytes));
            var moduleTypeName = typeof(DuplicateModuleBase).FullName!;
            var firstType = firstAssembly.GetType(moduleTypeName, throwOnError: true)!;
            var secondType = secondAssembly.GetType(moduleTypeName, throwOnError: true)!;
            var firstModule = (IModule) Activator.CreateInstance(firstType)!;
            var secondModule = (IModule) Activator.CreateInstance(secondType)!;
            var firstIdentifier = ModuleTypeIdentifier.Get(firstType);
            var secondIdentifier = ModuleTypeIdentifier.Get(secondType);
            var start = new DateTimeOffset(2026, 8, 2, 12, 0, 0, TimeSpan.Zero);
            var summary = new PipelineSummary(
                [firstModule, secondModule],
                [
                    CreateResult(firstModule, start, TimeSpan.FromSeconds(1)),
                    CreateResult(secondModule, start, TimeSpan.FromSeconds(2)),
                ],
                TimeSpan.FromSeconds(2),
                start,
                start.AddSeconds(2),
                moduleTimelines:
                [
                    CreateTimeline(firstType, start, TimeSpan.FromSeconds(1)),
                    CreateTimeline(secondType, start, TimeSpan.FromSeconds(2)),
                ]);
            var report = new PipelineRunReportFactory(
                    Mock.Of<ICommandExecutionCounter>(),
                    new PassthroughSecretObfuscator())
                .Create(summary, null, "load-context-types");
            var firstModuleReport = report.Modules[0];
            var secondModuleReport = report.Modules[1];

            using (Assert.Multiple())
            {
                await Assert.That(firstType.FullName).IsEqualTo(secondType.FullName);
                await Assert.That(firstType.Assembly.FullName).IsEqualTo(secondType.Assembly.FullName);
                await Assert.That(firstIdentifier).IsEqualTo(secondIdentifier);
                await Assert.That(ModuleTypeIdentifier.GetRuntime(firstType))
                    .IsNotEqualTo(ModuleTypeIdentifier.GetRuntime(secondType));
                await Assert.That(firstIdentifier).DoesNotContain("LoadContext=");
                await Assert.That(firstModuleReport.Duration).IsEqualTo(TimeSpan.FromSeconds(1));
                await Assert.That(secondModuleReport.Duration).IsEqualTo(TimeSpan.FromSeconds(2));
                await Assert.That(SpectreResultsPrinter.CreateModulesTable(summary with { RunReport = report }).Rows)
                    .Count().IsEqualTo(3);
            }
        }
        finally
        {
            firstContext.Unload();
            secondContext.Unload();
        }
    }

    [Test]
    public async Task ModuleTypeIdentifierRetainsIdentityWhileUnloadedContextIsReferenced()
    {
        var contextName = $"reload-report-context-{Guid.NewGuid():N}";
        var assemblyBytes = await File.ReadAllBytesAsync(typeof(RunReportTests).Assembly.Location);
        var firstContext = new DuplicateAssemblyLoadContext(contextName);
        var firstAssembly = firstContext.LoadFromStream(new MemoryStream(assemblyBytes));
        var moduleTypeName = typeof(DuplicateModuleBase).FullName!;
        var firstType = firstAssembly.GetType(moduleTypeName, throwOnError: true)!;
        var firstIdentifier = ModuleTypeIdentifier.Get(firstType);

        firstContext.Unload();

        var secondContext = new DuplicateAssemblyLoadContext(contextName);
        try
        {
            var secondAssembly = secondContext.LoadFromStream(new MemoryStream(assemblyBytes));
            var secondType = secondAssembly.GetType(moduleTypeName, throwOnError: true)!;
            var secondIdentifier = ModuleTypeIdentifier.Get(secondType);

            using (Assert.Multiple())
            {
                await Assert.That(secondIdentifier).IsEqualTo(firstIdentifier);
                await Assert.That(ModuleTypeIdentifier.GetRuntime(secondType))
                    .IsNotEqualTo(ModuleTypeIdentifier.GetRuntime(firstType));
                await Assert.That(ModuleTypeIdentifier.Get(firstType)).IsEqualTo(firstIdentifier);
            }
        }
        finally
        {
            secondContext.Unload();
        }
    }

    [Test]
    public async Task ModuleTypeIdentifierScopesLoadContextOrdinalsToMatchingTypes()
    {
        var contextName = $"scoped-report-context-{Guid.NewGuid():N}";
        var firstContext = new DuplicateAssemblyLoadContext(contextName);
        var secondContext = new DuplicateAssemblyLoadContext(contextName);
        try
        {
            var assemblyBytes = await File.ReadAllBytesAsync(typeof(RunReportTests).Assembly.Location);
            var firstAssembly = firstContext.LoadFromStream(new MemoryStream(assemblyBytes));
            var secondAssembly = secondContext.LoadFromStream(new MemoryStream(assemblyBytes));
            var firstIdentifier = ModuleTypeIdentifier.GetRuntime(
                firstAssembly.GetType(typeof(DuplicateModuleBase).FullName!, throwOnError: true)!);
            var secondIdentifier = ModuleTypeIdentifier.GetRuntime(
                secondAssembly.GetType(typeof(SuccessfulModule).FullName!, throwOnError: true)!);

            using (Assert.Multiple())
            {
                await Assert.That(firstIdentifier).EndsWith($"RuntimeLoadContext={contextName}#1");
                await Assert.That(secondIdentifier).EndsWith($"RuntimeLoadContext={contextName}#1");
            }
        }
        finally
        {
            firstContext.Unload();
            secondContext.Unload();
        }
    }

    [Test]
    public async Task ModuleTypeIdentifierIsStableWhenUnrelatedAssemblyLoads()
    {
        var assemblyName = $"StableRunReportAssembly{Guid.NewGuid():N}";
        var moduleType = CreateDynamicModuleType(
            assemblyName,
            new Version(1, 0),
            "Stable.ReportModule");
        var identifierBeforeLoad = ModuleTypeIdentifier.Get(moduleType);

        _ = CreateDynamicModuleType(
            assemblyName,
            new Version(2, 0),
            "Unrelated.ReportModule");
        var identifierAfterLoad = ModuleTypeIdentifier.Get(moduleType);

        using (Assert.Multiple())
        {
            await Assert.That(identifierAfterLoad).IsEqualTo(identifierBeforeLoad);
            await Assert.That(identifierAfterLoad).DoesNotContain("RuntimeAssembly=");
        }
    }

    [Test]
    public async Task FileSystemHistoryStorePrunesToConfiguredRetention()
    {
        var directory = CreateTemporaryDirectory();
        var options = OptionsFactory.Create(new PipelineOptions
        {
            RunReport = new RunReportOptions
            {
                HistoryDirectory = directory,
                HistoryRetention = 2,
            },
        });
        var store = new FileSystemRunHistoryStore(
            options,
            NullLogger<FileSystemRunHistoryStore>.Instance);

        try
        {
            var unrelatedPath = Path.Combine(directory, "unrelated-report.json");
            await File.WriteAllTextAsync(unrelatedPath, "{}");
            for (var index = 0; index < 3; index++)
            {
                await store.SaveAsync(new PipelineRunReport
                {
                    PipelineIdentity = "pipeline-a",
                    End = new DateTimeOffset(2026, 8, 2, 12, 0, index, TimeSpan.Zero),
                });
            }

            var latest = await store.GetLatestAsync("pipeline-a");

            using (Assert.Multiple())
            {
                await Assert.That(Directory.GetFiles(directory, "modularpipelines-run-*.json"))
                    .Count().IsEqualTo(2);
                await Assert.That(File.Exists(unrelatedPath)).IsTrue();
                await Assert.That(latest!.End.Second).IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task FileSystemHistoryStorePrunesOnlyStaleOwnedTemporaryFiles()
    {
        var directory = CreateTemporaryDirectory();
        var store = CreateHistoryStore(directory, historyRetention: 1);
        var staleTemporaryFile = Path.Combine(directory, ".modularpipelines-stale.tmp");
        var recentTemporaryFile = Path.Combine(directory, ".modularpipelines-recent.tmp");
        var unrelatedTemporaryFile = Path.Combine(directory, "unrelated.tmp");

        try
        {
            await File.WriteAllTextAsync(staleTemporaryFile, "stale");
            File.SetLastWriteTimeUtc(staleTemporaryFile, DateTime.UtcNow.AddDays(-2));
            await File.WriteAllTextAsync(recentTemporaryFile, "recent");
            await File.WriteAllTextAsync(unrelatedTemporaryFile, "unrelated");

            await store.SaveAsync(new PipelineRunReport
            {
                PipelineIdentity = "pipeline-a",
                End = DateTimeOffset.UtcNow,
            });

            using (Assert.Multiple())
            {
                await Assert.That(File.Exists(staleTemporaryFile)).IsFalse();
                await Assert.That(File.Exists(recentTemporaryFile)).IsTrue();
                await Assert.That(File.Exists(unrelatedTemporaryFile)).IsTrue();
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task FileSystemHistoryStorePartitionsRetentionByPipelineIdentity()
    {
        var directory = CreateTemporaryDirectory();
        var options = OptionsFactory.Create(new PipelineOptions
        {
            RunReport = new RunReportOptions
            {
                HistoryDirectory = directory,
                HistoryRetention = 1,
            },
        });
        var store = new FileSystemRunHistoryStore(
            options,
            NullLogger<FileSystemRunHistoryStore>.Instance);

        try
        {
            await store.SaveAsync(new PipelineRunReport
            {
                PipelineIdentity = "pipeline-a",
                End = new DateTimeOffset(2026, 8, 2, 12, 0, 1, TimeSpan.Zero),
            });
            await store.SaveAsync(new PipelineRunReport
            {
                PipelineIdentity = "pipeline-b",
                End = new DateTimeOffset(2026, 8, 2, 12, 0, 2, TimeSpan.Zero),
            });

            var pipelineA = await store.GetLatestAsync("pipeline-a");
            var pipelineB = await store.GetLatestAsync("pipeline-b");

            using (Assert.Multiple())
            {
                await Assert.That(pipelineA?.PipelineIdentity).IsEqualTo("pipeline-a");
                await Assert.That(pipelineB?.PipelineIdentity).IsEqualTo("pipeline-b");
                await Assert.That(Directory.GetFiles(directory, "modularpipelines-run-*.json"))
                    .Count().IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task FileSystemHistoryStorePrunesAbandonedPipelineIdentitiesToGlobalRetention()
    {
        var directory = CreateTemporaryDirectory();
        var options = OptionsFactory.Create(new PipelineOptions
        {
            RunReport = new RunReportOptions
            {
                HistoryDirectory = directory,
                HistoryRetention = 2,
                GlobalHistoryRetention = 3,
            },
        });
        var store = new FileSystemRunHistoryStore(
            options,
            NullLogger<FileSystemRunHistoryStore>.Instance);

        try
        {
            var unrelatedPath = Path.Combine(directory, "unrelated-report.json");
            await File.WriteAllTextAsync(unrelatedPath, "{}");
            for (var index = 0; index < 4; index++)
            {
                await store.SaveAsync(new PipelineRunReport
                {
                    PipelineIdentity = $"pipeline-{index}",
                    End = new DateTimeOffset(2026, 8, 2, 12, 0, index, TimeSpan.Zero),
                });
            }

            using (Assert.Multiple())
            {
                await Assert.That(await store.GetLatestAsync("pipeline-0")).IsNull();
                await Assert.That(await store.GetLatestAsync("pipeline-1")).IsNotNull();
                await Assert.That(await store.GetLatestAsync("pipeline-3")).IsNotNull();
                await Assert.That(Directory.GetFiles(directory, "modularpipelines-run-*.json"))
                    .Count().IsEqualTo(3);
                await Assert.That(File.Exists(unrelatedPath)).IsTrue();
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task FileSystemHistoryStoreReadsTimestampWithoutAssumingIdentityLength()
    {
        var directory = CreateTemporaryDirectory();
        var store = new FileSystemRunHistoryStore(
            OptionsFactory.Create(new PipelineOptions
            {
                RunReport = new RunReportOptions
                {
                    HistoryDirectory = directory,
                    HistoryRetention = 1,
                    GlobalHistoryRetention = 1,
                },
            }),
            NullLogger<FileSystemRunHistoryStore>.Instance);
        var legacyFile = Path.Combine(
            directory,
            "modularpipelines-run-legacy-202608021300000000000-00000000000000000000000000000000.json");

        try
        {
            Directory.CreateDirectory(directory);
            await File.WriteAllTextAsync(legacyFile, "{}");
            await store.SaveAsync(new PipelineRunReport
            {
                PipelineIdentity = "pipeline-a",
                End = new DateTimeOffset(2026, 8, 2, 12, 0, 0, TimeSpan.Zero),
            });

            using (Assert.Multiple())
            {
                await Assert.That(File.Exists(legacyFile)).IsTrue();
                await Assert.That(Directory.GetFiles(directory, "modularpipelines-run-*.json"))
                    .Count().IsEqualTo(1);
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task AtomicFileWriterKeepsExistingFileWhenWriteFails()
    {
        var directory = CreateTemporaryDirectory();
        var path = Path.Combine(directory, "run-report.json");

        try
        {
            await File.WriteAllTextAsync(path, "complete");

            await Assert.That(async () => await AtomicFileWriter.WriteAllTextAsync(
                    path,
                    "replacement",
                    static async (temporaryPath, _, cancellationToken) =>
                    {
                        await File.WriteAllTextAsync(temporaryPath, "partial", cancellationToken);
                        throw new InvalidOperationException("write failed");
                    }))
                .Throws<InvalidOperationException>();

            using (Assert.Multiple())
            {
                await Assert.That(await File.ReadAllTextAsync(path)).IsEqualTo("complete");
                await Assert.That(Directory.GetFiles(directory, ".modularpipelines-*.tmp"))
                    .IsEmpty();
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task AtomicFileWriterReplacesExistingFileAfterCompletedWrite()
    {
        var directory = CreateTemporaryDirectory();
        var path = Path.Combine(directory, "run-report.json");

        try
        {
            await File.WriteAllTextAsync(path, "original");

            await AtomicFileWriter.WriteAllTextAsync(path, "replacement");

            using (Assert.Multiple())
            {
                await Assert.That(await File.ReadAllTextAsync(path)).IsEqualTo("replacement");
                await Assert.That(Directory.GetFiles(directory, ".modularpipelines-*.tmp"))
                    .IsEmpty();
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task AtomicFileWriterDoesNotPublishCanceledWrite()
    {
        var directory = CreateTemporaryDirectory();
        var path = Path.Combine(directory, "run-report.json");
        using var cancellationTokenSource = new CancellationTokenSource();

        try
        {
            await Assert.That(async () => await AtomicFileWriter.WriteAllTextAsync(
                    path,
                    "replacement",
                    async (temporaryPath, contents, cancellationToken) =>
                    {
                        await File.WriteAllTextAsync(
                            temporaryPath,
                            contents,
                            cancellationToken);
                        cancellationTokenSource.Cancel();
                    },
                    cancellationTokenSource.Token))
                .Throws<OperationCanceledException>();

            using (Assert.Multiple())
            {
                await Assert.That(File.Exists(path)).IsFalse();
                await Assert.That(Directory.GetFiles(directory, ".modularpipelines-*.tmp"))
                    .IsEmpty();
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task FileSystemHistoryStoreAcceptsAdditiveOlderSchemas()
    {
        using (Assert.Multiple())
        {
            await Assert.That(FileSystemRunHistoryStore.IsSchemaVersionCompatible(1, 2)).IsTrue();
            await Assert.That(FileSystemRunHistoryStore.IsSchemaVersionCompatible(2, 2)).IsTrue();
            await Assert.That(FileSystemRunHistoryStore.IsSchemaVersionCompatible(0, 2)).IsFalse();
            await Assert.That(FileSystemRunHistoryStore.IsSchemaVersionCompatible(3, 2)).IsFalse();
        }
    }

    [Test]
    public async Task FileSystemHistoryStoreLogsUnsupportedSchemaOncePerRead()
    {
        var directory = CreateTemporaryDirectory();
        var log = new StringBuilder();
        var store = new FileSystemRunHistoryStore(
            OptionsFactory.Create(new PipelineOptions
            {
                RunReport = new RunReportOptions
                {
                    HistoryDirectory = directory,
                    HistoryRetention = 2,
                },
            }),
            new StringLogger<FileSystemRunHistoryStore>(log));

        try
        {
            for (var second = 1; second <= 2; second++)
            {
                await store.SaveAsync(new PipelineRunReport
                {
                    SchemaVersion = PipelineRunReport.CurrentSchemaVersion + 1,
                    PipelineIdentity = "pipeline-a",
                    End = new DateTimeOffset(2026, 8, 2, 12, 0, second, TimeSpan.Zero),
                });
            }

            foreach (var file in Directory.GetFiles(directory, "modularpipelines-run-*.json"))
            {
                await File.WriteAllTextAsync(
                    file,
                    $$"""
                      {
                        "schemaVersion": {{PipelineRunReport.CurrentSchemaVersion + 1}},
                        "pipelineIdentity": { "future": "pipeline-a" }
                      }
                      """);
            }

            var latest = await store.GetLatestAsync("pipeline-a");

            using (Assert.Multiple())
            {
                await Assert.That(latest).IsNull();
                await Assert.That(log.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                    .Count().IsEqualTo(1);
                await Assert.That(log.ToString()).Contains("supported versions are 1 through");
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task FileSystemHistoryStoreSkipsInvalidSchemaMetadata()
    {
        var directory = CreateTemporaryDirectory();
        var log = new StringBuilder();
        var store = new FileSystemRunHistoryStore(
            OptionsFactory.Create(new PipelineOptions
            {
                RunReport = new RunReportOptions
                {
                    HistoryDirectory = directory,
                    HistoryRetention = 4,
                },
            }),
            new StringLogger<FileSystemRunHistoryStore>(log));

        try
        {
            await store.SaveAsync(new PipelineRunReport
            {
                PipelineIdentity = "pipeline-a",
                End = new DateTimeOffset(2026, 8, 2, 12, 0, 1, TimeSpan.Zero),
            });
            var validReportFile = Directory
                .GetFiles(directory, "modularpipelines-run-*.json")
                .Single();
            var invalidReports = new[]
            {
                "[]",
                "{ \"schemaVersion\": \"future\" }",
                "{ \"schemaVersion\": 2147483648 }",
            };
            for (var index = 0; index < invalidReports.Length; index++)
            {
                await File.WriteAllTextAsync(
                    $"{validReportFile}.invalid-{index}.json",
                    invalidReports[index]);
            }

            var latest = await store.GetLatestAsync("pipeline-a");

            using (Assert.Multiple())
            {
                await Assert.That(latest?.PipelineIdentity).IsEqualTo("pipeline-a");
                await Assert.That(log.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                    .Count().IsEqualTo(invalidReports.Length);
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    [WindowsOnlyTest]
    public async Task FileSystemHistoryStoreContinuesPruningAfterDeleteFailure()
    {
        var directory = CreateTemporaryDirectory();
        var initialStore = CreateHistoryStore(directory, historyRetention: 3);

        try
        {
            for (var second = 1; second <= 3; second++)
            {
                await initialStore.SaveAsync(new PipelineRunReport
                {
                    PipelineIdentity = "pipeline-a",
                    End = new DateTimeOffset(2026, 8, 2, 12, 0, second, TimeSpan.Zero),
                });
            }

            var files = Directory.GetFiles(directory, "modularpipelines-run-*.json")
                .OrderByDescending(Path.GetFileName, StringComparer.Ordinal)
                .ToArray();
            File.SetAttributes(files[0], FileAttributes.ReadOnly);

            await CreateHistoryStore(directory, historyRetention: 1).SaveAsync(new PipelineRunReport
            {
                PipelineIdentity = "pipeline-a",
                End = new DateTimeOffset(2026, 8, 2, 12, 0, 4, TimeSpan.Zero),
            });

            using (Assert.Multiple())
            {
                await Assert.That(File.Exists(files[0])).IsTrue();
                await Assert.That(File.Exists(files[1])).IsFalse();
                await Assert.That(File.Exists(files[2])).IsFalse();
                await Assert.That(Directory.GetFiles(directory, "modularpipelines-run-*.json"))
                    .Count().IsEqualTo(2);
            }
        }
        finally
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }

            Directory.Delete(directory, recursive: true);
        }
    }

    private static FileSystemRunHistoryStore CreateHistoryStore(
        string directory,
        int historyRetention) =>
        new(
            OptionsFactory.Create(new PipelineOptions
            {
                RunReport = new RunReportOptions
                {
                    HistoryDirectory = directory,
                    HistoryRetention = historyRetention,
                },
            }),
            NullLogger<FileSystemRunHistoryStore>.Instance);

    private static PipelineOptions CreateReportingOptions(string reportPath) =>
        new()
        {
            RunReport = new RunReportOptions
            {
                ReportPath = reportPath,
                HistoryRetention = 0,
            },
        };

    [Test]
    public async Task RunReportEnrichersPopulateObfuscatedCorrelation()
    {
        var directory = CreateTemporaryDirectory();
        var distributedOptions = OptionsFactory.Create(new DistributedOptions());
        var commandExecutionCounter = new CommandExecutionCounter();
        var service = new RunReportService(
            Mock.Of<IRunHistoryStore>(),
            new PipelineRunReportFactory(
                commandExecutionCounter,
                new RedactingSecretObfuscator()),
            new KnownBuildSystemDetector(),
            OptionsFactory.Create(CreateReportingOptions(Path.Combine(directory, "report.json"))),
            distributedOptions,
            new RoleDetector(distributedOptions),
            Mock.Of<IDistributedCoordinator>(),
            commandExecutionCounter,
            NullLogger<RunReportService>.Instance,
            runReportEnrichers: [new StaticRunReportEnricher()]);

        try
        {
            var report = await service.CompleteAsync(CreateEmptySummary());

            using (Assert.Multiple())
            {
                await Assert.That(Guid.TryParseExact(report.RunId, "N", out _)).IsTrue();
                await Assert.That(report.Correlation).IsNotNull();
                await Assert.That(report.Correlation!.GitSha).IsEqualTo("**********-sha");
                await Assert.That(report.Correlation.GitBranch).IsEqualTo("**********-branch");
                await Assert.That(report.Correlation.CiRunUrl)
                    .IsEqualTo("https://ci.example/**********-run");
                await Assert.That(report.Correlation.Hostname).IsEqualTo(Environment.MachineName);
                await Assert.That(report.Correlation.BuildSystem).IsEqualTo(BuildSystem.GitHubActions);
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task RepeatedCompletionsReceiveDistinctRunIds()
    {
        var distributedOptions = OptionsFactory.Create(new DistributedOptions());
        var commandExecutionCounter = new CommandExecutionCounter();
        var service = new RunReportService(
            Mock.Of<IRunHistoryStore>(),
            new PipelineRunReportFactory(
                commandExecutionCounter,
                new PassthroughSecretObfuscator()),
            Mock.Of<IBuildSystemDetector>(),
            OptionsFactory.Create(new PipelineOptions()),
            distributedOptions,
            new RoleDetector(distributedOptions),
            Mock.Of<IDistributedCoordinator>(),
            commandExecutionCounter,
            NullLogger<RunReportService>.Instance);

        var firstReport = await service.CompleteAsync(CreateEmptySummary());
        var secondReport = await service.CompleteAsync(CreateEmptySummary());

        using (Assert.Multiple())
        {
            await Assert.That(Guid.TryParseExact(firstReport.RunId, "N", out _)).IsTrue();
            await Assert.That(Guid.TryParseExact(secondReport.RunId, "N", out _)).IsTrue();
            await Assert.That(secondReport.RunId).IsNotEqualTo(firstReport.RunId);
        }
    }

    [Test]
    public async Task RunReportIncludesCommandsExecutedByEnrichers()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "report.json");
        var distributedOptions = OptionsFactory.Create(new DistributedOptions());
        var commandExecutionCounter = new CommandExecutionCounter();
        commandExecutionCounter.Add(moduleType: null, count: 2);
        var service = new RunReportService(
            Mock.Of<IRunHistoryStore>(),
            new PipelineRunReportFactory(
                commandExecutionCounter,
                new PassthroughSecretObfuscator()),
            Mock.Of<IBuildSystemDetector>(),
            OptionsFactory.Create(CreateReportingOptions(reportPath)),
            distributedOptions,
            new RoleDetector(distributedOptions),
            Mock.Of<IDistributedCoordinator>(),
            commandExecutionCounter,
            NullLogger<RunReportService>.Instance,
            runReportEnrichers: [new CommandCountingRunReportEnricher(commandExecutionCounter)]);

        try
        {
            var report = await service.CompleteAsync(CreateEmptySummary());
            var persisted = RunReportJsonSerializer.Deserialize(
                await File.ReadAllTextAsync(reportPath));

            using (Assert.Multiple())
            {
                await Assert.That(report.CommandCount).IsEqualTo(3);
                await Assert.That(report.UnattributedCommandCount).IsEqualTo(3);
                await Assert.That(persisted!.CommandCount).IsEqualTo(3);
                await Assert.That(persisted.UnattributedCommandCount).IsEqualTo(3);
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task BuilderRegistrationInvokesRunReportEnricher()
    {
        var directory = CreateTemporaryDirectory();
        using var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            PrintLogo = false,
            PrintResults = false,
            RunReport = CreateReportingOptions(Path.Combine(directory, "report.json")).RunReport,
        });
        builder.AddModule<SuccessfulModule>();
        builder.AddRunReportEnricher<StaticRunReportEnricher>();

        try
        {
            var summary = await builder.ExecutePipelineAsync();

            await Assert.That(summary.RunReport!.Correlation!.GitSha).IsEqualTo("secret-sha");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task DisabledReportPersistenceSkipsRegisteredEnrichers()
    {
        var distributedOptions = OptionsFactory.Create(new DistributedOptions());
        var commandExecutionCounter = new CommandExecutionCounter();
        var service = new RunReportService(
            Mock.Of<IRunHistoryStore>(),
            new PipelineRunReportFactory(
                commandExecutionCounter,
                new PassthroughSecretObfuscator()),
            Mock.Of<IBuildSystemDetector>(),
            OptionsFactory.Create(new PipelineOptions()),
            distributedOptions,
            new RoleDetector(distributedOptions),
            Mock.Of<IDistributedCoordinator>(),
            commandExecutionCounter,
            NullLogger<RunReportService>.Instance,
            runReportEnrichers: [new StaticRunReportEnricher()]);

        var report = await service.CompleteAsync(CreateEmptySummary());

        await Assert.That(report.Correlation!.GitSha).IsNull();
    }

    [Test]
    public async Task FileSystemHistoryStoreReadsSchemaOneWithoutCorrelation()
    {
        var directory = CreateTemporaryDirectory();

        try
        {
            var store = CreateHistoryStore(directory, historyRetention: 1);
            await store.SaveAsync(new PipelineRunReport
            {
                SchemaVersion = 1,
                PipelineIdentity = "schema-one",
                End = DateTimeOffset.UtcNow,
            });

            var report = await store.GetLatestAsync("schema-one");

            using (Assert.Multiple())
            {
                await Assert.That(report).IsNotNull();
                await Assert.That(report!.SchemaVersion).IsEqualTo(1);
                await Assert.That(report.RunId).IsEmpty();
                await Assert.That(report.Correlation).IsNull();
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task RunReportEnricherTimeoutDoesNotSkipLaterEnrichers()
    {
        var directory = CreateTemporaryDirectory();
        var distributedOptions = OptionsFactory.Create(new DistributedOptions());
        var commandExecutionCounter = new CommandExecutionCounter();
        var service = new RunReportService(
            Mock.Of<IRunHistoryStore>(),
            new PipelineRunReportFactory(
                commandExecutionCounter,
                new PassthroughSecretObfuscator()),
            Mock.Of<IBuildSystemDetector>(),
            OptionsFactory.Create(CreateReportingOptions(Path.Combine(directory, "report.json"))),
            distributedOptions,
            new RoleDetector(distributedOptions),
            Mock.Of<IDistributedCoordinator>(),
            commandExecutionCounter,
            NullLogger<RunReportService>.Instance,
            enricherTimeout: TimeSpan.FromMilliseconds(25),
            runReportEnrichers:
            [
                new NeverCompletingRunReportEnricher(),
                new StaticRunReportEnricher(),
            ]);

        try
        {
            var report = await service.CompleteAsync(CreateEmptySummary())
                .WaitAsync(TimeSpan.FromSeconds(2));

            await Assert.That(report.Correlation!.GitSha).IsEqualTo("secret-sha");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task FileSystemHistoryUsesRunIdInOwnedFileName()
    {
        var directory = CreateTemporaryDirectory();
        var runId = Guid.NewGuid();

        try
        {
            var store = CreateHistoryStore(directory, historyRetention: 1);
            await store.SaveAsync(new PipelineRunReport
            {
                RunId = runId.ToString("D"),
                PipelineIdentity = "run-id-file",
                End = DateTimeOffset.UtcNow,
            });

            var fileName = Path.GetFileName(Directory.EnumerateFiles(directory).Single());
            await Assert.That(fileName).Contains(runId.ToString("N"));
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task KnownCiUsesDefaultArtifactReportPath()
    {
        var options = OptionsFactory.Create(new PipelineOptions());
        var commandExecutionCounter = new CommandExecutionCounter();
        var service = new RunReportService(
            Mock.Of<IRunHistoryStore>(),
            new PipelineRunReportFactory(
                commandExecutionCounter,
                new PassthroughSecretObfuscator()),
            new KnownBuildSystemDetector(),
            options,
            OptionsFactory.Create(new DistributedOptions()),
            new RoleDetector(OptionsFactory.Create(new DistributedOptions())),
            Mock.Of<IDistributedCoordinator>(),
            commandExecutionCounter,
            NullLogger<RunReportService>.Instance);

        await Assert.That(service.GetReportPath())
            .IsEqualTo(Path.Combine("artifacts", "run-report.json"));
    }

    [Test]
    public async Task DistributedWorkerDoesNotPersistRunReportOrHistory()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "run-report.json");
        var historyStore = new Mock<IRunHistoryStore>();
        var coordinator = new Mock<IDistributedCoordinator>();
        coordinator.Setup(x => x.RegisterWorkerAsync(
                It.IsAny<WorkerRegistration>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var commandExecutionCounter = new CommandExecutionCounter();
        commandExecutionCounter.Add(null, 3);
        commandExecutionCounter.Add(typeof(SuccessfulModule), 2);
        var distributedOptions = OptionsFactory.Create(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 1,
            TotalInstances = 2,
            ExecutionIdentifier = "current-run",
        });
        var previousInstance = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");
        Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "1");

        try
        {
            var service = new RunReportService(
                historyStore.Object,
                new PipelineRunReportFactory(
                    commandExecutionCounter,
                    new PassthroughSecretObfuscator()),
                Mock.Of<IBuildSystemDetector>(),
                OptionsFactory.Create(new PipelineOptions
                {
                    RunReport = new RunReportOptions
                    {
                        ReportPath = reportPath,
                        HistoryDirectory = Path.Combine(directory, "history"),
                        HistoryRetention = 2,
                    },
                }),
                distributedOptions,
                new RoleDetector(distributedOptions),
                coordinator.Object,
                commandExecutionCounter,
                NullLogger<RunReportService>.Instance);

            var report = await service.CompleteAsync(CreateEmptySummary());

            using (Assert.Multiple())
            {
                await Assert.That(report).IsNotNull();
                await Assert.That(File.Exists(reportPath)).IsFalse();
                historyStore.Verify(
                    store => store.GetLatestAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                    Times.Never);
                historyStore.Verify(
                    store => store.SaveAsync(It.IsAny<PipelineRunReport>(), It.IsAny<CancellationToken>()),
                    Times.Never);
                coordinator.Verify(x => x.RegisterWorkerAsync(
                    It.Is<WorkerRegistration>(registration =>
                        registration.WorkerIndex == 1
                        && registration.ExecutionIdentifier == "current-run"
                        && registration.UnattributedCommandCount == 3
                        && registration.ModuleCommandCounts![ModuleTypeIdentifier.Get(typeof(SuccessfulModule))] == 2),
                    It.IsAny<CancellationToken>()), Times.Once);
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousInstance);
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task ReportFactoryFailureReturnsFallbackReport()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "fallback.json");
        var module = new CommandModule();
        var start = DateTimeOffset.UtcNow;
        var summary = new PipelineSummary(
            [module],
            [CreateFailureResult(module, start)],
            TimeSpan.FromSeconds(1),
            start,
            start.AddSeconds(1));
        var distributedOptions = OptionsFactory.Create(new DistributedOptions());
        var commandExecutionCounter = new CommandExecutionCounter();
        var service = new RunReportService(
            Mock.Of<IRunHistoryStore>(),
            new PipelineRunReportFactory(
                commandExecutionCounter,
                new ThrowingSecretObfuscator()),
            Mock.Of<IBuildSystemDetector>(),
            OptionsFactory.Create(new PipelineOptions
            {
                RunReport = new RunReportOptions { ReportPath = reportPath },
            }),
            distributedOptions,
            new RoleDetector(distributedOptions),
            Mock.Of<IDistributedCoordinator>(),
            commandExecutionCounter,
            NullLogger<RunReportService>.Instance);

        try
        {
            var report = await service.CompleteAsync(summary);
            var persisted = RunReportJsonSerializer.Deserialize(await File.ReadAllTextAsync(reportPath));

            using (Assert.Multiple())
            {
                await Assert.That(report.Status).IsEqualTo(Status.Failed);
                await Assert.That(report.TotalDuration).IsEqualTo(summary.TotalDuration);
                await Assert.That(report.Modules).IsEmpty();
                await Assert.That(persisted!.Status).IsEqualTo(Status.Failed);
                await Assert.That(persisted.Modules).IsEmpty();
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task PipelineExceptionAndObfuscatorFailureReturnsSafeFallback()
    {
        var distributedOptions = OptionsFactory.Create(new DistributedOptions());
        var commandExecutionCounter = new CommandExecutionCounter();
        var service = new RunReportService(
            Mock.Of<IRunHistoryStore>(),
            new PipelineRunReportFactory(
                commandExecutionCounter,
                new ThrowingSecretObfuscator()),
            Mock.Of<IBuildSystemDetector>(),
            OptionsFactory.Create(new PipelineOptions()),
            distributedOptions,
            new RoleDetector(distributedOptions),
            Mock.Of<IDistributedCoordinator>(),
            commandExecutionCounter,
            NullLogger<RunReportService>.Instance);

        var report = await service.CompleteAsync(
            CreateEmptySummary(),
            new InvalidOperationException("pipeline secret"));

        using (Assert.Multiple())
        {
            await Assert.That(report.Status).IsEqualTo(Status.Failed);
            await Assert.That(report.Exception).IsNotNull();
            await Assert.That(report.Exception!.Type)
                .IsEqualTo(typeof(InvalidOperationException).FullName);
            await Assert.That(report.Exception.Message)
                .IsEqualTo("Exception details unavailable because secret obfuscation failed.");
            await Assert.That(report.Exception.Message).DoesNotContain("pipeline secret");
        }
    }

    [Test]
    public async Task RunHistoryOperationsReceiveBoundedCancellationTokens()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "run-report.json");
        var historyStore = new Mock<IRunHistoryStore>();
        var readToken = CancellationToken.None;
        var saveToken = CancellationToken.None;
        historyStore.Setup(store => store.GetLatestAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((_, token) => readToken = token)
            .ReturnsAsync((PipelineRunReport?) null);
        historyStore.Setup(store => store.SaveAsync(
                It.IsAny<PipelineRunReport>(),
                It.IsAny<CancellationToken>()))
            .Callback<PipelineRunReport, CancellationToken>((_, token) => saveToken = token)
            .Returns(Task.CompletedTask);
        var distributedOptions = OptionsFactory.Create(new DistributedOptions());
        var commandExecutionCounter = new CommandExecutionCounter();
        var service = new RunReportService(
            historyStore.Object,
            new PipelineRunReportFactory(
                commandExecutionCounter,
                new PassthroughSecretObfuscator()),
            Mock.Of<IBuildSystemDetector>(),
            OptionsFactory.Create(new PipelineOptions
            {
                RunReport = new RunReportOptions
                {
                    ReportPath = reportPath,
                    HistoryRetention = 1,
                },
            }),
            distributedOptions,
            new RoleDetector(distributedOptions),
            Mock.Of<IDistributedCoordinator>(),
            commandExecutionCounter,
            NullLogger<RunReportService>.Instance);

        try
        {
            await service.CompleteAsync(CreateEmptySummary());

            using (Assert.Multiple())
            {
                await Assert.That(readToken.CanBeCanceled).IsTrue();
                await Assert.That(saveToken.CanBeCanceled).IsTrue();
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task DistributedWorkerMetricsTimeoutWhenCoordinatorIgnoresCancellation()
    {
        var coordinator = new Mock<IDistributedCoordinator>();
        var registrationCompletion = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        coordinator.Setup(x => x.RegisterWorkerAsync(
                It.IsAny<WorkerRegistration>(),
                It.IsAny<CancellationToken>()))
            .Returns(registrationCompletion.Task);
        var distributedOptions = OptionsFactory.Create(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 1,
            TotalInstances = 2,
        });
        var commandExecutionCounter = new CommandExecutionCounter();
        var previousInstance = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");
        Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "1");

        try
        {
            var service = new RunReportService(
                Mock.Of<IRunHistoryStore>(),
                new PipelineRunReportFactory(
                    commandExecutionCounter,
                    new PassthroughSecretObfuscator()),
                Mock.Of<IBuildSystemDetector>(),
                OptionsFactory.Create(new PipelineOptions()),
                distributedOptions,
                new RoleDetector(distributedOptions),
                coordinator.Object,
                commandExecutionCounter,
                NullLogger<RunReportService>.Instance,
                workerMetricsTimeout: TimeSpan.FromMilliseconds(50));

            var report = await service.CompleteAsync(CreateEmptySummary())
                .WaitAsync(TimeSpan.FromSeconds(2));

            await Assert.That(report).IsNotNull();
            coordinator.Verify(x => x.RegisterWorkerAsync(
                It.IsAny<WorkerRegistration>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousInstance);
        }
    }

    [Test]
    public async Task RunHistoryOperationsTimeoutWhenStoreIgnoresCancellation()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "run-report.json");
        var readCompletion = new TaskCompletionSource<PipelineRunReport?>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var saveCompletion = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var historyStore = new Mock<IRunHistoryStore>();
        historyStore.Setup(store => store.GetLatestAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(readCompletion.Task);
        historyStore.Setup(store => store.SaveAsync(
                It.IsAny<PipelineRunReport>(),
                It.IsAny<CancellationToken>()))
            .Returns(saveCompletion.Task);
        var distributedOptions = OptionsFactory.Create(new DistributedOptions());
        var commandExecutionCounter = new CommandExecutionCounter();
        var service = new RunReportService(
            historyStore.Object,
            new PipelineRunReportFactory(
                commandExecutionCounter,
                new PassthroughSecretObfuscator()),
            Mock.Of<IBuildSystemDetector>(),
            OptionsFactory.Create(new PipelineOptions
            {
                RunReport = new RunReportOptions
                {
                    ReportPath = reportPath,
                    HistoryRetention = 1,
                },
            }),
            distributedOptions,
            new RoleDetector(distributedOptions),
            Mock.Of<IDistributedCoordinator>(),
            commandExecutionCounter,
            NullLogger<RunReportService>.Instance,
            historyStoreTimeout: TimeSpan.FromMilliseconds(50));

        try
        {
            var report = await service.CompleteAsync(CreateEmptySummary())
                .WaitAsync(TimeSpan.FromSeconds(2));

            await Assert.That(report).IsNotNull();
            historyStore.Verify(store => store.GetLatestAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);
            historyStore.Verify(store => store.SaveAsync(
                It.IsAny<PipelineRunReport>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task PipelineLevelFailureIsPersistedAsFailed()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "pipeline-failure.json");

        try
        {
            using var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                PrintLogo = false,
                PrintResults = false,
                RunReport = options.RunReport with { ReportPath = reportPath },
            });
            builder.AddModule<SuccessfulModule>();
            builder.AddPipelineGlobalHooks<ThrowingEndHook>();

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => builder.ExecutePipelineAsync());

            var report = RunReportJsonSerializer.Deserialize(
                await File.ReadAllTextAsync(reportPath));
            using (Assert.Multiple())
            {
                await Assert.That(report!.Status).IsEqualTo(Status.Failed);
                await Assert.That(report.Exception).IsNotNull();
                await Assert.That(report.Exception!.Type)
                    .IsEqualTo(typeof(InvalidOperationException).FullName);
                await Assert.That(report.Exception.Message)
                    .IsEqualTo("Pipeline end hook failed **********");
                await Assert.That(report.Exception.StackTrace).IsNotNull();
                await Assert.That(report.Modules).HasSingleItem();
                await Assert.That(report.Modules[0].ModuleName).IsEqualTo(nameof(SuccessfulModule));
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task RunReportIncludesSuccessfulEndHookDuration()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "lifecycle-duration.json");

        try
        {
            using var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                PrintLogo = false,
                PrintResults = false,
                RunReport = options.RunReport with { ReportPath = reportPath },
            });
            builder.AddModule<SuccessfulModule>();
            builder.AddPipelineGlobalHooks<DelayedEndHook>();

            var summary = await builder.ExecutePipelineAsync();
            var report = RunReportJsonSerializer.Deserialize(
                await File.ReadAllTextAsync(reportPath));
            var expectedParallelism = Math.Round(
                report!.Metrics!.TotalModuleExecutionTime.TotalMilliseconds
                / report.TotalDuration.TotalMilliseconds,
                2);

            using (Assert.Multiple())
            {
                await Assert.That(report.End).IsGreaterThanOrEqualTo(DelayedEndHook.CompletedAt);
                await Assert.That(report.TotalDuration).IsEqualTo(summary.TotalDuration);
                await Assert.That(report.TotalDuration).IsGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(100));
                await Assert.That(report.Metrics.WallClockDuration).IsEqualTo(report.TotalDuration);
                await Assert.That(report.Metrics.ParallelismFactor).IsEqualTo(expectedParallelism);
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task PipelineInitializationFailureIsPersistedAsFailed()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "initialization-failure.json");
        var historyPath = Path.Combine(directory, "history");

        try
        {
            PipelineRunReport failedReport;
            using (var builder = Pipeline.CreateBuilder())
            {
                builder.ConfigurePipelineOptions(options => options with
                {
                    PrintLogo = false,
                    PrintResults = false,
                    RunReport = options.RunReport with
                    {
                        ReportPath = reportPath,
                        HistoryDirectory = historyPath,
                        HistoryRetention = 2,
                    },
                });
                builder.AddModule<SuccessfulModule>();
                builder.AddRequirement(Require.That(_ => false, "requirement failed"));

                await Assert.ThrowsAsync<FailedRequirementsException>(
                    () => builder.ExecutePipelineAsync());

                failedReport = RunReportJsonSerializer.Deserialize(
                    await File.ReadAllTextAsync(reportPath))!;
            }

            using var successfulBuilder = Pipeline.CreateBuilder();
            successfulBuilder.ConfigurePipelineOptions(options => options with
            {
                PrintLogo = false,
                PrintResults = false,
                RunReport = options.RunReport with
                {
                    ReportPath = reportPath,
                    HistoryDirectory = historyPath,
                    HistoryRetention = 2,
                },
            });
            successfulBuilder.AddModule<SuccessfulModule>();
            await successfulBuilder.ExecutePipelineAsync();

            var successfulReport = RunReportJsonSerializer.Deserialize(
                await File.ReadAllTextAsync(reportPath))!;
            using (Assert.Multiple())
            {
                await Assert.That(failedReport.Status).IsEqualTo(Status.Failed);
                await Assert.That(failedReport.Modules).HasSingleItem();
                await Assert.That(failedReport.Modules[0].ModuleName).IsEqualTo(nameof(SuccessfulModule));
                await Assert.That(successfulReport.PipelineIdentity).IsEqualTo(failedReport.PipelineIdentity);
                await Assert.That(successfulReport.PreviousTotalDuration).IsNull();
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task DistributedMasterAggregatesWorkerUnattributedCommands()
    {
        var runStartedAt = DateTimeOffset.UtcNow;
        var module = new SuccessfulModule();
        var moduleTypeIdentifier = ModuleTypeIdentifier.Get(typeof(SuccessfulModule));
        var distributedOptions = OptionsFactory.Create(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });
        var coordinator = new Mock<IDistributedCoordinator>();
        coordinator.Setup(x => x.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                new WorkerRegistration(1, new HashSet<string>(), runStartedAt)
                {
                    UnattributedCommandCount = 3,
                    ModuleCommandCounts = new Dictionary<string, int>(StringComparer.Ordinal)
                    {
                        [moduleTypeIdentifier] = 3,
                    },
                },
                new WorkerRegistration(2, new HashSet<string>(), runStartedAt)
                {
                    UnattributedCommandCount = 0,
                    ModuleCommandCounts = new Dictionary<string, int>(StringComparer.Ordinal),
                },
            ]);
        var commandExecutionCounter = new CommandExecutionCounter();
        commandExecutionCounter.Add(null, 1);
        commandExecutionCounter.AddRemote(typeof(SuccessfulModule), workerIndex: 1, count: 1);
        var previousInstance = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");
        Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "0");

        try
        {
            var service = new RunReportService(
                Mock.Of<IRunHistoryStore>(),
                new PipelineRunReportFactory(
                    commandExecutionCounter,
                    new PassthroughSecretObfuscator()),
                Mock.Of<IBuildSystemDetector>(),
                OptionsFactory.Create(new PipelineOptions()),
                distributedOptions,
                new RoleDetector(distributedOptions),
                coordinator.Object,
                commandExecutionCounter,
                NullLogger<RunReportService>.Instance);

            var report = await service.CompleteAsync(new PipelineSummary(
                    [module],
                    [CreateResult(module, runStartedAt, TimeSpan.FromSeconds(1))],
                    TimeSpan.FromSeconds(1),
                    runStartedAt,
                    runStartedAt.AddSeconds(1),
                    moduleTimelines:
                    [
                        CreateTimeline(typeof(SuccessfulModule), runStartedAt, TimeSpan.FromSeconds(1)),
                    ]))
                .WaitAsync(TimeSpan.FromSeconds(2));

            using (Assert.Multiple())
            {
                await Assert.That(report.CommandCount).IsEqualTo(7);
                await Assert.That(report.UnattributedCommandCount).IsEqualTo(4);
                await Assert.That(report.Modules.Single().CommandCount).IsEqualTo(3);
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousInstance);
        }
    }

    [Test]
    public async Task ModuleFailureIsNotDuplicatedAsPipelineException()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "module-failure.json");

        try
        {
            using var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                ExecutionMode = ExecutionMode.StopOnFirstException,
                ThrowOnPipelineFailure = false,
                PrintLogo = false,
                PrintResults = false,
                RunReport = options.RunReport with { ReportPath = reportPath },
            });
            builder.AddModule<FailingModule>();

            await Assert.ThrowsAsync<ModuleFailedException>(
                () => builder.ExecutePipelineAsync());

            var report = RunReportJsonSerializer.Deserialize(
                await File.ReadAllTextAsync(reportPath));
            using (Assert.Multiple())
            {
                await Assert.That(report!.Status).IsEqualTo(Status.Failed);
                await Assert.That(report.Exception).IsNull();
                await Assert.That(report.Modules).HasSingleItem();
                await Assert.That(report.Modules[0].Exception).IsNotNull();
                await Assert.That(report.Modules[0].Exception!.Message)
                    .IsEqualTo("report failure **********");
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task PipelineExceptionRetainsNonModuleAggregateBranches()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "mixed-failure.json");

        try
        {
            using var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                ExecutionMode = ExecutionMode.WaitForAllModules,
                ThrowOnPipelineFailure = false,
                PrintLogo = false,
                PrintResults = false,
                RunReport = options.RunReport with { ReportPath = reportPath },
            });
            builder.AddModule<FailingModule>();
            builder.AddPipelineGlobalHooks<MixedFailureEndHook>();

            await Assert.ThrowsAsync<AggregateException>(() => builder.ExecutePipelineAsync());

            var report = RunReportJsonSerializer.Deserialize(
                await File.ReadAllTextAsync(reportPath));
            using (Assert.Multiple())
            {
                await Assert.That(report!.Exception).IsNotNull();
                await Assert.That(report.Exception!.InnerExceptions).HasSingleItem();
                await Assert.That(report.Exception.InnerExceptions[0].Message)
                    .IsEqualTo("Pipeline end hook failed");
                await Assert.That(report.Exception.StackTrace)
                    .Contains(nameof(MixedFailureEndHook.OnPipelineEndAsync));
                await Assert.That(report.Modules[0].Exception!.Message)
                    .IsEqualTo("report failure **********");
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task PipelineExceptionRetainsNonModuleWrapper()
    {
        var directory = CreateTemporaryDirectory();
        var reportPath = Path.Combine(directory, "wrapped-failure.json");

        try
        {
            using var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                ExecutionMode = ExecutionMode.WaitForAllModules,
                ThrowOnPipelineFailure = false,
                PrintLogo = false,
                PrintResults = false,
                RunReport = options.RunReport with { ReportPath = reportPath },
            });
            builder.AddModule<FailingModule>();
            builder.AddPipelineGlobalHooks<WrappedFailureEndHook>();

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => builder.ExecutePipelineAsync());

            var report = RunReportJsonSerializer.Deserialize(
                await File.ReadAllTextAsync(reportPath));
            using (Assert.Multiple())
            {
                await Assert.That(report!.Exception).IsNotNull();
                await Assert.That(report.Exception!.Type)
                    .IsEqualTo(typeof(InvalidOperationException).FullName);
                await Assert.That(report.Exception.Message)
                    .IsEqualTo("Pipeline end hook failed");
                await Assert.That(report.Exception.InnerException).IsNull();
                await Assert.That(report.Modules[0].Exception!.Message)
                    .IsEqualTo("report failure **********");
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    [Arguments(3, 1, 3, 0)]
    [Arguments(1, 1, 3, 2)]
    [Arguments(3, 2, 6, 3)]
    public async Task DistributedMasterAddsOnlyMissingUnmatchedWorkerMetrics(
        int collectedRemoteCount,
        int collectedRemoteWorkerIndex,
        int expectedCommandCount,
        int expectedUnattributedCount)
    {
        var runStartedAt = DateTimeOffset.UtcNow;
        var module = new SuccessfulModule();
        var distributedOptions = OptionsFactory.Create(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });
        var coordinator = new Mock<IDistributedCoordinator>();
        coordinator.Setup(x => x.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            [
                new WorkerRegistration(1, new HashSet<string>(), runStartedAt)
                {
                    UnattributedCommandCount = 0,
                    ModuleCommandCounts = new Dictionary<string, int>(StringComparer.Ordinal)
                    {
                        ["worker-load-context-identifier"] = 3,
                    },
                },
                .. collectedRemoteWorkerIndex == 1
                    ? Array.Empty<WorkerRegistration>()
                    : [new WorkerRegistration(2, new HashSet<string>(), runStartedAt)],
            ]);
        var commandExecutionCounter = new CommandExecutionCounter();
        commandExecutionCounter.AddRemote(
            typeof(SuccessfulModule),
            collectedRemoteWorkerIndex,
            collectedRemoteCount);
        var previousInstance = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");
        Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "0");

        try
        {
            var service = new RunReportService(
                Mock.Of<IRunHistoryStore>(),
                new PipelineRunReportFactory(
                    commandExecutionCounter,
                    new PassthroughSecretObfuscator()),
                Mock.Of<IBuildSystemDetector>(),
                OptionsFactory.Create(new PipelineOptions()),
                distributedOptions,
                new RoleDetector(distributedOptions),
                coordinator.Object,
                commandExecutionCounter,
                NullLogger<RunReportService>.Instance,
                workerMetricsTimeout: TimeSpan.FromMilliseconds(50));

            var report = await service.CompleteAsync(new PipelineSummary(
                    [module],
                    [CreateResult(module, runStartedAt, TimeSpan.FromSeconds(1))],
                    TimeSpan.FromSeconds(1),
                    runStartedAt,
                    runStartedAt.AddSeconds(1)))
                .WaitAsync(TimeSpan.FromSeconds(2));

            using (Assert.Multiple())
            {
                await Assert.That(report.CommandCount).IsEqualTo(expectedCommandCount);
                await Assert.That(report.UnattributedCommandCount).IsEqualTo(expectedUnattributedCount);
                await Assert.That(report.Modules.Single().CommandCount).IsEqualTo(collectedRemoteCount);
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousInstance);
        }
    }

    [Test]
    public async Task DistributedMasterReconcilesMatchedMetricsPerWorker()
    {
        var firstContext = new DuplicateAssemblyLoadContext();
        var secondContext = new DuplicateAssemblyLoadContext();
        try
        {
            var assemblyBytes = await File.ReadAllBytesAsync(typeof(RunReportTests).Assembly.Location);
            var moduleTypeName = typeof(DuplicateModuleBase).FullName!;
            var firstType = firstContext.LoadFromStream(new MemoryStream(assemblyBytes))
                .GetType(moduleTypeName, throwOnError: true)!;
            var secondType = secondContext.LoadFromStream(new MemoryStream(assemblyBytes))
                .GetType(moduleTypeName, throwOnError: true)!;
            var firstModule = (IModule) Activator.CreateInstance(firstType)!;
            var secondModule = (IModule) Activator.CreateInstance(secondType)!;
            var moduleTypeIdentifier = ModuleTypeIdentifier.Get(firstType);
            var runStartedAt = DateTimeOffset.UtcNow;
            var distributedOptions = OptionsFactory.Create(new DistributedOptions
            {
                Enabled = true,
                InstanceIndex = 0,
                TotalInstances = 3,
            });
            var coordinator = new Mock<IDistributedCoordinator>();
            coordinator.Setup(x => x.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync([
                    new WorkerRegistration(1, new HashSet<string>(), runStartedAt)
                    {
                        UnattributedCommandCount = 0,
                        ModuleCommandCounts = new Dictionary<string, int>(StringComparer.Ordinal)
                        {
                            [moduleTypeIdentifier] = 3,
                        },
                    },
                    new WorkerRegistration(2, new HashSet<string>(), runStartedAt),
                ]);
            var commandExecutionCounter = new CommandExecutionCounter();
            commandExecutionCounter.Add(firstType, count: 5);
            commandExecutionCounter.AddRemote(secondType, workerIndex: 2, count: 5);
            var previousInstance = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "0");

            try
            {
                var service = new RunReportService(
                    Mock.Of<IRunHistoryStore>(),
                    new PipelineRunReportFactory(
                        commandExecutionCounter,
                        new PassthroughSecretObfuscator()),
                    Mock.Of<IBuildSystemDetector>(),
                    OptionsFactory.Create(new PipelineOptions()),
                    distributedOptions,
                    new RoleDetector(distributedOptions),
                    coordinator.Object,
                    commandExecutionCounter,
                    NullLogger<RunReportService>.Instance,
                    workerMetricsTimeout: TimeSpan.FromMilliseconds(50));

                var report = await service.CompleteAsync(new PipelineSummary(
                        [firstModule, secondModule],
                        [],
                        TimeSpan.Zero,
                        runStartedAt,
                        runStartedAt))
                    .WaitAsync(TimeSpan.FromSeconds(2));

                using (Assert.Multiple())
                {
                    await Assert.That(ModuleTypeIdentifier.Get(secondType))
                        .IsEqualTo(moduleTypeIdentifier);
                    await Assert.That(report.CommandCount).IsEqualTo(13);
                    await Assert.That(report.UnattributedCommandCount).IsEqualTo(3);
                    await Assert.That(report.Modules.Sum(module => module.CommandCount)).IsEqualTo(10);
                }
            }
            finally
            {
                Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousInstance);
            }
        }
        finally
        {
            firstContext.Unload();
            secondContext.Unload();
        }
    }

    [Test]
    public async Task DistributedMasterWaitsForParticipatingWorkerFinalMetrics()
    {
        var runStartedAt = DateTimeOffset.UtcNow;
        var distributedOptions = OptionsFactory.Create(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 2,
        });
        var incompleteRegistration = new WorkerRegistration(
            1,
            new HashSet<string>(),
            runStartedAt);
        var pollingCount = 0;
        var coordinator = new Mock<IDistributedCoordinator>();
        coordinator.Setup(x => x.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                var registration = Interlocked.Increment(ref pollingCount) <= 12
                    ? incompleteRegistration
                    : incompleteRegistration with { UnattributedCommandCount = 3 };
                return [registration];
            });
        var commandExecutionCounter = new CommandExecutionCounter();
        commandExecutionCounter.Add(null, 1);
        var previousInstance = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");
        Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "0");

        try
        {
            var service = new RunReportService(
                Mock.Of<IRunHistoryStore>(),
                new PipelineRunReportFactory(
                    commandExecutionCounter,
                    new PassthroughSecretObfuscator()),
                Mock.Of<IBuildSystemDetector>(),
                OptionsFactory.Create(new PipelineOptions()),
                distributedOptions,
                new RoleDetector(distributedOptions),
                coordinator.Object,
                commandExecutionCounter,
                NullLogger<RunReportService>.Instance);

            var report = await service.CompleteAsync(CreateEmptySummary(runStartedAt))
                .WaitAsync(TimeSpan.FromSeconds(10));

            using (Assert.Multiple())
            {
                await Assert.That(report.CommandCount).IsEqualTo(4);
                await Assert.That(report.UnattributedCommandCount).IsEqualTo(4);
            }

            await Assert.That(pollingCount).IsGreaterThan(12);
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousInstance);
        }
    }

    [Test]
    public async Task DistributedMasterMetricsTimeoutWhenCoordinatorIgnoresCancellation()
    {
        var queryCompletion = new TaskCompletionSource<IReadOnlyList<WorkerRegistration>>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var coordinator = new Mock<IDistributedCoordinator>();
        coordinator.Setup(x => x.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()))
            .Returns(queryCompletion.Task);
        var distributedOptions = OptionsFactory.Create(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 2,
        });
        var commandExecutionCounter = new CommandExecutionCounter();
        var previousInstance = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");
        Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "0");

        try
        {
            var service = new RunReportService(
                Mock.Of<IRunHistoryStore>(),
                new PipelineRunReportFactory(
                    commandExecutionCounter,
                    new PassthroughSecretObfuscator()),
                Mock.Of<IBuildSystemDetector>(),
                OptionsFactory.Create(new PipelineOptions()),
                distributedOptions,
                new RoleDetector(distributedOptions),
                coordinator.Object,
                commandExecutionCounter,
                NullLogger<RunReportService>.Instance,
                workerMetricsTimeout: TimeSpan.FromMilliseconds(50));

            var report = await service.CompleteAsync(CreateEmptySummary())
                .WaitAsync(TimeSpan.FromSeconds(2));

            await Assert.That(report).IsNotNull();
            coordinator.Verify(
                x => x.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousInstance);
        }
    }

    [Test]
    public async Task DistributedMasterDoesNotWaitForWorkersMissingFromThisRun()
    {
        var runStartedAt = DateTimeOffset.UtcNow;
        var distributedOptions = OptionsFactory.Create(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });
        var coordinator = new Mock<IDistributedCoordinator>();
        coordinator.Setup(x => x.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                new WorkerRegistration(1, new HashSet<string>(), runStartedAt)
                {
                    UnattributedCommandCount = 3,
                },
            ]);
        var commandExecutionCounter = new CommandExecutionCounter();
        var previousInstance = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");
        Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "0");

        try
        {
            var service = new RunReportService(
                Mock.Of<IRunHistoryStore>(),
                new PipelineRunReportFactory(
                    commandExecutionCounter,
                    new PassthroughSecretObfuscator()),
                Mock.Of<IBuildSystemDetector>(),
                OptionsFactory.Create(new PipelineOptions()),
                distributedOptions,
                new RoleDetector(distributedOptions),
                coordinator.Object,
                commandExecutionCounter,
                NullLogger<RunReportService>.Instance);

            var report = await service.CompleteAsync(CreateEmptySummary(runStartedAt))
                .WaitAsync(TimeSpan.FromSeconds(2));

            await Assert.That(report.UnattributedCommandCount).IsEqualTo(3);
            coordinator.Verify(
                x => x.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousInstance);
        }
    }

    [Test]
    public async Task DistributedMasterUsesExecutionIdentifierInsteadOfWorkerClock()
    {
        var runStartedAt = DateTimeOffset.UtcNow;
        const string executionIdentifier = "current-run";
        var distributedOptions = OptionsFactory.Create(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
            ExecutionIdentifier = executionIdentifier,
        });
        var coordinator = new Mock<IDistributedCoordinator>();
        coordinator.Setup(x => x.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                new WorkerRegistration(1, new HashSet<string>(), runStartedAt.AddMinutes(-5))
                {
                    ExecutionIdentifier = executionIdentifier,
                    UnattributedCommandCount = 3,
                },
                new WorkerRegistration(2, new HashSet<string>(), runStartedAt.AddMinutes(5))
                {
                    ExecutionIdentifier = "previous-run",
                    UnattributedCommandCount = 99,
                },
            ]);
        var commandExecutionCounter = new CommandExecutionCounter();
        var previousInstance = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");
        Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "0");

        try
        {
            var service = new RunReportService(
                Mock.Of<IRunHistoryStore>(),
                new PipelineRunReportFactory(
                    commandExecutionCounter,
                    new PassthroughSecretObfuscator()),
                Mock.Of<IBuildSystemDetector>(),
                OptionsFactory.Create(new PipelineOptions()),
                distributedOptions,
                new RoleDetector(distributedOptions),
                coordinator.Object,
                commandExecutionCounter,
                NullLogger<RunReportService>.Instance);

            var report = await service.CompleteAsync(CreateEmptySummary(runStartedAt))
                .WaitAsync(TimeSpan.FromSeconds(2));

            await Assert.That(report.UnattributedCommandCount).IsEqualTo(3);
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousInstance);
        }
    }

    [Test]
    public async Task PipelineSummaryRoundTripPreservesRunReport()
    {
        var summary = CreateEmptySummary() with
        {
            RunReport = new PipelineRunReport
            {
                PipelineIdentity = "round-trip",
                CommandCount = 7,
            },
        };

        var deserialized = JsonSerializer.Deserialize<PipelineSummary>(JsonSerializer.Serialize(summary));

        using (Assert.Multiple())
        {
            await Assert.That(deserialized!.RunReport).IsNotNull();
            await Assert.That(deserialized.RunReport!.PipelineIdentity).IsEqualTo("round-trip");
            await Assert.That(deserialized.RunReport.CommandCount).IsEqualTo(7);
        }
    }

    [Test]
    public async Task RunReportOptionsRejectNegativeRetention()
    {
        var result = new OptionsValidator().ValidateOptions(new PipelineOptions
        {
            RunReport = new RunReportOptions { HistoryRetention = -1 },
        });

        await Assert.That(result.Errors.Select(error => error.Message))
            .Contains(message => message.Contains("HistoryRetention", StringComparison.Ordinal));
    }

    [Test]
    public async Task RunReportJsonRoundTripPreservesModuleOutputExcerpt()
    {
        var report = new PipelineRunReport
        {
            Modules =
            [
                new ModuleRunReport
                {
                    Output = new ModuleOutputExcerpt
                    {
                        StdoutTail = "stdout tail",
                        StderrTail = "stderr tail",
                        TruncatedBytes = 42,
                    },
                },
            ],
        };

        var deserialized = RunReportJsonSerializer.Deserialize(
            RunReportJsonSerializer.Serialize(report));
        var output = deserialized!.Modules.Single().Output;

        using (Assert.Multiple())
        {
            await Assert.That(output!.StdoutTail).IsEqualTo("stdout tail");
            await Assert.That(output.StderrTail).IsEqualTo("stderr tail");
            await Assert.That(output.TruncatedBytes).IsEqualTo(42);
        }
    }

    [Test]
    public async Task RunReportOptionsRejectNonPositiveOutputLimit()
    {
        var result = new OptionsValidator().ValidateOptions(new PipelineOptions
        {
            RunReport = new RunReportOptions
            {
                IncludeModuleOutput = true,
                MaxOutputBytesPerModule = 0,
            },
        });

        await Assert.That(result.Errors.Select(error => error.Message))
            .Contains(message => message.Contains("MaxOutputBytesPerModule", StringComparison.Ordinal));
    }

    [Test]
    public async Task RunReportOptionsIgnoreOutputLimitWhenOutputIsDisabled()
    {
        var result = new OptionsValidator().ValidateOptions(new PipelineOptions
        {
            RunReport = new RunReportOptions { MaxOutputBytesPerModule = 0 },
        });

        await Assert.That(result.Errors).IsEmpty();
    }

    [Test]
    public async Task RunReportIncludesMaskedModuleOutputWhenEnabled()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            PrintLogo = false,
            PrintResults = false,
            RunReport = options.RunReport with
            {
                AutoWriteInCi = false,
                HistoryRetention = 0,
                IncludeModuleOutput = true,
                MaxOutputBytesPerModule = 1024,
            },
        });
        builder.AddModule<OutputExcerptModule>();

        var summary = await builder.ExecutePipelineAsync();
        var output = summary.RunReport!.Modules.Single().Output;

        using (Assert.Multiple())
        {
            await Assert.That(output).IsNotNull();
            await Assert.That(output!.StdoutTail).Contains("stdout");
            await Assert.That(output.StderrTail).Contains("stderr");
            await Assert.That(output.StdoutTail).DoesNotContain("module-output-secret");
            await Assert.That(output.StderrTail).DoesNotContain("module-output-secret");
        }
    }

    [Test]
    public async Task RunReportMasksOutputAgainAtCreation()
    {
        var module = new SuccessfulModule();
        var start = DateTimeOffset.UtcNow;
        var summary = new PipelineSummary(
            [module],
            [CreateResult(module, start, TimeSpan.FromSeconds(1))],
            TimeSpan.FromSeconds(1),
            start,
            start.AddSeconds(1));
        var outputProvider = new Mock<IModuleOutputExcerptProvider>();
        outputProvider
            .Setup(x => x.GetModuleOutputExcerpt(typeof(SuccessfulModule)))
            .Returns(new ModuleOutputExcerpt
            {
                StdoutTail = "late-secret",
                StderrTail = "late-secret",
            });
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) => value?.Replace("late-secret", "***") ?? string.Empty);

        var report = new PipelineRunReportFactory(
                Mock.Of<ICommandExecutionCounter>(),
                obfuscator.Object,
                outputProvider.Object)
            .Create(summary, null, "output-remasking");

        using (Assert.Multiple())
        {
            await Assert.That(report.Modules.Single().Output!.StdoutTail).IsEqualTo("***");
            await Assert.That(report.Modules.Single().Output!.StderrTail).IsEqualTo("***");
        }
    }

    [Test]
    public async Task RunReportOptionsRejectNegativeGlobalRetention()
    {
        var result = new OptionsValidator().ValidateOptions(new PipelineOptions
        {
            RunReport = new RunReportOptions { GlobalHistoryRetention = -1 },
        });

        await Assert.That(result.Errors.Select(error => error.Message))
            .Contains(message => message.Contains("GlobalHistoryRetention", StringComparison.Ordinal));
    }

    [Test]
    public async Task RunReportOptionsRejectGlobalRetentionBelowPerIdentityRetention()
    {
        var result = new OptionsValidator().ValidateOptions(new PipelineOptions
        {
            RunReport = new RunReportOptions
            {
                HistoryRetention = 5,
                GlobalHistoryRetention = 4,
            },
        });

        await Assert.That(result.Errors.Select(error => error.Message))
            .Contains(message => message.Contains("GlobalHistoryRetention", StringComparison.Ordinal)
                                 && message.Contains("HistoryRetention", StringComparison.Ordinal));
    }

    private static async Task<PipelineSummary> RunPipelineAsync(
        string reportPath,
        string historyPath)
    {
        using var builder = Pipeline.CreateBuilder();
        builder.WriteRunReport(reportPath);
        builder.ConfigurePipelineOptions(options => options with
        {
            ExecutionMode = ExecutionMode.WaitForAllModules,
            ThrowOnPipelineFailure = false,
            PrintLogo = false,
            PrintResults = false,
            RunReport = options.RunReport with
            {
                HistoryDirectory = historyPath,
                HistoryRetention = 2,
            },
        });
        builder.Services.AddSingleton<ICommandInterceptor, SuccessfulCommandInterceptor>();
        builder.AddModule<CommandModule>();
        builder.AddModule<FailingModule>();
        builder.AddModule<SkippedModule>();
        return await builder.ExecutePipelineAsync();
    }

    private static async Task<PipelineSummary> RunPipelineWithoutReportAsync(string historyPath)
    {
        using var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            PrintLogo = false,
            PrintResults = false,
            RunReport = options.RunReport with
            {
                AutoWriteInCi = false,
                HistoryDirectory = historyPath,
                HistoryRetention = 2,
            },
        });
        builder.AddModule<SuccessfulModule>();
        return await builder.ExecutePipelineAsync();
    }

    private static string CreateTemporaryDirectory()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"ModularPipelines-run-report-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        return directory;
    }

    private static Type CreateDynamicModuleType(
        string assemblyName,
        Version version,
        string typeName = "Duplicate.ReportModule")
    {
        var dynamicAssemblyName = new AssemblyName(assemblyName) { Version = version };
        var assembly = AssemblyBuilder.DefineDynamicAssembly(
            dynamicAssemblyName,
            AssemblyBuilderAccess.Run);
        return assembly
            .DefineDynamicModule(assemblyName)
            .DefineType(typeName, TypeAttributes.Public, typeof(DuplicateModuleBase))
            .CreateType()!;
    }

    private static Type CreateDynamicType(
        string assemblyName,
        Version version,
        string typeName)
    {
        var dynamicAssemblyName = new AssemblyName(assemblyName) { Version = version };
        var assembly = AssemblyBuilder.DefineDynamicAssembly(
            dynamicAssemblyName,
            AssemblyBuilderAccess.Run);
        return assembly
            .DefineDynamicModule(assemblyName)
            .DefineType(typeName, TypeAttributes.Public)
            .CreateType()!;
    }

    private static PipelineSummary CreateEmptySummary(DateTimeOffset? start = null)
    {
        var now = start ?? DateTimeOffset.UtcNow;
        return new PipelineSummary([], [], TimeSpan.Zero, now, now);
    }

    private static IModuleResult CreateFailureResult(
        IModule module,
        DateTimeOffset start)
    {
        var context = new ModuleExecutionContext(module, module.GetType())
        {
            Status = Status.Failed,
            StartTime = start,
            EndTime = start.AddSeconds(1),
        };
        try
        {
            return ModuleResult.CreateFailure(
                new InvalidOperationException("Report creation failure"),
                context);
        }
        finally
        {
            context.ModuleCancellationTokenSource.Dispose();
        }
    }

    private static IModuleResult CreateResult(
        IModule module,
        DateTimeOffset start,
        TimeSpan duration)
    {
        var context = new ModuleExecutionContext(module, module.GetType())
        {
            Status = Status.Successful,
            StartTime = start,
            EndTime = start + duration,
        };
        try
        {
            return ModuleResult<string>.CreateSuccess(null, context);
        }
        finally
        {
            context.ModuleCancellationTokenSource.Dispose();
        }
    }

    private static IModuleResult CreateSkippedResult(IModule module)
    {
        var context = new ModuleExecutionContext(module, module.GetType())
        {
            Status = Status.Skipped,
        };
        try
        {
            return ModuleResult.CreateSkipped(
                SkipDecision.Skip("ignored"),
                context);
        }
        finally
        {
            context.ModuleCancellationTokenSource.Dispose();
        }
    }

    private static IModuleResult CreateHistoricalResult(IModule module, DateTimeOffset start) =>
        (ModuleResult) CreateResult(module, start, TimeSpan.Zero) with
        {
            ModuleStatus = Status.UsedHistory,
        };

    private static IModuleResult CreateCachedResult(IModule module, DateTimeOffset start) =>
        (ModuleResult)CreateResult(module, start, TimeSpan.Zero) with
        {
            ModuleStatus = Status.CachedResult,
        };

    private static ModuleRunReport CreatePreviousModuleReport(
        Type moduleType,
        string typeName,
        DateTimeOffset start,
        TimeSpan duration) => new()
        {
            ModuleName = moduleType.Name,
            ModuleTypeName = typeName,
            Status = Status.Successful,
            Duration = duration,
            DurationMeasured = true,
            Start = start,
            End = start + duration,
        };

    private static PipelineRunReport CreateMeasuredRunReport(
        Status status,
        TimeSpan duration,
        PipelineRunReport? previousReport = null)
    {
        var module = new SuccessfulModule();
        var start = new DateTimeOffset(2026, 8, 3, 12, 0, 0, TimeSpan.Zero);
        var result = (ModuleResult)CreateResult(module, start, duration) with
        {
            ModuleStatus = status,
        };
        var summary = new PipelineSummary(
            [module],
            [result],
            duration,
            start,
            start + duration,
            moduleTimelines:
            [
                CreateTimeline(typeof(SuccessfulModule), start, duration) with
                {
                    Status = status,
                },
            ]) with
        {
            StatusOverride = status,
        };

        return new PipelineRunReportFactory(
                Mock.Of<ICommandExecutionCounter>(),
                new PassthroughSecretObfuscator())
            .Create(summary, previousReport, "measured-run");
    }

    private static ModuleTimeline CreateTimeline(
        Type moduleType,
        DateTimeOffset start,
        TimeSpan duration) => new()
        {
            ModuleName = moduleType.Name,
            ModuleTypeName = ModuleTypeIdentifier.Get(moduleType),
            RuntimeModuleTypeName = ModuleTypeIdentifier.GetRuntime(moduleType),
            StartTime = start,
            EndTime = start + duration,
            ExecutionDuration = duration,
            Status = Status.Successful,
        };
}
