using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class GitInformationTests : TestBase
{
    [Test]
    public async Task Can_Send_Request_With_String_To_Request_Implicit_Conversion()
    {
        var context = await GetService<IPipelineContext>();
        
        var gitInformation = context.Git().Information;

        var branch = gitInformation.BranchName;
        
        await Assert.That(branch).IsNotNull().And.IsNotEmpty();
    }
}