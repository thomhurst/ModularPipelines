using ModularPipelines.Models;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests.Models;

public class RequirementDecisionTests
{
    [Test]
    public async Task True_Implicit_Cast()
    {
        RequirementDecision requirementDecision = true;
        
        await using (Assert.Multiple())
        {
            await Assert.That(requirementDecision.Success).Is.True();
            await Assert.That(requirementDecision.Reason).Is.Null();
        }
    }

    [Test]
    public async Task False_Implicit_Cast()
    {
        RequirementDecision requirementDecision = false;
        
        await using (Assert.Multiple())
        {
            await Assert.That(requirementDecision.Success).Is.False();
            await Assert.That(requirementDecision.Reason).Is.Null();
        }
    }

    [Test]
    public async Task String_Implicit_Cast()
    {
        RequirementDecision requirementDecision = "Foo!";
        
        await using (Assert.Multiple())
        {
            await Assert.That(requirementDecision.Success).Is.False();
            await Assert.That(requirementDecision.Reason).Is.EqualTo("Foo!");
        }
    }

    [Test]
    public async Task Failed()
    {
        var requirementDecision = RequirementDecision.Failed("Blah!");
        
        await using (Assert.Multiple())
        {
            await Assert.That(requirementDecision.Success).Is.False();
            await Assert.That(requirementDecision.Reason).Is.EqualTo("Blah!");
        }
    }

    [Test]
    public async Task Passed()
    {
        var requirementDecision = RequirementDecision.Passed;
        
        await using (Assert.Multiple())
        {
            await Assert.That(requirementDecision.Success).Is.True();
            await Assert.That(requirementDecision.Reason).Is.Null();
        }
    }

    [DataDrivenTest]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Of(bool success)
    {
        var requirementDecision = RequirementDecision.Of(success, "Blah!");
        
        await using (Assert.Multiple())
        {
            await Assert.That(requirementDecision.Success).Is.EqualTo(success);
            await Assert.That(requirementDecision.Reason).Is.EqualTo(!success ? "Blah!" : null);
        }
    }
}