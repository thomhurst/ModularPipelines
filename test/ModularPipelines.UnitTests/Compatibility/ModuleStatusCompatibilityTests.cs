using System.Text.Json;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.UnitTests.Compatibility;

public class ModuleStatusCompatibilityTests
{
    [Test]
    [Arguments(ModuleStatus.NotStarted, 0)]
    [Arguments(ModuleStatus.Running, 1)]
    [Arguments(ModuleStatus.Succeeded, 2)]
    [Arguments(ModuleStatus.Failed, 3)]
    [Arguments(ModuleStatus.FailureIgnored, 4)]
    [Arguments(ModuleStatus.Skipped, 5)]
    [Arguments(ModuleStatus.TimedOut, 6)]
    [Arguments(ModuleStatus.Cancelled, 7)]
    [Arguments(ModuleStatus.DependencyFailed, 8)]
    [Arguments(ModuleStatus.RestoredFromHistory, 9)]
    [Arguments(ModuleStatus.RestoredFromCache, 10)]
    [Arguments(ModuleStatus.Unknown, 11)]
    public async Task V4ValuesAreSequential(ModuleStatus status, int expectedValue)
    {
        await Assert.That((int) status).IsEqualTo(expectedValue);
    }

    [Test]
    public async Task ModuleResultUsesStatusProperty()
    {
        await Assert.That(typeof(IModuleResult).GetProperty(nameof(IModuleResult.Status))).IsNotNull();
        await Assert.That(typeof(IModuleResult).GetProperty("ModuleStatus")).IsNull();
    }

    [Test]
    [Arguments("NotYetStarted", ModuleStatus.NotStarted)]
    [Arguments("Processing", ModuleStatus.Running)]
    [Arguments("Successful", ModuleStatus.Succeeded)]
    [Arguments("UsedHistory", ModuleStatus.RestoredFromHistory)]
    [Arguments("Failed", ModuleStatus.Failed)]
    [Arguments("IgnoredFailure", ModuleStatus.FailureIgnored)]
    [Arguments("PipelineTerminated", ModuleStatus.Cancelled)]
    [Arguments("TimedOut", ModuleStatus.TimedOut)]
    [Arguments("Skipped", ModuleStatus.Skipped)]
    [Arguments("Unknown", ModuleStatus.Unknown)]
    [Arguments("CachedResult", ModuleStatus.RestoredFromCache)]
    [Arguments("DependencyFailed", ModuleStatus.DependencyFailed)]
    public async Task RunHistoryReaderMapsV3StatusNames(string legacyName, ModuleStatus expected)
    {
        var json = $$"""
                     {
                       "status": "{{legacyName}}",
                       "modules": [{ "status": "{{legacyName}}" }]
                     }
                     """;

        var report = RunReportJsonSerializer.Deserialize(json);

        await Assert.That(report!.Status).IsEqualTo(expected);
        await Assert.That(report.Modules[0].Status).IsEqualTo(expected);
    }

    [Test]
    public async Task RunHistoryWriterUsesV4StatusNames()
    {
        var report = new PipelineRunReport
        {
            Status = ModuleStatus.Succeeded,
            Modules = [new ModuleRunReport { Status = ModuleStatus.RestoredFromHistory }],
        };

        var json = RunReportJsonSerializer.Serialize(report);

        await Assert.That(json).Contains("Succeeded");
        await Assert.That(json).Contains("RestoredFromHistory");
        await Assert.That(json).DoesNotContain("Successful");
        await Assert.That(json).DoesNotContain("UsedHistory");
    }

    [Test]
    public async Task RunHistoryReaderRejectsUndefinedNumericStatus()
    {
        const string json = """
                            {
                              "status": "999",
                              "modules": []
                            }
                            """;

        await Assert.That(() => RunReportJsonSerializer.Deserialize(json))
            .Throws<JsonException>();
    }

    [Test]
    public async Task NonGenericModuleResultReadsLegacyModuleStatusProperty()
    {
        ModuleResult result = new ModuleResult.Failure(new InvalidOperationException("Failed"))
        {
            ModuleName = "LegacyModule",
            ModuleDuration = TimeSpan.Zero,
            ModuleStart = DateTimeOffset.MinValue,
            ModuleEnd = DateTimeOffset.MinValue,
            Status = ModuleStatus.Failed,
        };
        var legacyJson = RenameStatusProperty(JsonSerializer.Serialize(result));

        var deserialized = JsonSerializer.Deserialize<ModuleResult>(legacyJson);

        await Assert.That(deserialized!.Status).IsEqualTo(ModuleStatus.Failed);
    }

    [Test]
    public async Task GenericModuleResultReadsLegacyModuleStatusProperty()
    {
        ModuleResult<int> result = new ModuleResult<int>.Success(42)
        {
            ModuleName = "LegacyModule",
            ModuleDuration = TimeSpan.Zero,
            ModuleStart = DateTimeOffset.MinValue,
            ModuleEnd = DateTimeOffset.MinValue,
            Status = ModuleStatus.Succeeded,
        };
        var legacyJson = RenameStatusProperty(JsonSerializer.Serialize(result));

        var deserialized = JsonSerializer.Deserialize<ModuleResult<int>>(legacyJson);

        await Assert.That(deserialized!.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    private static string RenameStatusProperty(string json)
    {
        return json.Replace("\"Status\":", "\"ModuleStatus\":", StringComparison.Ordinal);
    }
}
