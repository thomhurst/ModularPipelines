using ModularPipelines.Extensions;
using ModularPipelines.Options;
using TUnit.Assertions;
using TUnit.Core;

namespace ModularPipelines.UnitTests.Extensions;

public class CommandExtensionsTests
{
    [Test]
    public async Task ToToolOptions_SingleArg()
    {
        var commandLineOptions = new CommandLineOptions()
            .ToCommandLineToolOptions("mytool", "arg1");
        await Assert.Multiple(() =>
        {
            Assert.That(commandLineOptions.Tool).Is.EqualTo("mytool");
            Assert.That(commandLineOptions.Arguments!).Is.EquivalentTo(new[] { "arg1" });
        });
    }

    [Test]
    public async Task ToToolOptions_MultipleArgs()
    {
        var commandLineOptions = new CommandLineOptions()
            .ToCommandLineToolOptions("mytool", new[] { "arg1", "arg2" });
        await Assert.Multiple(() =>
        {
            Assert.That(commandLineOptions.Tool).Is.EqualTo("mytool");
            Assert.That(commandLineOptions.Arguments!).Is.EquivalentTo(new[] { "arg1", "arg2" });
        });
    }

    [Test]
    public async Task WithArguments_AddsToExisting()
    {
        var commandLineOptions = new CommandLineOptions()
            .ToCommandLineToolOptions("mytool", new[] { "arg1", "arg2" })
            .WithArguments(new[] { "arg3", "arg4", "arg5" });
        await Assert.Multiple(() =>
        {
            Assert.That(commandLineOptions.Tool).Is.EqualTo("mytool");
            Assert.That(commandLineOptions.Arguments!).Is.EquivalentTo(new[] { "arg1", "arg2", "arg3", "arg4", "arg5" });
        });
    }
}