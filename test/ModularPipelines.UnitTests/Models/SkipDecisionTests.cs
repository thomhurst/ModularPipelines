using ModularPipelines.Models;

namespace ModularPipelines.UnitTests.Models;

public class SkipDecisionTests
{
    [Test]
    public void True_Implicit_Cast()
    {
        SkipDecision skipDecision = true;
        
        Assert.Multiple(() =>
        {
            Assert.That(skipDecision.ShouldSkip, Is.True);
            Assert.That(skipDecision.Reason, Is.Null);
        });    
    }
    
    [Test]
    public void False_Implicit_Cast()
    {
        SkipDecision skipDecision = false;
        
        Assert.Multiple(() =>
        {
            Assert.That(skipDecision.ShouldSkip, Is.False);
            Assert.That(skipDecision.Reason, Is.Null);
        });
    }
    
    [Test]
    public void Skip()
    {
        var skipDecision = SkipDecision.Skip("Blah!");
        
        Assert.Multiple(() =>
        {
            Assert.That(skipDecision.ShouldSkip, Is.True);
            Assert.That(skipDecision.Reason, Is.EqualTo("Blah!"));
        });
    }
    
    [Test]
    public void DoNotSkip()
    {
        var skipDecision = SkipDecision.DoNotSkip;
        
        Assert.Multiple(() =>
        {
            Assert.That(skipDecision.ShouldSkip, Is.True);
            Assert.That(skipDecision.Reason, Is.Null);
        });
    }
    
    [TestCase(true)]
    [TestCase(false)]
    public void Of(bool shouldSkip)
    {
        var skipDecision = SkipDecision.Of(shouldSkip, "Blah!");
        
        Assert.Multiple(() =>
        {
            Assert.That(skipDecision.ShouldSkip, Is.EqualTo(shouldSkip));
            Assert.That(skipDecision.Reason, shouldSkip ? Is.EqualTo("Blah!") : Is.Null);
        });
    }
}