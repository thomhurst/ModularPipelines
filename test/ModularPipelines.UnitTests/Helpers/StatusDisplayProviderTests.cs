using ModularPipelines.Enums;
using ModularPipelines.Helpers;

namespace ModularPipelines.UnitTests.Helpers;

public class StatusDisplayProviderTests
{
    [Test]
    public async Task Cancelled_Status_Uses_Neutral_Message()
    {
        var message = StatusDisplayProvider.FormatStatusMessage("ExampleModule", ModuleStatus.Cancelled);

        await Assert.That(message).Contains("was cancelled");
        await Assert.That(message).DoesNotContain("pipeline error");
    }
}
