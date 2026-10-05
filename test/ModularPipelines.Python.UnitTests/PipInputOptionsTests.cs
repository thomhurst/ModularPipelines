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
    [MatrixDataSource]
    public async Task Script_Files_Are_Repeatable_Standalone_Inputs(
        [Matrix("install", "download", "wheel", "lock")] string verb,
        [Matrix(false, true)] bool empty)
    {
        var builder = await GetService<ICommandLineBuilder>();
        var scripts = new SingleUseValues(empty ? [] : ["first.py", "second script.py"]);
        PipOptions options = verb switch
        {
            "install" => new PipInstallOptions { RequirementsFromScript = scripts },
            "download" => new PipDownloadOptions { RequirementsFromScript = scripts },
            "wheel" => new PipWheelOptions { RequirementsFromScript = scripts },
            "lock" => new PipLockOptions { RequirementsFromScript = scripts },
            _ => throw new ArgumentOutOfRangeException(nameof(verb)),
        };

        for (var attempt = 0; attempt < 2; attempt++)
        {
            if (empty)
            {
                await Assert.That(() => builder.Build(options)).Throws<CommandOptionsValidationException>();
            }
            else
            {
                await OptionsRenderingTestHelper.AssertArguments(builder.Build(options).Arguments,
                    [verb, "--requirements-from-script", "first.py", "--requirements-from-script", "second script.py"]);
            }
        }

        await Assert.That(scripts.EnumerationCount).IsEqualTo(1);
    }

    [Test]
    [Arguments("install")]
    [Arguments("download")]
    [Arguments("wheel")]
    [Arguments("lock")]
    [Arguments("index")]
    [Arguments("list")]
    public async Task Refresh_Values_Preserve_Order_And_Comma_Delimited_Operands(string verb)
    {
        var builder = await GetService<ICommandLineBuilder>();
        string[] values = [":none:", "first,second"];
        PipOptions options = verb switch
        {
            "install" => new PipInstallOptions { RequirementSpecifier = ["example"], RefreshPackage = values },
            "download" => new PipDownloadOptions { RequirementSpecifier = ["example"], RefreshPackage = values },
            "wheel" => new PipWheelOptions { RequirementSpecifier = ["example"], RefreshPackage = values },
            "lock" => new PipLockOptions { LocalProjectPath = ["example"], RefreshPackage = values },
            "index" => new PipIndexOptions { RefreshPackage = values },
            "list" => new PipListOptions { RefreshPackage = values },
            _ => throw new ArgumentOutOfRangeException(nameof(verb)),
        };
        var operand = verb is "index" or "list" ? string.Empty : " example";
        await Assert.That(builder.Build(options).ToString()).IsEqualTo(
            $"pip {verb} --refresh-package :none: --refresh-package first,second{operand}");
    }

    [Test]
    [MatrixDataSource]
    public async Task Dependency_Groups_Reject_Blank_Entries(
        [Matrix("install", "download", "wheel", "lock")] string verb,
        [Matrix("", " ", "\t")] string blank,
        [Matrix(false, true)] bool includeValidGroup)
    {
        var builder = await GetService<ICommandLineBuilder>();
        var groups = new SingleUseValues(includeValidGroup ? ["development", blank] : [blank]);
        PipOptions options = verb switch
        {
            "install" => new PipInstallOptions { Group = groups },
            "download" => new PipDownloadOptions { Group = groups },
            "wheel" => new PipWheelOptions { Group = groups },
            "lock" => new PipLockOptions { Group = groups },
            _ => throw new ArgumentOutOfRangeException(nameof(verb)),
        };

        for (var attempt = 0; attempt < 2; attempt++)
        {
            await Assert.That(() => builder.Build(options)).Throws<CommandOptionsValidationException>();
        }

        await Assert.That(groups.EnumerationCount).IsEqualTo(1);
    }

    [Test]
    [Arguments("install", false)]
    [Arguments("install", true)]
    [Arguments("download", false)]
    [Arguments("download", true)]
    [Arguments("wheel", false)]
    [Arguments("wheel", true)]
    [Arguments("lock", false)]
    [Arguments("lock", true)]
    public async Task Dependency_Groups_Are_A_Repeatable_Standalone_Input(string verb, bool empty)
    {
        var builder = await GetService<ICommandLineBuilder>();
        var groups = new SingleUseValues(empty ? [] : ["development", "testing"]);
        PipOptions options = verb switch
        {
            "install" => new PipInstallOptions { Group = groups },
            "download" => new PipDownloadOptions { Group = groups },
            "wheel" => new PipWheelOptions { Group = groups },
            "lock" => new PipLockOptions { Group = groups },
            _ => throw new ArgumentOutOfRangeException(nameof(verb)),
        };

        for (var attempt = 0; attempt < 2; attempt++)
        {
            if (empty)
            {
                await Assert.That(() => builder.Build(options)).Throws<CommandOptionsValidationException>();
            }
            else
            {
                await Assert.That(builder.Build(options).ToString()).IsEqualTo(
                    $"pip {verb} --group development --group testing");
            }
        }

        await Assert.That(groups.EnumerationCount).IsEqualTo(1);
    }

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
