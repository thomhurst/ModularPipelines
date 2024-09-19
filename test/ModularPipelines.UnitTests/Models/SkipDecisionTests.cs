using ModularPipelines.Models;

namespace ModularPipelines.UnitTests.Models;

public class SkipDecisionTests
{
    [Test]
    public async Task True_Implicit_Cast()
    {
        SkipDecision skipDecision = true;
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).IsTrue();
            await Assert.That(skipDecision.Reason).IsNull();
        }
    }

    [Test]
    public async Task String_Implicit_Cast()
    {
        SkipDecision skipDecision = "Foo!";
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).IsTrue();
            await Assert.That(skipDecision.Reason).IsEqualTo("Foo!");
        }
    }

    [Test]
    public async Task False_Implicit_Cast()
    {
        SkipDecision skipDecision = false;
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).IsFalse();
            await Assert.That(skipDecision.Reason).IsNull();
        }
    }

    [Test]
    public async Task Skip()
    {
        var skipDecision = SkipDecision.Skip("Blah!");
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).IsTrue();
            await Assert.That(skipDecision.Reason).IsEqualTo("Blah!");
        }
    }

    [Test]
    public async Task DoNotSkip()
    {
        var skipDecision = SkipDecision.DoNotSkip;
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).IsFalse();
            await Assert.That(skipDecision.Reason).IsNull();
        }
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Of(bool shouldSkip)
    {
        var skipDecision = SkipDecision.Of(shouldSkip, "Blah!");
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).IsEqualTo(shouldSkip);
            await Assert.That(skipDecision.Reason).IsEqualTo(shouldSkip ? "Blah!" : null);
        }
    }
}