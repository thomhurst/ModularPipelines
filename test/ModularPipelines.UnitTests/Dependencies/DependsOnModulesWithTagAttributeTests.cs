using ModularPipelines.Attributes;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Dependencies;

/// <summary>
/// Unit tests for <see cref="DependsOnModulesWithTagAttribute"/>.
/// </summary>
public class DependsOnModulesWithTagAttributeTests : TestBase
{
    [Test]
    public async Task ShouldDependOn_ModuleHasTag_ReturnsTrue()
    {
        var attr = new DependsOnModulesWithTagAttribute("database");
        var context = new MockDependencyContext().WithTags(typeof(TestModule), "database");

        var result = attr.ShouldDependOn(typeof(TestModule), context);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task ShouldDependOn_ModuleLacksTag_ReturnsFalse()
    {
        var attr = new DependsOnModulesWithTagAttribute("database");
        var context = new MockDependencyContext().WithTags(typeof(TestModule), "other");

        var result = attr.ShouldDependOn(typeof(TestModule), context);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task ShouldDependOn_ModuleHasNoTags_ReturnsFalse()
    {
        var attr = new DependsOnModulesWithTagAttribute("database");
        var context = new MockDependencyContext();

        var result = attr.ShouldDependOn(typeof(TestModule), context);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task ShouldDependOn_CaseInsensitive_ReturnsTrue()
    {
        var attr = new DependsOnModulesWithTagAttribute("DATABASE");
        var context = new MockDependencyContext().WithTags(typeof(TestModule), "database");

        var result = attr.ShouldDependOn(typeof(TestModule), context);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task ShouldDependOn_ModuleHasMultipleTagsIncludingMatch_ReturnsTrue()
    {
        var attr = new DependsOnModulesWithTagAttribute("database");
        var context = new MockDependencyContext().WithTags(typeof(TestModule), "infrastructure", "database", "critical");

        var result = attr.ShouldDependOn(typeof(TestModule), context);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Constructor_WithNullTag_ThrowsArgumentException()
    {
        await Assert.That(() => new DependsOnModulesWithTagAttribute(null!))
            .ThrowsException();
    }

    [Test]
    public async Task Constructor_WithEmptyTag_ThrowsArgumentException()
    {
        await Assert.That(() => new DependsOnModulesWithTagAttribute(string.Empty))
            .ThrowsException();
    }

    [Test]
    public async Task Constructor_WithWhitespaceTag_ThrowsArgumentException()
    {
        await Assert.That(() => new DependsOnModulesWithTagAttribute("   "))
            .ThrowsException();
    }

    [Test]
    public async Task Tag_Property_ReturnsConstructorValue()
    {
        var attr = new DependsOnModulesWithTagAttribute("my-tag");

        await Assert.That(attr.Tag).IsEqualTo("my-tag");
    }

    private class TestModule { }
}
