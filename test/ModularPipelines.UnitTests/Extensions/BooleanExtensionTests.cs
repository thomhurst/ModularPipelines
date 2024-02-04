using ModularPipelines.Extensions;
using TUnit.Assertions;
using TUnit.Core;

namespace ModularPipelines.UnitTests.Extensions;

public class BooleanExtensionTests
{
    [Test]
    public void True()
    {
        var trueSkipDecision = true.AsSkipDecisionIfTrue("My reason");
        
        Assert.Multiple(() =>
        {
            Assert.That(trueSkipDecision.ShouldSkip).Is.True();
            Assert.That(trueSkipDecision.Reason).Is.EqualTo("My reason");
        });
    }
    
    [Test]
    public void False()
    {
        var falseSkipDecision = false.AsSkipDecisionIfTrue("My reason");
        
        Assert.Multiple(() =>
        {
            Assert.That(falseSkipDecision.ShouldSkip).Is.False();
            Assert.That(falseSkipDecision.Reason).Is.Null);
        });
    }
}