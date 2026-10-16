using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Events;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.PipelineCli;
using ModularPipelines.Reporting;
using Moq;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.UnitTests.Engine;

public class RerunFailedTests
{
    [Test]
    public async Task Failed_Run_Restores_Successful_Values_And_Executes_Failures_And_Dependents()
    {
        var state = new RunState();
        var first = await RunAsync(state);
        await Assert.That(Result<FlakyModule>(first).Status).IsEqualTo(ModuleStatus.Failed);
        await Assert.That(state.Count<FinalModule>()).IsEqualTo(0);

        // Persisted repositories must work without in-process module type references.
        state.History.Reports[0] = RunReportJsonSerializer.Deserialize(
            RunReportJsonSerializer.Serialize(state.History.Reports[0]))!;
        foreach (var (moduleType, result) in state.Results.Values.ToArray())
        {
            state.Results.Values[moduleType] = JsonSerializer.Deserialize<ModuleResult<int>>(
                JsonSerializer.Serialize((ModuleResult<int>) result))!;
        }

        state.Fail = false;
        var second = await RunAsync(state, rerun: true);

        using (Assert.Multiple())
        {
            await Assert.That(state.Count<RootModule>()).IsEqualTo(1);
            await Assert.That(state.Count<StableChildModule>()).IsEqualTo(1);
            await Assert.That(state.Count<IndependentModule>()).IsEqualTo(1);
            await Assert.That(state.Count<FlakyModule>()).IsEqualTo(2);
            await Assert.That(state.Count<FinalModule>()).IsEqualTo(1);
            await Assert.That(Result<RootModule>(second).Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
            await Assert.That(Result<FlakyModule>(second).ValueOrDefault).IsEqualTo(43);
            await Assert.That(Result<FinalModule>(second).ValueOrDefault).IsEqualTo(44);
        }

        // A third rerun must retain the original result provenance for restored modules.
        await RunAsync(state, rerun: true);
        await Assert.That(state.Count<RootModule>()).IsEqualTo(1);
        await Assert.That(state.Count<FinalModule>()).IsEqualTo(1);
    }

    [Test]
    public async Task Missing_Result_Executes_Transitive_Dependents_Even_When_They_Previously_Succeeded()
    {
        var state = new RunState { Fail = false };
        await RunAsync(state);
        state.Results.Values.TryRemove(typeof(RootModule), out _);

        var summary = await RunAsync(state, rerun: true);

        using (Assert.Multiple())
        {
            await Assert.That(state.Count<RootModule>()).IsEqualTo(2);
            await Assert.That(state.Count<StableChildModule>()).IsEqualTo(2);
            await Assert.That(state.Count<FlakyModule>()).IsEqualTo(2);
            await Assert.That(state.Count<FinalModule>()).IsEqualTo(2);
            await Assert.That(state.Count<IndependentModule>()).IsEqualTo(1);
            await Assert.That(Result<FinalModule>(summary).ValueOrDefault).IsEqualTo(44);
        }
    }

    [Test]
    [Arguments(ModuleStatus.Canceled)]
    [Arguments(ModuleStatus.DependencyFailed)]
    [Arguments(ModuleStatus.Skipped)]
    [Arguments(ModuleStatus.Unknown)]
    [Arguments(ModuleStatus.Failed)]
    public async Task NonSuccessful_Previous_Status_Executes_Module_And_Transitive_Dependents(ModuleStatus status)
    {
        var state = new RunState { Fail = false };
        await RunAsync(state);
        var report = state.History.Reports.Single();
        state.History.Reports[0] = report with
        {
            Modules = report.Modules.Select(module => module.ModuleName == nameof(RootModule)
                ? module with { Status = status }
                : module).ToArray(),
        };

        await RunAsync(state, rerun: true);

        await Assert.That(state.Count<RootModule>()).IsEqualTo(2);
        await Assert.That(state.Count<StableChildModule>()).IsEqualTo(2);
        await Assert.That(state.Count<FinalModule>()).IsEqualTo(2);
        await Assert.That(state.Count<IndependentModule>()).IsEqualTo(1);
    }

    [Test]
    [Arguments("time")]
    [Arguments("type")]
    [Arguments("status")]
    [Arguments("failure")]
    public async Task Invalid_Stored_Result_Executes_Module_And_Dependents(string corruption)
    {
        var state = new RunState { Fail = false };
        await RunAsync(state);
        var original = (ModuleResult<int>) state.Results.Values[typeof(RootModule)];
        state.Results.Values[typeof(RootModule)] = corruption switch
        {
            "time" => original with { StartTime = original.StartTime.AddDays(-1) },
            "type" => original with
            {
                TypeName = typeof(IndependentModule).FullName,
                ModuleType = typeof(IndependentModule),
            },
            "status" => original with { Status = ModuleStatus.Failed },
            "failure" => new ModuleResult<int>.Failure(new InvalidOperationException("Invalid stored result"))
            {
                Name = original.Name,
                TypeName = original.TypeName,
                StartTime = original.StartTime,
                EndTime = original.EndTime,
                Duration = original.Duration,
                Status = ModuleStatus.Succeeded,
            },
            _ => throw new ArgumentOutOfRangeException(nameof(corruption)),
        };

        await RunAsync(state, rerun: true);

        await Assert.That(state.Count<RootModule>()).IsEqualTo(2);
        await Assert.That(state.Count<FinalModule>()).IsEqualTo(2);
        await Assert.That(state.Count<IndependentModule>()).IsEqualTo(1);
    }

    [Test]
    [Arguments("missing")]
    [Arguments("corrupt")]
    [Arguments("fingerprint")]
    [Arguments("identity")]
    [Arguments("schema")]
    public async Task Unusable_History_Executes_Full_Graph(string corruption)
    {
        var state = new RunState { Fail = false };
        await RunAsync(state);
        var report = state.History.Reports.Single();
        state.History.Reports.Clear();
        switch (corruption)
        {
            case "missing":
                break;
            case "corrupt":
                state.History.ThrowOnRead = true;
                break;
            case "fingerprint":
                state.History.Reports.Add(report with { GraphFingerprint = "different-graph" });
                break;
            case "identity":
                state.History.Reports.Add(report with { PipelineIdentity = "different-pipeline" });
                break;
            case "schema":
                state.History.Reports.Add(report with { SchemaVersion = PipelineRunReport.CurrentSchemaVersion + 1 });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(corruption));
        }

        var summary = await RunAsync(state, rerun: true);

        await Assert.That(state.Count<RootModule>()).IsEqualTo(2);
        await Assert.That(state.Count<FinalModule>()).IsEqualTo(2);
        await Assert.That(state.Count<IndependentModule>()).IsEqualTo(2);
        await Assert.That(summary.Results.All(result => result.Status == ModuleStatus.Succeeded)).IsTrue();
    }

