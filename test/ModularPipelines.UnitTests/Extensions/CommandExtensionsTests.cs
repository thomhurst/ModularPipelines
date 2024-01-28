using ModularPipelines.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Extensions;

public class CommandExtensionsTests
{
    [Test]
    public void ToToolOptions_SingleArg()
    {
        var commandLineOptions = new CommandLineOptions()
            .ToCommandLineToolOptions("mytool", "arg1");
        
        Assert.Multiple(() =>
        {
            Assert.That(commandLineOptions.Tool, Is.EqualTo("mytool"));
            Assert.That(commandLineOptions.Arguments, Is.EquivalentTo(new[] { "arg1" }));
        });
    }
    
    [Test]
    public void ToToolOptions_MultipleArgs()
    {
        var commandLineOptions = new CommandLineOptions()
            .ToCommandLineToolOptions("mytool", ["arg1", "arg2"]);
        
        Assert.Multiple(() =>
        {
            Assert.That(commandLineOptions.Tool, Is.EqualTo("mytool"));
            Assert.That(commandLineOptions.Arguments, Is.EquivalentTo(new[] { "arg1", "arg2" }));
        });
    }
    
    [Test]
    public void WithArguments_AddsToExisting()
    {
        var commandLineOptions = new CommandLineOptions()
            .ToCommandLineToolOptions("mytool", ["arg1", "arg2"])
            .WithArguments(["arg3", "arg4", "arg5"]);
        
        Assert.Multiple(() =>
        {
            Assert.That(commandLineOptions.Tool, Is.EqualTo("mytool"));
            Assert.That(commandLineOptions.Arguments, Is.EquivalentTo(new[] { "arg1", "arg2", "arg3", "arg4", "arg5" }));
        });
    }
}