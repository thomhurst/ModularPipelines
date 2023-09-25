namespace ModularPipelines.TestsForTests;

public class Tests
{
    [Test]
    [Category("Pass")]
    public void Pass()
    {
        Assert.Pass();
    }

    [Test]
    [Category("Pass")]
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