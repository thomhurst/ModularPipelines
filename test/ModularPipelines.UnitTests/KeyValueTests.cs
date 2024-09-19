using ModularPipelines.Models;

namespace ModularPipelines.UnitTests;

public class KeyValueTests
{
    [Test]
    public async Task ImplicitOperator1()
    {
        KeyValue keyValue = ("one", "two");
        
        await using (Assert.Multiple())
        {
            await Assert.That(keyValue.Key).IsEqualTo("one");
            await Assert.That(keyValue.Value).IsEqualTo("two");
        }
    }

    [Test]
    public async Task ImplicitOperator2()
    {
        KeyValue keyValue = new Tuple<string, string>("one", "two");
        
        await using (Assert.Multiple())
        {
            await Assert.That(keyValue.Key).IsEqualTo("one");
            await Assert.That(keyValue.Value).IsEqualTo("two");
        }
    }

    [Test]
    public async Task ImplicitOperator3()
    {
        KeyValue keyValue = new KeyValuePair<string, string>("one", "two");
        
        await using (Assert.Multiple())
        {
            await Assert.That(keyValue.Key).IsEqualTo("one");
            await Assert.That(keyValue.Value).IsEqualTo("two");
        }
    }
}