using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Rust.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Rust.UnitTests;

public class CargoOptionsTests : TestBase
{
    [Test]
    public async Task Add_Emits_Positional_Dependency_Source()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new CargoAddOptions { Dep = ["serde"] });

        await Assert.That(commandLine.ToString()).IsEqualTo("cargo add serde");
    }

    [Test]
    public async Task Add_Emits_Path_Dependency_Source()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new CargoAddOptions { Path = "../shared" });

        await Assert.That(commandLine.ToString()).IsEqualTo("cargo add --path ../shared");
    }

    [Test]
    public async Task Add_Emits_Git_Dependency_Source()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new CargoAddOptions { Git = "https://example.test/repo.git" });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("cargo add --git https://example.test/repo.git");
    }

    [Test]
    public async Task Add_Rejects_Missing_Dependency_Source()
    {
        var builder = await GetService<ICommandLineBuilder>();

        await Assert.That(() => builder.Build(new CargoAddOptions()))
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("At least one of Dep, Path, or Git must be specified.");
    }

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
