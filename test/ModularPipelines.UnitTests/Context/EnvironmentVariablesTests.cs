using ModularPipelines.Context;

namespace ModularPipelines.UnitTests.Context;

// PATH changes must not overlap tests that start child processes.
[TUnit.Core.NotInParallel]
public class EnvironmentVariablesTests
{
    [Test]
    [Arguments(null)]
    [Arguments("")]
    public async Task AddToPath_Does_Not_Create_Empty_Entries(string? initialPath)
    {
        await WithPathRestored(async () =>
        {
            Environment.SetEnvironmentVariable("PATH", initialPath);
            new EnvironmentVariables().AddToPath("new-tool");

            await Assert.That(Environment.GetEnvironmentVariable("PATH")).IsEqualTo("new-tool");
        });
    }

    [Test]
    public async Task AddToPath_Removes_Empty_Entries_And_Duplicates()
    {
        await WithPathRestored(async () =>
        {
            var separator = Path.PathSeparator;
            Environment.SetEnvironmentVariable("PATH", $"{separator}first{separator}{separator}second{separator}first{separator}");
            IEnvironmentVariablesContext variables = new EnvironmentVariables();
            variables.AddToPath($"second{separator}{separator}third{separator}", EnvironmentVariableTarget.Process);

            await Assert.That(Environment.GetEnvironmentVariable("PATH"))
                .IsEqualTo($"first{separator}second{separator}third");
        });
    }

    [Test]
    public async Task AddToPath_Uses_Platform_Path_Comparison()
    {
        await WithPathRestored(async () =>
        {
            Environment.SetEnvironmentVariable("PATH", "tool");
            new EnvironmentVariables().AddToPath("TOOL");

            await Assert.That(Environment.GetEnvironmentVariable("PATH"))
                .IsEqualTo(OperatingSystem.IsWindows() ? "tool" : $"tool{Path.PathSeparator}TOOL");
        });
    }

    [Test]
    [Arguments("first", "first;second;third", false)]
    [Arguments("first", "first;second;third", true)]
    [Arguments("second", "second;first;third", false)]
    [Arguments("second", "second;first;third", true)]
    [Arguments("new-tool", "new-tool;first;second;third", false)]
    [Arguments("new-tool", "new-tool;first;second;third", true)]
    public async Task AddToPath_Can_Prepend_And_Move_Existing_Paths(string pathToAdd, string expected, bool explicitTarget)
    {
        await WithPathRestored(async () =>
        {
            Environment.SetEnvironmentVariable("PATH", string.Join(Path.PathSeparator, "first", "second", "third"));
            IEnvironmentVariablesContext variables = new EnvironmentVariables();

            if (explicitTarget)
            {
                variables.AddToPath(pathToAdd, prepend: true, EnvironmentVariableTarget.Process);
            }
            else
            {
                variables.AddToPath(pathToAdd, prepend: true);
            }

            await Assert.That(Environment.GetEnvironmentVariable("PATH"))
                .IsEqualTo(expected.Replace(';', Path.PathSeparator));
        });
    }

    [Test]
    public async Task AddToPath_Empty_Input_Does_Not_Add_Current_Directory()
    {
        await WithPathRestored(async () =>
        {
            Environment.SetEnvironmentVariable("PATH", "tool");
            new EnvironmentVariables().AddToPath(string.Empty);

            await Assert.That(Environment.GetEnvironmentVariable("PATH")).IsEqualTo("tool");
        });
    }

    [Test]
    public async Task GetPath_Excludes_Empty_Entries()
    {
        await WithPathRestored(async () =>
        {
            Environment.SetEnvironmentVariable("PATH", $"{Path.PathSeparator}tool{Path.PathSeparator}");

            await Assert.That(new EnvironmentVariables().GetPath().SequenceEqual(["tool"])).IsTrue();
        });
    }

    [Test]
    public async Task AddToPath_Preserves_Concurrent_Updates_Across_Instances()
    {
        await WithPathRestored(async () =>
        {
            Environment.SetEnvironmentVariable("PATH", null);
            var entries = Enumerable.Range(0, 100).Select(index => $"tool-{index}").ToArray();

            Parallel.ForEach(entries, entry => new EnvironmentVariables().AddToPath(entry));

            var actual = new EnvironmentVariables().GetPath();
            await Assert.That(actual.Count).IsEqualTo(entries.Length);
            await Assert.That(actual.Order(StringComparer.Ordinal).SequenceEqual(entries.Order(StringComparer.Ordinal))).IsTrue();
        });
    }

    [Test]
    public async Task GetAll_Uses_Platform_Environment_Name_Comparison()
    {
        var name = $"MP_ENV_{Guid.NewGuid():N}";
        try
        {
            Environment.SetEnvironmentVariable(name, "value");
            var variables = new EnvironmentVariables().GetAll();

            await Assert.That(variables[name]).IsEqualTo("value");
            await Assert.That(variables.ContainsKey(name.ToLowerInvariant())).IsEqualTo(OperatingSystem.IsWindows());
        }
        finally
        {
            Environment.SetEnvironmentVariable(name, null);
        }
    }

    private static async Task WithPathRestored(Func<Task> action)
    {
        var originalPath = Environment.GetEnvironmentVariable("PATH");
        try
        {
            await action();
        }
        finally
        {
            Environment.SetEnvironmentVariable("PATH", originalPath);
        }
    }
}
