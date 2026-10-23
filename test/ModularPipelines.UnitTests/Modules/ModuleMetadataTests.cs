using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Reporting;
using ModularPipelines.Secrets;
using Moq;

namespace ModularPipelines.UnitTests.Modules;

public class ModuleMetadataTests
{
    [Test]
    public async Task ConfiguredIdentityMatchesAcrossResultsPlansTimelinesAndReports()
    {
        var module = new IdentifiedModule();
        var start = DateTimeOffset.UtcNow;
        var end = start.AddSeconds(2);
        var context = new ModuleExecutionContext<int>(module, typeof(IdentifiedModule))
        {
            StartTime = start,
            EndTime = end,
            Status = ModuleStatus.Succeeded,
        };
        try
        {
            var result = ModuleResult<int>.CreateSuccess(42, context);
            var plan = new PipelinePlanModule(module, null, SkipDecision.DoNotSkip, end - start, false);
            var collector = new MetricsCollector();
            collector.RecordModuleStarted(typeof(IdentifiedModule), start);
            collector.RecordModuleCompleted(typeof(IdentifiedModule), end, true, false, ModuleStatus.Succeeded);
            var timeline = collector.GetTimelines().Single();
            var summary = new PipelineSummary([module], [result], end - start, start, end,
                moduleTimelines: [timeline]);
            var report = new PipelineRunReportFactory(Mock.Of<ICommandExecutionCounter>(), new PassthroughSecretObfuscator())
                .Create(summary, null, "metadata-test").Modules.Single();

            foreach (var id in new[] { ((IModuleResult) result).ModuleId, plan.ModuleId, timeline.ModuleId, report.ModuleId })
            {
                await Assert.That(id).IsEqualTo(new ModuleId("metadata-test-module"));
            }

            await Assert.That(plan.Name).IsEqualTo(result.Name);
            await Assert.That(plan.TypeName).IsEqualTo(result.TypeName);
            await Assert.That(timeline.Name).IsEqualTo(result.Name);
            await Assert.That(report.Name).IsEqualTo(result.Name);
            await Assert.That(report.StartTime).IsEqualTo(result.StartTime);
            await Assert.That(report.EndTime).IsEqualTo(result.EndTime);
            await Assert.That(report.ResultStartTime).IsEqualTo(result.StartTime);
            await Assert.That(report.ResultEndTime).IsEqualTo(result.EndTime);
        }
        finally
        {
            context.ModuleCancellationTokenSource.Dispose();
        }
    }

    [Test]
    public async Task AllOutcomeFactoriesAndConversionsPreserveIdentityThroughJson()
    {
        var module = new IdentifiedModule();
        var context = new ModuleExecutionContext<int>(module, typeof(IdentifiedModule));
        try
        {
            var failure = ModuleResult.CreateFailure(new InvalidOperationException("failure"), context);
            var skipped = ModuleResult.CreateSkipped(SkipDecision.Skip("skip"), context);
            ModuleResult<int> typedFailure = failure;
            ModuleResult<int> typedSkipped = skipped;
            ModuleResult[] results =
            [
                failure, skipped, typedFailure, typedSkipped,
                ModuleResult<int>.CreateSuccess(42, context),
                ModuleResult<int>.CreateFailure(new InvalidOperationException("failure"), context),
                ModuleResult<int>.CreateSkipped(SkipDecision.Skip("skip"), context),
                typedFailure.ToSuccess(42),
                typedSkipped.ToFailure(new InvalidOperationException("replacement")),
            ];

            foreach (var result in results)
            {
                var json = result is ModuleResult<int> typedResult
                    ? JsonSerializer.Serialize(typedResult)
                    : JsonSerializer.Serialize(result);
                var restored = result is ModuleResult<int>
                    ? (ModuleResult) JsonSerializer.Deserialize<ModuleResult<int>>(json)!
                    : JsonSerializer.Deserialize<ModuleResult>(json)!;
                await Assert.That(result.ModuleId).IsEqualTo(ModuleId.FromType(typeof(IdentifiedModule)));
                await Assert.That(restored.ModuleId).IsEqualTo(result.ModuleId);
            }
        }
        finally
        {
            context.ModuleCancellationTokenSource.Dispose();
        }
    }

    [Test]
    public async Task LegacyHistoryReceivesIdentityFromTheRequestedModule()
    {
        var module = new IdentifiedModule();
        var start = DateTimeOffset.UtcNow;
        var legacy = new ModuleResult<int>.Success(42)
        {
            Name = nameof(IdentifiedModule),
            StartTime = start,
            EndTime = start,
            Duration = TimeSpan.Zero,
            Status = ModuleStatus.Succeeded,
        };
        var pipelineContext = Mock.Of<IPipelineContext>();
        var repository = new Mock<IModuleResultRepository>();
        repository.Setup(repository => repository.IsEnabled).Returns(true);
        repository.Setup(repository => repository.GetResultAsync(module, pipelineContext, It.IsAny<CancellationToken>()))
            .ReturnsAsync(legacy);
        var provider = new ModuleResultHistoryProvider(repository.Object, NullLogger<ModuleResultHistoryProvider>.Instance);

        var restored = await provider.TryGetAsync(module, pipelineContext);

        await Assert.That(restored!.ModuleId).IsEqualTo(new ModuleId("metadata-test-module"));
        await Assert.That(restored.StartTime).IsEqualTo(legacy.StartTime);
        await Assert.That(legacy.ModuleId).IsEqualTo(default(ModuleId));
        using var document = JsonDocument.Parse(JsonSerializer.Serialize<IModuleResult>(legacy));
        await Assert.That(document.RootElement.TryGetProperty("ModuleId", out _)).IsFalse();
    }

