using ModularPipelines.Attributes;

namespace ModularPipelines.UnitTests.Plugins;

public class ModularPipelinesPluginAttributeTests
{
    [Test]
    public async Task Constructor_SetsCompatibleMajorVersion()
    {
        var attr = new ModularPipelinesPluginAttribute(5);

        await Assert.That(attr.CompatibleMajorVersion).IsEqualTo(5);
    }

    [Test]
    public async Task Constructor_AcceptsZero()
    {
        var attr = new ModularPipelinesPluginAttribute(0);

        await Assert.That(attr.CompatibleMajorVersion).IsEqualTo(0);
    }

    [Test]
    public async Task Constructor_ThrowsForNegativeVersion()
    {
        await Assert.That(() => new ModularPipelinesPluginAttribute(-1))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task Attribute_HasCorrectUsage()
    {
        var usage = typeof(ModularPipelinesPluginAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .Single();

        await Assert.That(usage.ValidOn).IsEqualTo(AttributeTargets.Assembly);
        await Assert.That(usage.AllowMultiple).IsFalse();
    }
}
