using ModularPipelines.Context;
using ModularPipelines.Rust.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Rust.UnitTests;

public class CargoOptionsTests : TestBase
{
    [Test]
    public async Task Preserves_PassThrough_Switch_After_Test_Arguments()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new CargoTestOptions
        {
            Args = ["filter"],
            Arguments = ["--help"],
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("cargo test -- filter --help");
    }

    [Test]
    public async Task Hoists_Attached_Equals_Option_Before_Test_Arguments()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new CargoTestOptions
        {
            Args = ["filter"],
            Arguments = ["--color=never"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("cargo test --color=never -- filter");
    }
}
