using ModularPipelines.Models;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests.Models;

public class RequirementDecisionTests
{
    [Test]
    public async Task True_Implicit_Cast()
    {
        RequirementDecision requirementDecision = true;
        
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.True();
            Assert.That(requirementDecision.Reason).Is.Null();
        });
    }

    [Test]
    public async Task False_Implicit_Cast()
    {
        RequirementDecision requirementDecision = false;
        
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.False();
            Assert.That(requirementDecision.Reason).Is.Null();
        });
    }

    [Test]
    public async Task String_Implicit_Cast()
    {
        RequirementDecision requirementDecision = "Foo!";
        
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.False();
            Assert.That(requirementDecision.Reason).Is.EqualTo("Foo!");
        });
    }

    [Test]
    public async Task Failed()
    {
        var requirementDecision = RequirementDecision.Failed("Blah!");
        
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.False();
            Assert.That(requirementDecision.Reason).Is.EqualTo("Blah!");
        });
    }

    [Test]
    public async Task Passed()
    {
        var requirementDecision = RequirementDecision.Passed;
        
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.True();
            Assert.That(requirementDecision.Reason).Is.Null();
        });
    }

    [DataDrivenTest]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Of(bool success)
    {
        var requirementDecision = RequirementDecision.Of(success, "Blah!");
        
        await Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success).Is.EqualTo(success);
            Assert.That(requirementDecision.Reason).Is.EqualTo(!success ? "Blah!" : null);
        });
    }
}