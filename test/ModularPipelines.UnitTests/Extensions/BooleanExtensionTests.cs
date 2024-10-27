using ModularPipelines.Extensions;

namespace ModularPipelines.UnitTests.Extensions;

public class BooleanExtensionTests
{
    [Test]
    public async Task True()
    {
        var trueSkipDecision = true.AsSkipDecisionIfTrue("My reason");
        
        using (Assert.Multiple())
        {
            await Assert.That(trueSkipDecision.ShouldSkip).IsTrue();
            await Assert.That(trueSkipDecision.Reason).IsEqualTo("My reason");
        }
    }

    [Test]
    public async Task False()
    {
        var falseSkipDecision = false.AsSkipDecisionIfTrue("My reason");

        using (Assert.Multiple())
        {
            await Assert.That(falseSkipDecision.ShouldSkip).IsFalse();
            await Assert.That(falseSkipDecision.Reason).IsNull();
        }
    }
}