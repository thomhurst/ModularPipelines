using ModularPipelines.Homebrew.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Homebrew.UnitTests;

public class BrewGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Shared_Flags_Follow_Command_And_Are_Inherited()
    {
        var rendered = await RenderCommand(new BrewListOptions { Debug = true, Quiet = true, Verbose = true });

        await Assert.That(rendered).IsEqualTo("brew list --debug --quiet --verbose");
        await Assert.That(typeof(BrewListOptions).GetProperty(nameof(BrewOptions.Debug))!.DeclaringType)
            .IsEqualTo(typeof(BrewOptions));
    }

    [Test]
    [Arguments(null)]
    [Arguments(false)]
    public async Task Unset_Or_False_Shared_Flags_Are_Omitted(bool? value)
    {
        var rendered = await RenderCommand(new BrewListOptions { Debug = value, Quiet = value, Verbose = value });

        await Assert.That(rendered).IsEqualTo("brew list");
    }

    [Test]
    [Arguments("desc")]
    [Arguments("tests")]
    [Arguments("vulns")]
    public async Task Debug_Set_Through_Base_Is_Visible_To_Command(string command)
    {
        BrewOptions options = command switch
        {
            "desc" => new BrewDescOptions(["wget"]),
            "tests" => new BrewTestsOptions(),
            _ => new BrewVulnsOptions(),
        };
        options.Debug = true;

        var rendered = await RenderCommand(options);

        await Assert.That(rendered).IsEqualTo(command == "desc" ? "brew desc --debug wget" : $"brew {command} --debug");
        await Assert.That((bool?) options.GetType().GetProperty(nameof(BrewOptions.Debug))!.GetValue(options)).IsTrue();
        options.GetType().GetProperty(nameof(BrewOptions.Debug))!.SetValue(options, false);
        await Assert.That(options.Debug).IsFalse();
    }

    [Test]
    public async Task Desc_Keeps_Description_Independent_From_Debug()
    {
        var options = new BrewDescOptions(["wget"]) { Description = true, Debug = true };
        ((BrewOptions) options).Debug = true;

        var rendered = await RenderCommand(options);

        await Assert.That(rendered).IsEqualTo("brew desc --description --debug wget");
    }

    [Test]
    public async Task Tests_Renders_Command_Debug_Only_Once()
    {
        var options = new BrewTestsOptions { Debug = true };
        ((BrewOptions) options).Debug = true;

        var rendered = await RenderCommand(options);

        await Assert.That(rendered).IsEqualTo("brew tests --debug");
    }

    [Test]
    public async Task Vulns_Keeps_Dependencies_Independent_From_Debug()
    {
        var options = new BrewVulnsOptions { Deps = true, Debug = true, Formula = ["wget"] };
        ((BrewOptions) options).Debug = true;

        var rendered = await RenderCommand(options);

        await Assert.That(rendered).IsEqualTo("brew vulns --deps --debug wget");
    }
}
