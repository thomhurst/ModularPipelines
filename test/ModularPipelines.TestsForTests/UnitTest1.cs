namespace ModularPipelines.TestsForTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test, Category("Pass")]
    public void Pass()
    {
        Assert.Pass();
    }
    
    [Test, Category("Pass")]
    public void Pass2()
    {
        Assert.Pass();
    }
    
    [Test]
    public void Ignore()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void Fail()
    {
        Assert.Fail();
    }
}