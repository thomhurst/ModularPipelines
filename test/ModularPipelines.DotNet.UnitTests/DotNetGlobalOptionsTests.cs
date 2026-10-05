using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.DotNet.UnitTests;

public class DotNetGlobalOptionsTests : TestBase
{
    [Test]
    [Arguments(true, "dotnet --diagnostics tool list --global")]
    [Arguments(false, "dotnet tool list --global")]
    [Arguments(null, "dotnet tool list --global")]
    public async Task Sdk_Diagnostics_Precedes_Nested_Command(bool? diagnostics, string expected)
    {
        var rendered = await RenderCommand(new DotNetToolListOptions { Diagnostics = diagnostics, Global = true });
        await Assert.That(rendered).IsEqualTo(expected);
    }

    [Test]
    public async Task Command_Local_Values_Remain_After_Command()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new DotNetToolListOptions { Diagnostics = true, ToolPath = "tools directory" });
        await Assert.That(string.Join("|", command.Arguments)).IsEqualTo("--diagnostics|tool|list|--tool-path|tools directory");
    }

    [Test]
    public async Task Every_Command_Inherits_One_Diagnostics_Property_Without_Host_Options()
    {
        var commands = typeof(DotNetOptions).Assembly.GetTypes()
            .Where(type => !type.IsAbstract && typeof(DotNetOptions).IsAssignableFrom(type)).ToArray();
        await Assert.That(commands).IsNotEmpty();
        foreach (var type in commands)
        {
            var diagnostics = type.GetProperties().Single(property => property.Name == nameof(DotNetOptions.Diagnostics));
            await Assert.That(diagnostics.DeclaringType).IsEqualTo(typeof(DotNetOptions));
        }

        await Assert.That(typeof(DotNetOptions).GetProperty("Verbosity")).IsNull();
        await Assert.That(typeof(DotNetOptions).GetProperty("FxVersion")).IsNull();
        await Assert.That(typeof(DotNetOptions).GetProperty("Version")).IsNull();
    }
}
