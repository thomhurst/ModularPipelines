using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Dependencies;

public class ModuleTagAttributeTests : TestBase
{
    [Test]
    public async Task Constructor_WithValidTag_SetsTagProperty()
    {
        var attr = new ModuleTagAttribute("database");
        await Assert.That(attr.Tag).IsEqualTo("database");
    }

    [Test]
    public async Task Constructor_WithNullTag_ThrowsArgumentException()
    {
        await Assert.That(() => new ModuleTagAttribute(null!))
            .ThrowsException()
            .And.IsTypeOf<ArgumentException>();
    }

    [Test]
    public async Task Constructor_WithEmptyTag_ThrowsArgumentException()
    {
        await Assert.That(() => new ModuleTagAttribute(""))
            .ThrowsException()
            .And.IsTypeOf<ArgumentException>();
    }

    [Test]
    public async Task Constructor_WithWhitespaceTag_ThrowsArgumentException()
    {
        await Assert.That(() => new ModuleTagAttribute("   "))
            .ThrowsException()
            .And.IsTypeOf<ArgumentException>();
    }

    [Test]
    public async Task Attribute_AllowsMultiple()
    {
        var usage = typeof(ModuleTagAttribute).GetCustomAttribute<AttributeUsageAttribute>();
        await Assert.That(usage!.AllowMultiple).IsTrue();
    }

    [Test]
    public async Task Attribute_IsInheritable()
    {
        var usage = typeof(ModuleTagAttribute).GetCustomAttribute<AttributeUsageAttribute>();
        await Assert.That(usage!.Inherited).IsTrue();
    }
}
