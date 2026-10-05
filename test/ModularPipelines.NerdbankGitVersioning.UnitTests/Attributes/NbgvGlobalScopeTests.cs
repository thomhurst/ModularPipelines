using System.Reflection;
using ModularPipelines.NerdbankGitVersioning.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.NerdbankGitVersioning.UnitTests.Attributes;

public class NbgvGlobalScopeTests : TestBase
{
    [Test]
    public async Task Project_Remains_Command_Local()
    {
        var rendered = await RenderCommand(new NbgvGetVersionOptions { Project = "src/App" });
        await Assert.That(rendered).IsEqualTo("nbgv get-version --project src/App");
        await Assert.That(typeof(NbgvOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)).IsEmpty();
    }

    [Test]
    public async Task Cloud_Version_Remains_A_Value_After_Command()
    {
        var rendered = await RenderCommand(new NbgvCloudOptions { Version = "1.2.3" });
        await Assert.That(rendered).IsEqualTo("nbgv cloud --version 1.2.3");
    }
}
