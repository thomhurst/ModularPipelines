using ModularPipelines.Enums;
using ModularPipelines.Helpers;

namespace ModularPipelines.UnitTests.Helpers;

public class StatusDisplayProviderTests
{
    [Test]
    public async Task Canceled_Status_Uses_Neutral_Message()
    {
        var message = StatusDisplayProvider.FormatStatusMessage("ExampleModule", ModuleStatus.Canceled);

        await Assert.That(message).Contains("was canceled");
        await Assert.That(message).DoesNotContain("pipeline error");
    }
}
