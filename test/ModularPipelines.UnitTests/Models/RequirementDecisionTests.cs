using ModularPipelines.Models;
using TUnit.Assertions;
using TUnit.Core;

namespace ModularPipelines.UnitTests.Models;

public class RequirementDecisionTests
{
    [Test]
    public void True_Implicit_Cast()
    {
        RequirementDecision requirementDecision = true;
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.True();
            Assert.That(requirementDecision.Reason).Is.Null();
        });
    }

    [Test]
    public void False_Implicit_Cast()
    {
        RequirementDecision requirementDecision = false;
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.False();
            Assert.That(requirementDecision.Reason).Is.Null();
        });
    }

    [Test]
    public void String_Implicit_Cast()
    {
        RequirementDecision requirementDecision = "Foo!";
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.False();
            Assert.That(requirementDecision.Reason).Is.EqualTo("Foo!");
        });
    }

    [Test]
    public void Failed()
    {
        var requirementDecision = RequirementDecision.Failed("Blah!");
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.False();
            Assert.That(requirementDecision.Reason).Is.EqualTo("Blah!");
        });
    }

    [Test]
    public void Passed()
    {
        var requirementDecision = RequirementDecision.Passed;
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.True();
            Assert.That(requirementDecision.Reason).Is.Null();
        });
    }

    [TestWithData(true)]
    [TestWithData(false)]
    public void Of(bool success)
    {
        var requirementDecision = RequirementDecision.Of(success, "Blah!");
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.EqualTo(success);
            Assert.That(requirementDecision.Reason).Is.EqualTo(success ? "Blah!" : null);
        });
    }
}