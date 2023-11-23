using ModularPipelines.Models;

namespace ModularPipelines.UnitTests.Models;

public class RequirementDecisionTests
{
    [Test]
    public void True_Implicit_Cast()
    {
        RequirementDecision requirementDecision = true;
        
        Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success, Is.True);
            Assert.That(requirementDecision.Reason, Is.Null);
        });    
    }
    
    [Test]
    public void False_Implicit_Cast()
    {
        RequirementDecision requirementDecision = false;
        
        Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success, Is.False);
            Assert.That(requirementDecision.Reason, Is.Null);
        });
    }
    
    [Test]
    public void String_Implicit_Cast()
    {
        RequirementDecision requirementDecision = "Foo!";
        
        Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success, Is.False);
            Assert.That(requirementDecision.Reason, Is.EqualTo("Foo"));
        });    
    }

    [Test]
    public void Failed()
    {
        var requirementDecision = RequirementDecision.Failed("Blah!");
        
        Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success, Is.False);
            Assert.That(requirementDecision.Reason, Is.EqualTo("Blah!"));
        });
    }
    
    [Test]
    public void Passed()
    {
        var requirementDecision = RequirementDecision.Passed;
        
        Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success, Is.True);
            Assert.That(requirementDecision.Reason, Is.Null);
        });
    }
    
    [TestCase(true)]
    [TestCase(false)]
    public void Of(bool success)
    {
        var requirementDecision = RequirementDecision.Of(success, "Blah!");
        
        Assert.Multiple(() =>
        {
            Assert.That(requirementDecision.Success, Is.EqualTo(success));
            Assert.That(requirementDecision.Reason, !success ? Is.EqualTo("Blah!") : Is.Null);
        });
    }
}