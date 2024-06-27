using ModularPipelines.Models;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests.Models;

public class SkipDecisionTests
{
    [Test]
    public async Task True_Implicit_Cast()
    {
        SkipDecision skipDecision = true;
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).Is.True();
            await Assert.That(skipDecision.Reason).Is.Null();
        }
    }

    [Test]
    public async Task String_Implicit_Cast()
    {
        SkipDecision skipDecision = "Foo!";
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).Is.True();
            await Assert.That(skipDecision.Reason).Is.EqualTo("Foo!");
        }
    }

    [Test]
    public async Task False_Implicit_Cast()
    {
        SkipDecision skipDecision = false;
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).Is.False();
            await Assert.That(skipDecision.Reason).Is.Null();
        }
    }

    [Test]
    public async Task Skip()
    {
        var skipDecision = SkipDecision.Skip("Blah!");
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).Is.True();
            await Assert.That(skipDecision.Reason).Is.EqualTo("Blah!");
        }
    }

    [Test]
    public async Task DoNotSkip()
    {
        var skipDecision = SkipDecision.DoNotSkip;
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).Is.False();
            await Assert.That(skipDecision.Reason).Is.Null();
        }
    }

    [DataDrivenTest]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Of(bool shouldSkip)
    {
        var skipDecision = SkipDecision.Of(shouldSkip, "Blah!");
        
        await using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).Is.EqualTo(shouldSkip);
            await Assert.That(skipDecision.Reason).Is.EqualTo(shouldSkip ? "Blah!" : null);
        }
    }
}