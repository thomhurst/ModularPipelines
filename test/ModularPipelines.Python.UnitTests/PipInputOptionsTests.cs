using System.Collections;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Python.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Python.UnitTests;

public class PipInputOptionsTests : TestBase
{
    private static readonly string[] sourceArray = new[] { "first", "second" };

    [Test]
    [Arguments(false, false, 0)]
    [Arguments(false, false, 1)]
    [Arguments(false, false, 2)]
    [Arguments(false, true, 0)]
    [Arguments(false, true, 1)]
    [Arguments(false, true, 2)]
    [Arguments(true, false, 0)]
    [Arguments(true, false, 1)]
    [Arguments(true, false, 2)]
    [Arguments(true, true, 0)]
    [Arguments(true, true, 1)]
    [Arguments(true, true, 2)]
    public async Task Single_Use_Inputs_Survive_Validation_And_Repeated_Rendering(
        bool wheel, bool requirementFile, int count)
    {
        var builder = await GetService<ICommandLineBuilder>();
        var values = sourceArray.Take(count).ToArray();
        var input = new SingleUseValues(values);
        PipOptions options = (wheel, requirementFile) switch
        {
            (true, true) => new PipWheelOptions { Requirement = input },
            (true, false) => new PipWheelOptions { RequirementSpecifier = input },
            (false, true) => new PipUninstallOptions { Requirement = input },
            (false, false) => new PipUninstallOptions { Package = input },
        };

        for (var attempt = 0; attempt < 2; attempt++)
        {
            if (count == 0)
            {
                await Assert.That(() => builder.Build(options)).Throws<CommandOptionsValidationException>();
            }
            else
            {
                var arguments = string.Join(" ", values.Select(value => requirementFile ? $"--requirement {value}" : value));
                await Assert.That(builder.Build(options).ToString()).IsEqualTo(
                    $"pip {(wheel ? "wheel" : "uninstall")} {arguments}");
            }
        }

        await Assert.That(input.EnumerationCount).IsEqualTo(1);
    }

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

    private sealed class SingleUseValues(string[] values) : IEnumerable<string>
    {
        public int EnumerationCount { get; private set; }

        public IEnumerator<string> GetEnumerator()
        {
            if (++EnumerationCount > 1)
            {
                throw new InvalidOperationException("Input can only be enumerated once.");
            }

            return ((IEnumerable<string>) values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
