using ModularPipelines.Context;
using ModularPipelines.Rust.Enums;
using ModularPipelines.Rust.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Rust.UnitTests;

public class CargoGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Common_Settings_Precede_Command_And_Local_Settings()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new CargoBuildOptions
        {
            Color = CargoColor.Never,
            Config = ["build.jobs=2", "local configuration.toml"],
            Offline = true,
            Verbose = 2,
            Release = true,
        });

        await Assert.That(string.Join("|", command.Arguments)).IsEqualTo(
            "--color|never|--config|build.jobs=2|--config|local configuration.toml|--offline|--verbose|--verbose|build|--release");
    }

    [Test]
    public async Task False_And_Unset_Global_Flags_Are_Omitted()
    {
        var rendered = await RenderCommand(new CargoCheckOptions { Quiet = false, Offline = false, Verbose = 0 });
        await Assert.That(rendered).IsEqualTo("cargo check");
    }

    [Test]
    public async Task Manifest_Options_Remain_After_Command()
    {
        var rendered = await RenderCommand(new CargoBuildOptions { Locked = true, ManifestPath = "Cargo.toml" });
        await Assert.That(rendered).IsEqualTo("cargo --locked build --manifest-path Cargo.toml");
    }

    [Test]
    public async Task Every_Command_Inherits_Exactly_One_Copy_Of_Each_Global_Property()
    {
        string[] names = ["Color", "Config", "Frozen", "Locked", "Offline", "Quiet", "Verbose"];
        foreach (var type in typeof(CargoOptions).Assembly.GetTypes()
                     .Where(type => !type.IsAbstract && typeof(CargoOptions).IsAssignableFrom(type)))
        {
            foreach (var name in names)
            {
                var property = type.GetProperties().Single(property => property.Name == name);
                await Assert.That(property.DeclaringType).IsEqualTo(typeof(CargoOptions));
            }
        }

        await Assert.That(typeof(CargoOptions).GetProperty(nameof(CargoOptions.Config))!
            .IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(typeof(CargoOptions).GetProperty("C")).IsNull();
        await Assert.That(typeof(CargoOptions).GetProperty("Z")).IsNull();
        await Assert.That(typeof(CargoOptions).GetProperty("ManifestPath")).IsNull();
    }
}
