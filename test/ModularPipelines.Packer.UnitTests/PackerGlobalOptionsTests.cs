using ModularPipelines.Packer.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Packer.UnitTests;

public class PackerGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Machine_Readable_Precedes_Command_And_Color_Precedes_Operand()
    {
        var command = await RenderCommand(new PackerBuildOptions("image.pkr.hcl")
        {
            MachineReadable = true,
            Color = false,
        });

        await Assert.That(command).IsEqualTo("packer -machine-readable build -color=false image.pkr.hcl");
    }

    [Test]
    [Arguments(false)]
    [Arguments(null)]
    public async Task Disabled_Machine_Readable_Is_Omitted(bool? enabled)
    {
        var command = await RenderCommand(new PackerBuildOptions("image.pkr.hcl") { MachineReadable = enabled });

        await Assert.That(command).IsEqualTo("packer build image.pkr.hcl");
    }

    [Test]
    [Arguments(true, "true")]
    [Arguments(false, "false")]
    [Arguments(null, null)]
    public async Task Color_Preserves_Boolean_Value_And_Default(bool? color, string? value)
    {
        var command = await RenderCommand(new PackerBuildOptions("image.pkr.hcl") { Color = color });
        var option = value is null ? string.Empty : $" -color={value}";

        await Assert.That(command).IsEqualTo($"packer build{option} image.pkr.hcl");
    }

    [Test]
    [Arguments(true, "true")]
    [Arguments(false, "false")]
    [Arguments(null, null)]
    public async Task Write_Preserves_Boolean_Value_And_Default(bool? write, string? value)
    {
        var command = await RenderCommand(new PackerFmtOptions { Write = write, Template = "image.pkr.hcl" });
        var option = value is null ? string.Empty : $" -write={value}";

        await Assert.That(command).IsEqualTo($"packer fmt{option} image.pkr.hcl");
    }

    [Test]
    [Arguments("hcl2")]
    [Arguments("json")]
    [Arguments(null)]
    public async Task Console_Config_Type_Preserves_Value_And_Default(string? configType)
    {
        var command = await RenderCommand(new PackerConsoleOptions
        {
            ConfigType = configType,
            Template = "image.pkr.hcl",
        });
        var option = configType is null ? string.Empty : $" -config-type={configType}";

        await Assert.That(command).IsEqualTo($"packer console{option} image.pkr.hcl");
    }

    [Test]
    public async Task Debug_Remains_A_Command_Local_Flag()
    {
        var command = await RenderCommand(new PackerBuildOptions("image.pkr.hcl") { Debug = true });

        await Assert.That(command).IsEqualTo("packer build -debug image.pkr.hcl");
        await Assert.That(typeof(PackerBuildOptions).GetProperty(nameof(PackerBuildOptions.Debug))!.DeclaringType)
            .IsEqualTo(typeof(PackerBuildOptions));
    }

    [Test]
    public async Task Every_Generated_Command_Inherits_One_Machine_Readable_Property()
    {
        var commands = typeof(PackerOptions).Assembly.GetTypes()
            .Where(type => !type.IsAbstract && typeof(PackerOptions).IsAssignableFrom(type)).ToArray();
        await Assert.That(commands).IsNotEmpty();
        foreach (var command in commands)
        {
            var properties = command.GetProperties().Where(property => property.Name == nameof(PackerOptions.MachineReadable)).ToArray();
            await Assert.That(properties).Count().IsEqualTo(1);
            await Assert.That(properties.Single().DeclaringType).IsEqualTo(typeof(PackerOptions));
        }
    }
}
