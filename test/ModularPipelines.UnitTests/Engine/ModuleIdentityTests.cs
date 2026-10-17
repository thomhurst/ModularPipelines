using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Reporting;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleIdentityTests
{
    [ModuleId("build:linux/../release")]
    private sealed class OriginalModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("output");
    }

    [ModuleId("build:linux/../release")]
    private sealed class RenamedModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("output");
    }

    private abstract class IdentityModule : Module
    {
        protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }

    [ModuleId("a\nb")]
    private sealed class FirstNewlineModule : IdentityModule;

    [ModuleId("c")]
    private sealed class LastModule : IdentityModule;

    [ModuleId("a")]
    private sealed class FirstModule : IdentityModule;

    [ModuleId("b\nc")]
    private sealed class LastNewlineModule : IdentityModule;

    [Test]
    public async Task DefaultPipelineIdentityDistinguishesNewlinesInsideModuleIds()
    {
        var first = PipelineRunCompatibility.GetPipelineIdentity(null, [new FirstNewlineModule(), new LastModule()]);
        var second = PipelineRunCompatibility.GetPipelineIdentity(null, [new FirstModule(), new LastNewlineModule()]);

        await Assert.That(first).IsNotEqualTo(second);
        await Assert.That(PipelineRunCompatibility.GetPipelineIdentity(null, [new LastModule(), new FirstNewlineModule()]))
            .IsEqualTo(first);
    }

    [Test]
    public async Task EnginePassesConfiguredIdentityToEstimateProvider()
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModule<OriginalModule>()
            .AddModuleEstimatedTimeProvider<RecordingProvider>()
            .BuildAsync();

        var summary = await host.RunAsync();
        var report = new PipelineRunReportFactory(
            Mock.Of<ICommandExecutionCounter>(),
            Mock.Of<ModularPipelines.Secrets.ISecretObfuscator>())
            .Create(summary, null, "identity-test");

        var provider = (RecordingProvider) host.Services.GetRequiredService<IModuleEstimatedTimeProvider>();
        var id = ModuleId.FromType(typeof(OriginalModule));
        await Assert.That(report.Modules.Single().ModuleId).IsEqualTo(id);
        await Assert.That(provider.Requested).IsNotEmpty();
        await Assert.That(provider.Requested.All(candidate => candidate == id)).IsTrue();
        await Assert.That(provider.Saved).IsEquivalentTo([id]);
    }

    [Test]
    public async Task RenamedModuleUsesPreviousDurationWithSameIdentity()
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModule<RenamedModule>()
            .BuildAsync();
        var summary = await host.RunAsync();
        var previous = new PipelineRunReport
        {
            Modules = [new ModuleRunReport
            {
                ModuleId = ModuleId.FromType(typeof(OriginalModule)),
                Status = ModuleStatus.Succeeded,
                DurationMeasured = true,
                Duration = TimeSpan.FromSeconds(7),
            }],
        };
        var factory = new PipelineRunReportFactory(
            Mock.Of<ICommandExecutionCounter>(),
            Mock.Of<ModularPipelines.Secrets.ISecretObfuscator>());

        var report = factory.Create(summary, previous, "identity-test");

        await Assert.That(report.Modules.Single().PreviousDuration).IsEqualTo(TimeSpan.FromSeconds(7));
    }

    [Test]
    public async Task RenamedModuleLoadsPreviousRunUsingDefaultPipelineIdentity()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"module-identity-{Guid.NewGuid():N}");
        try
        {
            var runReportOptions = new RunReportOptions
            {
                AutoWriteInCi = false,
                HistoryDirectory = directory,
                HistoryRetention = 2,
            };
            await using var originalHost = await TestPipelineBuilder.Create()
                .AddModule<OriginalModule>()
                .ConfigureOptions(options => options with { RunReport = runReportOptions })
                .BuildAsync();
            var original = (await originalHost.RunAsync()).RunReport!;

            await using var renamedHost = await TestPipelineBuilder.Create()
                .AddModule<RenamedModule>()
                .ConfigureOptions(options => options with { RunReport = runReportOptions })
                .BuildAsync();
            var renamed = (await renamedHost.RunAsync()).RunReport!;

            await Assert.That(renamed.PipelineIdentity).IsEqualTo(original.PipelineIdentity);
            await Assert.That(renamed.PreviousEnd).IsEqualTo(original.End);
            await Assert.That(renamed.Modules.Single().PreviousDuration)
                .IsEqualTo(original.Modules.Single().Duration);
        }
        finally
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
    }

    [Test]
    public async Task RenamedModuleSharesEstimatesIncludingSubModules()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"module-identity-{Guid.NewGuid():N}");
        try
        {
            var provider = new FileSystemModuleEstimatedTimeProvider(directory, TimeProvider.System);
            var original = ModuleId.FromType(typeof(OriginalModule));
            var renamed = ModuleId.FromType(typeof(RenamedModule));
            await provider.SaveModuleTimeAsync(original, TimeSpan.FromSeconds(7));
            await provider.SaveSubModuleTimeAsync(original, new SubModuleEstimation("compile", TimeSpan.FromSeconds(3)));

            await Assert.That(await provider.GetModuleEstimatedTimeAsync(renamed)).IsEqualTo(TimeSpan.FromSeconds(7));
            var subModules = (await provider.GetSubModuleEstimatedTimesAsync(renamed)).ToArray();
            await Assert.That(subModules).IsEquivalentTo([new SubModuleEstimation("compile", TimeSpan.FromSeconds(3))]);
            await Assert.That(Directory.GetFiles(directory)).Count().IsEqualTo(2);
            await Assert.That(Directory.GetDirectories(directory)).IsEmpty();
        }
        finally
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
    }

    [Test]
    public async Task RenamedModuleQueriesHistoryUsingSerializedIdentity()
    {
        var id = ModuleId.FromType(typeof(OriginalModule));
        var report = new PipelineRunReport
        {
            RunId = "previous-run",
            Modules = [new ModuleRunReport
            {
                ModuleId = id,
                Status = ModuleStatus.Succeeded,
                Duration = TimeSpan.FromSeconds(7),
                DurationMeasured = true,
            }],
        };
        var restored = RunReportJsonSerializer.Deserialize(RunReportJsonSerializer.Serialize(report))!;
        var store = new Mock<IRunHistoryStore>();
        store.Setup(value => value.GetRunsAsync(It.IsAny<RunHistoryQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Reports(restored));
        var reader = new RunHistoryReader(store.Object, Microsoft.Extensions.Options.Options.Create(new PipelineOptions
        {
            RunReport = new RunReportOptions { PipelineIdentity = "identity-test" },
        }));

        var samples = await reader.GetModuleDurationTrendAsync(ModuleId.FromType(typeof(RenamedModule)), 1);

        await Assert.That(restored.Modules.Single().ModuleId).IsEqualTo(id);
        await Assert.That(samples).HasSingleItem();
        await Assert.That(samples[0].Duration).IsEqualTo(TimeSpan.FromSeconds(7));
        await Assert.That(async () => await reader.GetModuleDurationTrendAsync(default, 1)).Throws<ArgumentException>();
    }

    [Test]
    public async Task StringConversionRequiresExplicitValidation()
    {
        var id = (ModuleId) "build";
        await Assert.That((string) id).IsEqualTo("build");
        await Assert.That(() => (ModuleId) " ").Throws<ArgumentException>();
        await Assert.That(typeof(ModuleId).GetMethods().Any(method =>
            method.Name == "op_Implicit" && method.ReturnType == typeof(ModuleId))).IsFalse();
        await Assert.That(JsonSerializer.Deserialize<ModuleId>(JsonSerializer.Serialize(id))).IsEqualTo(id);
    }

    private static async IAsyncEnumerable<PipelineRunReport> Reports(PipelineRunReport report)
    {
        await Task.CompletedTask;
        yield return report;
    }

    public sealed class RecordingProvider : IModuleEstimatedTimeProvider
    {
        public ConcurrentBag<ModuleId> Requested { get; } = [];
        public ConcurrentBag<ModuleId> Saved { get; } = [];

        public Task<TimeSpan> GetModuleEstimatedTimeAsync(ModuleId moduleId, CancellationToken cancellationToken = default)
        {
            Requested.Add(moduleId);
            return Task.FromResult(TimeSpan.FromSeconds(1));
        }

        public Task SaveModuleTimeAsync(ModuleId moduleId, TimeSpan duration, CancellationToken cancellationToken = default)
        {
            Saved.Add(moduleId);
            return Task.CompletedTask;
        }
    }
}