    [Test]
    public async Task Changed_Registered_Graph_With_Same_Identity_Executes_All_Modules()
    {
        var state = new RunState { Fail = false };
        await RunAsync(state);

        var summary = await RunAsync(state, rerun: true, addModule: true);

        await Assert.That(state.Count<RootModule>()).IsEqualTo(2);
        await Assert.That(state.Count<IndependentModule>()).IsEqualTo(2);
        await Assert.That(state.Count<AddedModule>()).IsEqualTo(1);
        await Assert.That(summary.RunReport!.GraphFingerprint)
            .IsNotEqualTo(state.History.Reports.First().GraphFingerprint);
    }

    [Test]
    public async Task Changed_Dependency_Edges_With_Same_Module_Types_Execute_Full_Graph()
    {
        var state = new RunState { Fail = false };
        await RunAsync(state);
        state.AddDependency = true;

        var summary = await RunAsync(state, rerun: true);

        await Assert.That(state.Count<RootModule>()).IsEqualTo(2);
        await Assert.That(state.Count<IndependentModule>()).IsEqualTo(2);
        await Assert.That(summary.RunReport!.GraphFingerprint)
            .IsNotEqualTo(state.History.Reports.First().GraphFingerprint);
    }

    [Test]
    public async Task Newer_Incompatible_Report_Does_Not_Hide_Compatible_History()
    {
        var state = new RunState { Fail = false };
        await RunAsync(state);
        var compatible = state.History.Reports.Single();
        state.History.Reports.Add(compatible with
        {
            RunId = "newer-incompatible",
            GraphFingerprint = "different-graph",
            End = compatible.End.AddMinutes(1),
        });

        var summary = await RunAsync(state, rerun: true);

        await Assert.That(state.Count<RootModule>()).IsEqualTo(1);
        await Assert.That(Result<RootModule>(summary).Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
    }

    [Test]
    public async Task No_Cache_Disables_Rerun_Result_Restoration()
    {
        var state = new RunState { Fail = false };
        await RunAsync(state);
        state.Results.ReadCount = 0;

        await RunAsync(state, rerun: true, configure: options => options with { DisableModuleCache = true });

        await Assert.That(state.Count<RootModule>()).IsEqualTo(2);
        await Assert.That(state.Count<IndependentModule>()).IsEqualTo(2);
        await Assert.That(state.Results.ReadCount).IsEqualTo(0);
    }

    [Test]
    public async Task Rerun_Bypasses_Module_Cache_Reads_And_Writes()
    {
        var state = new RunState();
        await RunAsync(state);
        state.Fail = false;
        var cache = new TrackingCacheRepository();

        await RunAsync(state, rerun: true, cache: cache);

        await Assert.That(cache.ReadCount).IsEqualTo(0);
        await Assert.That(cache.WriteCount).IsEqualTo(0);
        await Assert.That(state.Count<FlakyModule>()).IsEqualTo(2);
    }

    [Test]
    [Arguments("category")]
    [Arguments("module")]
    [Arguments("condition")]
    [Arguments("target")]
    public async Task Current_Selection_And_Skip_Conditions_Take_Priority_Over_Restoration(string selection)
    {
        var state = new RunState { Fail = false };
        await RunAsync(state);
        state.SkipIndependent = selection == "condition";

        var summary = await RunAsync(state, rerun: true, configure: options => selection switch
        {
            "category" => options with { IgnoreCategories = ["independent"] },
            "module" => options with { SkippedModules = [nameof(IndependentModule)] },
            "target" => options with { TargetModules = [nameof(FinalModule)] },
            _ => options,
        });

        await Assert.That(state.Count<IndependentModule>()).IsEqualTo(1);
        await Assert.That(Result<IndependentModule>(summary).Status).IsNotEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Command_Line_Recognizes_Rerun_Failed_And_No_Cache()
    {
        var options = PipelineCommandLineParser.Parse(["--rerun-failed", "--no-cache", "Feature:Enabled=true"]);
        var builder = Pipeline.CreateBuilder(["--rerun-failed", "--no-cache"]);

        await Assert.That(options.RerunFailed).IsTrue();
        await Assert.That(options.DisableModuleCache).IsTrue();
        await Assert.That(options.HostArguments).Contains("Feature:Enabled=true");
        await Assert.That(builder.Options.RerunFailed).IsTrue();
        await Assert.That(builder.Options.DisableModuleCache).IsTrue();
    }

    [Test]
    public async Task Disabled_History_Executes_Full_Graph()
    {
        var state = new RunState { Fail = false };
        await RunAsync(state);
        state.Results.ReadCount = 0;

        await RunAsync(state, rerun: true, configure: options => options with
        {
            RunReport = options.RunReport with { HistoryRetention = 0 },
        });

        await Assert.That(state.Count<RootModule>()).IsEqualTo(2);
        await Assert.That(state.Count<FinalModule>()).IsEqualTo(2);
        await Assert.That(state.Results.ReadCount).IsEqualTo(0);
    }

    [Test]
    public async Task Successful_Null_And_Default_Values_Are_Restored()
    {
        var state = new RunState { Fail = false, RootValue = 0 };
        await RunAsync(state, addNullModule: true);

        var summary = await RunAsync(state, rerun: true, addNullModule: true);
        var nullResult = summary.Results.OfType<ModuleResult<string?>>().Single();

        await Assert.That(state.Count<RootModule>()).IsEqualTo(1);
        await Assert.That(state.Count<NullModule>()).IsEqualTo(1);
        await Assert.That(Result<RootModule>(summary).Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
        await Assert.That(Result<RootModule>(summary).ValueOrDefault).IsEqualTo(0);
        await Assert.That(nullResult.Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
        await Assert.That(nullResult.ValueOrDefault).IsNull();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Disabled_Or_Throwing_Result_Repository_Executes_Full_Graph(bool throwOnRead)
    {
        var state = new RunState { Fail = false };
        await RunAsync(state);
        state.Results.IsEnabled = throwOnRead;
        state.Results.ThrowOnRead = throwOnRead;

        var summary = await RunAsync(state, rerun: true);

        await Assert.That(state.Count<RootModule>()).IsEqualTo(2);
        await Assert.That(state.Count<FinalModule>()).IsEqualTo(2);
        await Assert.That(state.Count<IndependentModule>()).IsEqualTo(2);
        await Assert.That(summary.Results.All(result => result.Status == ModuleStatus.Succeeded)).IsTrue();
    }

    [Test]
    public async Task Derived_Pipeline_Identity_Restores_Compatible_History()
    {
        static PipelineOptions WithoutExplicitIdentity(PipelineOptions options) => options with
        {
            RunReport = options.RunReport with { PipelineIdentity = null },
        };

        var state = new RunState { Fail = false };
        var first = await RunAsync(state, configure: WithoutExplicitIdentity);
        var second = await RunAsync(state, rerun: true, configure: WithoutExplicitIdentity);

        await Assert.That(first.RunReport!.PipelineIdentity).IsNotNullOrWhiteSpace();
        await Assert.That(second.RunReport!.PipelineIdentity).IsEqualTo(first.RunReport.PipelineIdentity);
        await Assert.That(state.Count<RootModule>()).IsEqualTo(1);
        await Assert.That(state.Count<FinalModule>()).IsEqualTo(1);
        await Assert.That(Result<RootModule>(second).Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Distributed_Execution_Rejects_Rerun_Failed(bool isWorker)
    {
        var location = new Mock<IExecutionLocationContext>();
        location.SetupGet(context => context.IsMaster).Returns(!isWorker);
        location.SetupGet(context => context.IsWorker).Returns(isWorker);
        var plan = new RerunFailedPlan(
            OptionsFactory.Create(new PipelineOptions { RerunFailed = true }),
            Mock.Of<IRunHistoryStore>(),
            Mock.Of<IDependencyChainProvider>(),
            Mock.Of<IModuleResultHistoryProvider>(),
            Mock.Of<IPipelineContextProvider>(),
            location.Object,
            NullLogger<RerunFailedPlan>.Instance);

        await Assert.That(() => plan.InitializeAsync(new OrganizedModules([], []), CancellationToken.None))
            .Throws<NotSupportedException>();
    }

    private static ModuleResult<int> Result<TModule>(PipelineSummary summary) =>
        summary.Results.OfType<ModuleResult<int>>().Single(result => result.Name == typeof(TModule).Name);

    private static async Task<PipelineSummary> RunAsync(
        RunState state,
        bool rerun = false,
        bool addModule = false,
        Func<PipelineOptions, PipelineOptions>? configure = null,
        TrackingCacheRepository? cache = null,
        bool addNullModule = false)
    {
        var builder = Pipeline.CreateBuilder();
        builder.Configuration["RerunTests:AddDependency"] = state.AddDependency.ToString();
        builder.ConfigureOptions(options =>
        {
            options = options with
            {
                RerunFailed = rerun,
                FailureMode = FailureMode.ContinueOnFailure,
                ThrowOnPipelineFailure = false,
                Console = options.Console with { PrintLogo = false, PrintResults = false },
                RunReport = options.RunReport with
                {
                    AutoWriteInCi = false,
                    PipelineIdentity = "rerun-failed-tests",
                },
            };
            return configure?.Invoke(options) ?? options;
        });
        builder.Services.AddSingleton(state);
        builder.Services.AddSingleton<IModuleResultRepository>(state.Results);
        builder.Services.AddSingleton<IRunHistoryStore>(state.History);
        if (cache is not null)
        {
            builder.Services.AddSingleton<IModuleCacheResultRepository>(cache);
        }

        builder.AddModule<RootModule>();
        builder.AddModule<StableChildModule>();
        builder.AddModule<FlakyModule>();
        builder.AddModule<FinalModule>();
        builder.AddModule<IndependentModule>();
        if (addModule)
        {
            builder.AddModule<AddedModule>();
        }

        if (addNullModule)
        {
            builder.AddModule<NullModule>();
        }

        return await builder.RunAsync();
    }

    private sealed class RunState
    {
        private readonly ConcurrentDictionary<Type, int> _counts = new();

        public bool Fail { get; set; } = true;

        public bool SkipIndependent { get; set; }

        public bool AddDependency { get; set; }

        public int RootValue { get; set; } = 42;

        public ResultRepository Results { get; } = new();

        public HistoryStore History { get; } = new();

        public void Record<TModule>() => _counts.AddOrUpdate(typeof(TModule), 1, (_, count) => count + 1);

        public int Count<TModule>() => _counts.GetValueOrDefault(typeof(TModule));
    }

    private sealed class RootModule(RunState state) : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.Record<RootModule>();
            return Task.FromResult(state.RootValue);
        }
    }

    [DependsOn<RootModule>]
    private sealed class StableChildModule(RunState state) : Module<int>
    {
        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.Record<StableChildModule>();
            return (await context.GetModule<RootModule>()).ValueOrDefault + 10;
        }
    }

    [DependsOn<RootModule>]
    private sealed class FlakyModule(RunState state) : Module<int>
    {
        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.Record<FlakyModule>();
            if (state.Fail)
            {
                throw new InvalidOperationException("Initial run fails");
            }

            return (await context.GetModule<RootModule>()).ValueOrDefault + 1;
        }
    }

    [DependsOn<FlakyModule>]
    private sealed class FinalModule(RunState state) : Module<int>
    {
        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.Record<FinalModule>();
            return (await context.GetModule<FlakyModule>()).ValueOrDefault + 1;
        }
    }

    [ModuleCategory("independent")]
    [AddConfiguredDependency]
    private sealed class IndependentModule(RunState state) : Module<int>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => state.SkipIndependent ? SkipDecision.Skip("Current run condition") : SkipDecision.DoNotSkip);

        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.Record<IndependentModule>();
            return Task.FromResult(7);
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class AddConfiguredDependencyAttribute : Attribute, IModuleRegistrationHandler
    {
        public Task OnRegistrationAsync(IModuleRegistrationContext context, CancellationToken cancellationToken)
        {
            if (context.Configuration["RerunTests:AddDependency"] == bool.TrueString)
            {
                context.AddDependency<StableChildModule>();
            }

            return Task.CompletedTask;
        }
    }

    private sealed class AddedModule(RunState state) : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.Record<AddedModule>();
            return Task.FromResult(9);
        }
    }

    private sealed class NullModule(RunState state) : Module<string?>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            state.Record<NullModule>();
            return Task.FromResult<string?>(null);
        }
    }

    private sealed class ResultRepository : IModuleResultRepository
    {
        public bool IsEnabled { get; set; } = true;

        public bool ThrowOnRead { get; set; }

        public ConcurrentDictionary<Type, object> Values { get; } = new();

        public int ReadCount;

        public Task SaveResultAsync<T>(Module<T> module, ModuleResult<T> moduleResult, IPipelineContext pipelineContext, CancellationToken cancellationToken)
        {
            Values[module.GetType()] = moduleResult;
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(Module<T> module, IPipelineContext pipelineContext, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ReadCount);
            if (ThrowOnRead)
            {
                throw new InvalidDataException("Stored result is unreadable");
            }

            return Task.FromResult(Values.GetValueOrDefault(module.GetType()) as ModuleResult<T>);
        }
    }

    private sealed class HistoryStore : IRunHistoryStore
    {
        public List<PipelineRunReport> Reports { get; } = [];

        public bool ThrowOnRead { get; set; }

        public async IAsyncEnumerable<PipelineRunReport> GetRunsAsync(
            RunHistoryQuery query,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            if (ThrowOnRead)
            {
                throw new InvalidDataException("Corrupt history");
            }

            foreach (var report in Reports.OrderByDescending(report => report.End).Take(query.MaxRuns))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return report;
            }
        }

        public Task SaveAsync(PipelineRunReport report, CancellationToken cancellationToken = default)
        {
            Reports.Add(report);
            return Task.CompletedTask;
        }
    }

    private sealed class TrackingCacheRepository : IModuleCacheResultRepository
    {
        public int ReadCount;

        public int WriteCount;

        public Task SaveResultAsync<T>(Module<T> module, ModuleResult<T> moduleResult, IPipelineContext pipelineContext, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref WriteCount);
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(Module<T> module, IPipelineContext pipelineContext, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ReadCount);
            return Task.FromResult<ModuleResult<T>?>(null);
        }

        public void DiscardFingerprint(IModule module)
        {
        }
    }
}
