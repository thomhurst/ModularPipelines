using System.Reflection;
using ModularPipelines.GitHub.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.GitHub.UnitTests;

public class GhGlobalOptionsAuditTests : TestBase
{
    [Test]
    public async Task Repository_Selection_Follows_Issue_Command_And_Renders_Once()
    {
        var rendered = await RenderCommand(new GhIssueListOptions { Repo = "owner/repository", Limit = 3 });

        await Assert.That(rendered).IsEqualTo("gh issue list --limit=3 --repo=owner/repository");
    }

    [Test]
    public async Task Base_And_Unrelated_Commands_Do_Not_Expose_Repository_Selection()
    {
        await Assert.That(typeof(GhOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)).IsEmpty();
        await Assert.That(typeof(GhAuthStatusOptions).GetProperty("Repo")).IsNull();
        await Assert.That(typeof(GhApiOptions).GetProperty("Repo")).IsNull();
        await Assert.That(typeof(GhConfigGetOptions).GetProperty("Repo")).IsNull();
        await Assert.That(await RenderCommand(new GhAuthStatusOptions())).IsEqualTo("gh auth status");
    }
}
