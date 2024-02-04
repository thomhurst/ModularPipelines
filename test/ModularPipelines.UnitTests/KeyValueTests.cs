using ModularPipelines.Models;
using TUnit.Assertions;
using TUnit.Core;

namespace ModularPipelines.UnitTests;

public class KeyValueTests
{
    [Test]
    public void ImplicitOperator1()
    {
        KeyValue keyValue = ("one", "two");

        Assert.Multiple(() =>
        {
            Assert.That(keyValue.Key).Is.EqualTo("one");
            Assert.That(keyValue.Value).Is.EqualTo("two");
        });
    }

    [Test]
    public void ImplicitOperator2()
    {
        KeyValue keyValue = new Tuple<string, string>("one", "two");

        Assert.Multiple(() =>
        {
            Assert.That(keyValue.Key).Is.EqualTo("one");
            Assert.That(keyValue.Value).Is.EqualTo("two");
        });
    }

    [Test]
    public void ImplicitOperator3()
    {
        KeyValue keyValue = new KeyValuePair<string, string>("one", "two");

        Assert.Multiple(() =>
        {
            Assert.That(keyValue.Key).Is.EqualTo("one");
            Assert.That(keyValue.Value).Is.EqualTo("two");
        });
    }
}