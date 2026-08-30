using System.Reflection;
using System.Runtime.CompilerServices;
using CliWrap;
using CommandResult = ModularPipelines.CommandResult;

namespace ModularPipelines.UnitTests.Models;

public class CommandResultTests
{
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
            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StartTime).IsNotEqualTo(default(DateTimeOffset));
            await Assert.That(result.EndTime).IsEqualTo(result.StartTime);
            await Assert.That(result.Duration).IsEqualTo(TimeSpan.Zero);
        }
    }
}
