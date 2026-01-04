namespace ModularPipelines.TestsForTests;

public class Tests
{
    [Test]
    [Category("Pass")]
    public void Pass()
    {
    }

    [Test]
    [Category("Pass")]
    public void Pass2()
    {
    }

    [Test]
    public void Ignore()
    {
        Skip.Test("Ignoring this test");
    }

    [Test]
    public void Fail()
    {
        Assert.Fail("This test is meant to fail");
    }
}