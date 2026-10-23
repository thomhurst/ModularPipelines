namespace ModularPipelines.UnitTests.Models;

public class CommandLineTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task CommandLine_EqualValues_HaveValueEquality(bool emptyArguments)
    {
        var first = new CommandLine("git", emptyArguments ? [] : ["status", "--short"]);
        var second = new CommandLine("git", emptyArguments ? [] : ["status", "--short"]);

        using (Assert.Multiple())
        {
            await Assert.That(first).IsEqualTo(second);
            await Assert.That(first.Equals((object) second)).IsTrue();
            await Assert.That(first == second).IsTrue();
            await Assert.That(first != second).IsFalse();
            await Assert.That(first.GetHashCode()).IsEqualTo(second.GetHashCode());
            await Assert.That(new HashSet<CommandLine> { first, second }).Count().IsEqualTo(1);
        }
    }

    [Test]
    [Arguments("Git", "status", "--short")]
    [Arguments("git", "Status", "--short")]
    [Arguments("git", "--short", "status")]
    [Arguments("git", "status", "--long")]
    [Arguments("git", "status", "")]
    public async Task CommandLine_DifferentValues_AreNotEqual(string tool, string firstArgument, string secondArgument)
    {
        var first = new CommandLine("git", ["status", "--short"]);
        var second = new CommandLine(tool, [firstArgument, secondArgument]);

        using (Assert.Multiple())
        {
            await Assert.That(first.Equals(second)).IsFalse();
            await Assert.That(first.Equals((object) second)).IsFalse();
            await Assert.That(first == second).IsFalse();
            await Assert.That(first != second).IsTrue();
        }
    }

    [Test]
    public async Task CommandLine_ArgumentCount_IsPartOfEquality()
    {
        var first = new CommandLine("git", ["status"]);
        var second = new CommandLine("git", ["status", "--short"]);

        await Assert.That(first.Equals(second)).IsFalse();
        await Assert.That(second.Equals(first)).IsFalse();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task CommandLine_Null_IsNotEqual(bool useObjectEquality)
    {
        var commandLine = new CommandLine("git", ["status"]);
        var equalsNull = useObjectEquality
            ? commandLine.Equals((object?) null)
            : commandLine.Equals((CommandLine?) null);

        await Assert.That(equalsNull).IsFalse();
    }

    [Test]
    public async Task CommandLine_UnrelatedObject_IsNotEqual()
    {
        var commandLine = new CommandLine("git", ["status"]);

        await Assert.That(commandLine.Equals("git status")).IsFalse();
    }

    [Test]
    public async Task CommandLine_DerivedRecords_PreserveEqualityContract()
    {
        var commandLine = new CommandLine("git", ["status"]);
        CommandLine derived = new TaggedCommandLine("first");
        CommandLine equalDerived = new TaggedCommandLine("first");
        CommandLine differentDerived = new TaggedCommandLine("second");

        using (Assert.Multiple())
        {
            await Assert.That(commandLine.Equals(derived)).IsFalse();
            await Assert.That(derived.Equals(commandLine)).IsFalse();
            await Assert.That(derived.Equals(equalDerived)).IsTrue();
            await Assert.That(derived.GetHashCode()).IsEqualTo(equalDerived.GetHashCode());
            await Assert.That(derived.Equals(differentDerived)).IsFalse();
        }
    }

    [Test]
    public async Task CommandLine_StoresToolAndArguments()
    {
        var commandLine = new CommandLine("git", ["add", "--all"]);

        using (Assert.Multiple())
        {
            await Assert.That(commandLine.Tool).IsEqualTo("git");
            await Assert.That(commandLine.Arguments).IsEquivalentTo(new[] { "add", "--all" });
        }
    }

    [Test]
    public async Task CommandLine_ToString_FormatsCorrectly()
    {
        var commandLine = new CommandLine("git", ["commit", "-m", "test message"]);

        await Assert.That(commandLine.ToString()).IsEqualTo("git commit -m test message");
    }

    [Test]
    public async Task CommandLine_EmptyArguments_ToStringShowsToolOnly()
    {
        var commandLine = new CommandLine("git", []);

        await Assert.That(commandLine.ToString()).IsEqualTo("git");
    }

    [Test]
    public async Task CommandLine_ArgumentsAreImmutable()
    {
        var args = new List<string> { "add" };
        var commandLine = new CommandLine("git", args);
        var hashCode = commandLine.GetHashCode();
        args.Add("--all");

        await Assert.That(commandLine.Arguments).Count().IsEqualTo(1);
        await Assert.That(commandLine).IsEqualTo(new CommandLine("git", ["add"]));
        await Assert.That(commandLine.GetHashCode()).IsEqualTo(hashCode);
    }

    private record TaggedCommandLine(string Tag) : CommandLine("git", ["status"]);
}
