using ModularPipelines.Extensions;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests.Extensions;

public class BooleanExtensionTests
{
    [Test]
    public async Task True()
    {
        var trueSkipDecision = true.AsSkipDecisionIfTrue("My reason");
        
        await using (Assert.Multiple())
        {
            await Assert.That(trueSkipDecision.ShouldSkip).Is.True();
            await Assert.That(trueSkipDecision.Reason).Is.EqualTo("My reason");
        }
    }

    [Test]
    public async Task False()
    {
        var falseSkipDecision = false.AsSkipDecisionIfTrue("My reason");

        await using (Assert.Multiple())
        {
            await Assert.That(falseSkipDecision.ShouldSkip).Is.False();
            await Assert.That(falseSkipDecision.Reason).Is.Null();
        }
    }
}