using ModularPipelines.Models;
using TUnit.Assertions;
using TUnit.Core;

namespace ModularPipelines.UnitTests.Models;

public class SkipDecisionTests
{
    [Test]
    public void True_Implicit_Cast()
    {
        SkipDecision skipDecision = true;
        await Assert.Multiple(() =>
        {
            Assert.That(skipDecision.ShouldSkip).Is.True();
            Assert.That(skipDecision.Reason).Is.Null();
        });
    }

    [Test]
    public void String_Implicit_Cast()
    {
        SkipDecision skipDecision = "Foo!";
        await Assert.Multiple(() =>
        {
            Assert.That(skipDecision.ShouldSkip).Is.True();
            Assert.That(skipDecision.Reason).Is.EqualTo("Foo!");
        });
    }

    [Test]
    public void False_Implicit_Cast()
    {
        SkipDecision skipDecision = false;
        await Assert.Multiple(() =>
        {
            Assert.That(skipDecision.ShouldSkip).Is.False();
            Assert.That(skipDecision.Reason).Is.Null();
        });
    }

    [Test]
    public void Skip()
    {
        var skipDecision = SkipDecision.Skip("Blah!");
        await Assert.Multiple(() =>
        {
            Assert.That(skipDecision.ShouldSkip).Is.True();
            Assert.That(skipDecision.Reason).Is.EqualTo("Blah!");
        });
    }

    [Test]
    public void DoNotSkip()
    {
        var skipDecision = SkipDecision.DoNotSkip;
        await Assert.Multiple(() =>
        {
            Assert.That(skipDecision.ShouldSkip).Is.False();
            Assert.That(skipDecision.Reason).Is.Null();
        });
    }

    [TestWithData(true)]
    [TestWithData(false)]
    public void Of(bool shouldSkip)
    {
        var skipDecision = SkipDecision.Of(shouldSkip, "Blah!");

        Assert.Multiple(() =>
        {
            Assert.That(skipDecision.ShouldSkip).Is.EqualTo(shouldSkip);
            Assert.That(skipDecision.Reason).shouldSkip ? Is.EqualTo("Blah!") : Is.Null();
        });
    }
}