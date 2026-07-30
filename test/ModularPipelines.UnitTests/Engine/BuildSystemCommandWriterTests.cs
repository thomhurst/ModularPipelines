using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel]
public class BuildSystemCommandWriterTests
{
#pragma warning disable TUnit0055
    [Test]
    public async Task DependencyRegistration_Captures_Output_Before_Service_Resolution()
    {
        const string command = "::add-mask::registration-order";
        var originalOutput = System.Console.Out;
        using var registeredOutput = new StringWriter();
        using var redirectedOutput = new StringWriter();

        try
        {
            System.Console.SetOut(registeredOutput);
            var services = new ServiceCollection();
            DependencyInjectionSetup.Initialize(services);

            System.Console.SetOut(redirectedOutput);
            using var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<IBuildSystemCommandWriter>().WriteLine(command);
        }
        finally
        {
            System.Console.SetOut(originalOutput);
        }

        await Assert.That(registeredOutput.ToString())
            .IsEqualTo($"{command}{Environment.NewLine}");
        await Assert.That(redirectedOutput.ToString()).IsEmpty();
    }
#pragma warning restore TUnit0055

    [Test]
    public async Task Writes_Long_Command_As_One_Unmodified_Line()
    {
        var output = new StringWriter();
        var writer = new BuildSystemCommandWriter(output);
        var command = $"::add-mask::{new string('x', 512)}";

        writer.WriteLine(command);

        await Assert.That(output.ToString()).IsEqualTo($"{command}{Environment.NewLine}");
    }

    [Test]
    public async Task Rejects_Multiple_Physical_Lines()
    {
        var writer = new BuildSystemCommandWriter(new StringWriter());

        await Assert.That(() => writer.WriteLine("::notice::first\nsecond"))
            .Throws<ArgumentException>();
    }
}
