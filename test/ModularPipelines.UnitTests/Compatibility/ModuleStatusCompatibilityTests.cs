using ModularPipelines.Reporting;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    [Arguments(ModuleStatus.Canceled, 7)]
    [Arguments(ModuleStatus.DependencyFailed, 8)]
    [Arguments(ModuleStatus.RestoredFromHistory, 9)]
    [Arguments(ModuleStatus.RestoredFromCache, 10)]
    [Arguments(ModuleStatus.Unknown, 11)]
    public async Task V4ValuesAreSequential(ModuleStatus status, int expectedValue)
    {
        await Assert.That((int) status).IsEqualTo(expectedValue);
    }

    [Test]
    public async Task CanceledStatusUsesUsSpelling()
    {
        await Assert.That(Enum.GetNames<ModuleStatus>()).DoesNotContain("Cancelled");
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
    [Arguments("PipelineTerminated", ModuleStatus.Canceled)]
    [Arguments("Cancelled", ModuleStatus.Canceled)]
    [Arguments("Canceled", ModuleStatus.Canceled)]
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
            Name = "LegacyModule",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.MinValue,
            EndTime = DateTimeOffset.MinValue,
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
            Name = "LegacyModule",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.MinValue,
            EndTime = DateTimeOffset.MinValue,
            Status = ModuleStatus.Succeeded,
        };
        var legacyJson = RenameStatusProperty(JsonSerializer.Serialize(result));

        var deserialized = JsonSerializer.Deserialize<ModuleResult<int>>(legacyJson);

        await Assert.That(deserialized!.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    [Arguments("Cancelled")]
    [Arguments("PipelineTerminated")]
    public async Task ModuleResultReadsPreviousCanceledStatusNames(string persistedName)
    {
        ModuleResult result = new ModuleResult.Failure(new OperationCanceledException("Canceled"))
        {
            Name = "PersistedModule",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.MinValue,
            EndTime = DateTimeOffset.MinValue,
            Status = ModuleStatus.Canceled,
        };
        var json = JsonSerializer.Serialize(result)
            .Replace("\"Status\":\"Canceled\"", $"\"Status\":\"{persistedName}\"", StringComparison.Ordinal);

        await Assert.That(json).Contains(persistedName);

        var deserialized = JsonSerializer.Deserialize<ModuleResult>(json);

        await Assert.That(deserialized!.Status).IsEqualTo(ModuleStatus.Canceled);
    }

    [Test]
    public async Task ModuleResultRoundTripsStatusWithConfiguredEnumConverter()
    {
        ModuleResult result = new ModuleResult.Failure(new OperationCanceledException("Canceled"))
        {
            Name = "Module",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.MinValue,
            EndTime = DateTimeOffset.MinValue,
            Status = ModuleStatus.Canceled,
        };
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        };

        var json = JsonSerializer.Serialize(result, options);

        await Assert.That(json).Contains("\"Status\":\"canceled\"");

        var deserialized = JsonSerializer.Deserialize<ModuleResult>(json, options);

        await Assert.That(deserialized!.Status).IsEqualTo(ModuleStatus.Canceled);
    }

    [Test]
    public async Task ModuleResultReadsDifferentlyCasedStatusName()
    {
        ModuleResult result = new ModuleResult.Failure(new InvalidOperationException("Failed"))
        {
            Name = "Module",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.MinValue,
            EndTime = DateTimeOffset.MinValue,
            Status = ModuleStatus.Failed,
        };
        var json = JsonSerializer.Serialize(result)
            .Replace("\"Status\":\"Failed\"", "\"Status\":\"failed\"", StringComparison.Ordinal);

        await Assert.That(json).Contains("\"Status\":\"failed\"");

        var deserialized = JsonSerializer.Deserialize<ModuleResult>(json);

        await Assert.That(deserialized!.Status).IsEqualTo(ModuleStatus.Failed);
    }

    [Test]
    public async Task ModuleResultWritesCanceledStatusName()
    {
        ModuleResult result = new ModuleResult.Failure(new OperationCanceledException("Stopped"))
        {
            Name = "Module",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.MinValue,
            EndTime = DateTimeOffset.MinValue,
            Status = ModuleStatus.Canceled,
        };

        var json = JsonSerializer.Serialize(result);

        await Assert.That(json).Contains("\"Status\":\"Canceled\"");
        await Assert.That(json).DoesNotContain("Cancelled");
    }

    private static string RenameStatusProperty(string json)
    {
        return json.Replace("\"Status\":", "\"ModuleStatus\":", StringComparison.Ordinal);
    }
}
