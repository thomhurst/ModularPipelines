using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Python.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Python.UnitTests;

public class PipInputOptionsTests : TestBase
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Requirement_File_Is_A_Complete_Input_Source(bool wheel)
    {
        var builder = await GetService<ICommandLineBuilder>();
        PipOptions options = wheel
            ? new PipWheelOptions { Requirement = ["requirements.txt"] }
            : new PipUninstallOptions { Requirement = ["requirements.txt"] };

        var commandLine = builder.Build(options);

        await Assert.That(commandLine.ToString()).IsEqualTo(
            $"pip {(wheel ? "wheel" : "uninstall")} --requirement requirements.txt");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Positional_Requirements_Remain_Valid(bool wheel)
    {
        var builder = await GetService<ICommandLineBuilder>();
        PipOptions options = wheel
            ? new PipWheelOptions { RequirementSpecifier = ["example-package"] }
            : new PipUninstallOptions { Package = ["example-package"] };

        await Assert.That(builder.Build(options).ToString()).IsEqualTo(
            $"pip {(wheel ? "wheel" : "uninstall")} example-package");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Missing_Input_Is_Rejected(bool wheel)
    {
        var builder = await GetService<ICommandLineBuilder>();
        PipOptions options = wheel ? new PipWheelOptions() : new PipUninstallOptions();

        await Assert.That(() => builder.Build(options)).Throws<CommandOptionsValidationException>();
    }

    [Test]
    public async Task Wheel_Accepts_An_Editable_Project_Without_Positional_Requirements()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new PipWheelOptions { Editable = "./project" });

        await Assert.That(commandLine.ToString()).IsEqualTo("pip wheel --editable ./project");
    }
}
