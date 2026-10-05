using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using CliWrap;
using ModularPipelines.Distributed.Serialization;
using CommandResult = ModularPipelines.CommandResult;

namespace ModularPipelines.UnitTests.Models;

public class CommandResultTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Json_Without_Truncation_Metadata_Defaults_Counts_To_Zero(bool distributedOptions)
    {
        const string json = """
            {
              "CommandInput": "tool --version",
              "EnvironmentVariables": {},
              "WorkingDirectory": ".",
              "StandardOutput": "1.0.0",
              "StandardError": "",
              "ExitCode": 0,
              "StartTime": "2026-10-04T12:00:00+00:00",
              "EndTime": "2026-10-04T12:00:01+00:00",
              "Duration": "00:00:01"
            }
            """;

        var options = distributedOptions ? ModuleResultSerializer.CreateOptions() : new JsonSerializerOptions();
        var result = JsonSerializer.Deserialize<CommandResult>(json, options)!;
        await Assert.That(result.StandardOutput).IsEqualTo("1.0.0");
        await Assert.That(result.StandardOutputTruncatedCharacters).IsEqualTo(0);
        await Assert.That(result.StandardErrorTruncatedCharacters).IsEqualTo(0);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Json_Round_Trip_Preserves_Truncation_Metadata(bool distributedOptions)
    {
        var original = CommandResult.Ok("output", "error") with
        {
            StandardOutputTruncatedCharacters = 12,
            StandardErrorTruncatedCharacters = 34,
        };

        var options = distributedOptions ? ModuleResultSerializer.CreateOptions() : new JsonSerializerOptions();
        var result = JsonSerializer.Deserialize<CommandResult>(JsonSerializer.Serialize(original, options), options)!;
        await Assert.That(result.StandardOutputTruncatedCharacters).IsEqualTo(12);
        await Assert.That(result.StandardErrorTruncatedCharacters).IsEqualTo(34);
    }

    [Test]
    public async Task Public_Result_Data_Is_Required()
    {
        var nonRequiredProperties = typeof(CommandResult)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(static property => property.GetCustomAttribute<RequiredMemberAttribute>() is null)
            .Select(static property => property.Name)
            .ToArray();

        await Assert.That(nonRequiredProperties).IsEmpty();
    }

    [Test]
    public async Task Dry_Run_Result_Contains_Completed_Result_Data()
    {
        var result = new CommandResult(
            Cli.Wrap("tool"),
            "tool",
            new Dictionary<string, string?>());

        using (Assert.Multiple())
        {
            await Assert.That(result.StandardOutput).IsEqualTo(string.Empty);
            await Assert.That(result.StandardError).IsEqualTo(string.Empty);
            await Assert.That(result.StandardOutputTruncatedCharacters).IsEqualTo(0);
            await Assert.That(result.StandardErrorTruncatedCharacters).IsEqualTo(0);
            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StartTime).IsNotEqualTo(default(DateTimeOffset));
            await Assert.That(result.EndTime).IsEqualTo(result.StartTime);
            await Assert.That(result.Duration).IsEqualTo(TimeSpan.Zero);
        }
    }
}