    [Test]
    public async Task AlignedReportNamesKeepExistingHistoryKeys()
    {
        const string json = """
            { "modules": [{ "moduleName": "old-name", "moduleTypeName": "configured-id",
              "start": "2026-10-01T00:00:00+00:00", "end": "2026-10-01T00:00:02+00:00" }] }
            """;
        var report = RunReportJsonSerializer.Deserialize(json)!;
        var module = report.Modules.Single();
        var serialized = RunReportJsonSerializer.Serialize(report);
        var roundTrip = RunReportJsonSerializer.Deserialize(serialized)!.Modules.Single();

        await Assert.That(module.Name).IsEqualTo("old-name");
        await Assert.That(module.ModuleId).IsEqualTo(new ModuleId("configured-id"));
        await Assert.That(module.EndTime - module.StartTime).IsEqualTo(TimeSpan.FromSeconds(2));
        await Assert.That(roundTrip).IsEqualTo(module);
        using var document = JsonDocument.Parse(serialized);
        var payload = document.RootElement.GetProperty("modules")[0];
        await Assert.That(payload.TryGetProperty("moduleName", out _)).IsTrue();
        await Assert.That(payload.TryGetProperty("start", out _)).IsTrue();
        await Assert.That(payload.TryGetProperty("end", out _)).IsTrue();
    }

    [Test]
    [Arguments("default", false)]
    [Arguments("camel", false)]
    [Arguments("snake", false)]
    [Arguments("kebab", false)]
    [Arguments("default", true)]
    [Arguments("camel", true)]
    [Arguments("snake", true)]
    [Arguments("kebab", true)]
    public async Task TimelineNamesKeepExistingJsonKeys(string namingPolicy, bool sourceGenerated)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = namingPolicy switch
            {
                "camel" => JsonNamingPolicy.CamelCase,
                "snake" => JsonNamingPolicy.SnakeCaseLower,
                "kebab" => JsonNamingPolicy.KebabCaseLower,
                _ => null,
            },
        };
        // Match the pre-rename contract with the caller's case-sensitive naming options.
        var json = JsonSerializer.Serialize(new { ModuleName = "old-name", ModuleTypeName = "old-type" }, options);
        options = new JsonSerializerOptions(options);
        if (sourceGenerated)
        {
            options.TypeInfoResolver = TimelineTestJsonContext.Default;
        }

        var timeline = JsonSerializer.Deserialize<ModuleTimeline>(json, options)!;
        await Assert.That(timeline.Name).IsEqualTo("old-name");
        await Assert.That(timeline.TypeName).IsEqualTo("old-type");
        var serialized = JsonSerializer.Serialize(timeline, options);
        var restored = JsonSerializer.Deserialize<ModuleTimeline>(serialized, options)!;
        await Assert.That(restored.Name).IsEqualTo("old-name");
        await Assert.That(restored.TypeName).IsEqualTo("old-type");
        await Assert.That(restored.ModuleId).IsEqualTo(default(ModuleId));
        using var legacy = JsonDocument.Parse(json);
        using var current = JsonDocument.Parse(serialized);
        foreach (var property in legacy.RootElement.EnumerateObject())
        {
            await Assert.That(current.RootElement.GetProperty(property.Name).GetString()).IsEqualTo(property.Value.GetString());
        }

        var nameKey = options.PropertyNamingPolicy?.ConvertName(nameof(ModuleTimeline.Name)) ?? nameof(ModuleTimeline.Name);
        var typeNameKey = options.PropertyNamingPolicy?.ConvertName(nameof(ModuleTimeline.TypeName)) ?? nameof(ModuleTimeline.TypeName);
        await Assert.That(current.RootElement.TryGetProperty(nameKey, out _)).IsFalse();
        await Assert.That(current.RootElement.TryGetProperty(typeNameKey, out _)).IsFalse();
    }

    [Test]
    public async Task TimelineJsonPreservesIdentityAndLifecycleMetadataWithoutReflection()
    {
        var start = DateTimeOffset.Parse("2026-10-01T00:00:00+00:00");
        var timeline = new ModuleTimeline
        {
            Name = "module-name",
            TypeName = "module-type",
            ModuleId = new ModuleId("configured-id"),
            Priority = ModulePriority.High,
            ExecutionHint = ExecutionHint.IoBound,
            ReadyTime = start,
            QueuedTime = start.AddSeconds(1),
            StartTime = start.AddSeconds(2),
            EndTime = start.AddSeconds(5),
            DependencyWaitTime = TimeSpan.FromSeconds(1),
            QueueWaitTime = TimeSpan.FromSeconds(2),
            ExecutionDuration = TimeSpan.FromSeconds(3),
            WasSkipped = true,
            WasSuccessful = true,
            Status = ModuleStatus.Succeeded,
        };
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            TypeInfoResolver = TimelineTestJsonContext.Default,
        };

        var serialized = JsonSerializer.Serialize(timeline, options);
        var restored = JsonSerializer.Deserialize<ModuleTimeline>(serialized, options);

        await Assert.That(restored).IsEqualTo(timeline);
    }

    private sealed class PassthroughSecretObfuscator : ISecretObfuscator
    {
        public string Obfuscate(string? input, object? optionsObject) => input ?? string.Empty;
    }

    [ModularPipelines.ModuleId("metadata-test-module")]
    private sealed class IdentifiedModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(42);
    }
}

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ModuleTimeline))]
internal sealed partial class TimelineTestJsonContext : JsonSerializerContext;
