using ModularPipelines.Models;

namespace ModularPipelines.UnitTests;

public class KeyValueTests
{
    [Test]
    public async Task ImplicitOperator1()
    {
        KeyValue keyValue = ("one", "two");
        
        await Assert.Multiple(() =>
        {
            Assert.That(keyValue.Key).Is.EqualTo("one");
            Assert.That(keyValue.Value).Is.EqualTo("two");
        });
    }

    [Test]
    public async Task ImplicitOperator2()
    {
        KeyValue keyValue = new Tuple<string, string>("one", "two");
        
        await Assert.Multiple(() =>
        {
            Assert.That(keyValue.Key).Is.EqualTo("one");
            Assert.That(keyValue.Value).Is.EqualTo("two");
        });
    }

    [Test]
    public async Task ImplicitOperator3()
    {
        KeyValue keyValue = new KeyValuePair<string, string>("one", "two");
        
        await Assert.Multiple(() =>
        {
            Assert.That(keyValue.Key).Is.EqualTo("one");
            Assert.That(keyValue.Value).Is.EqualTo("two");
        });
    }
}