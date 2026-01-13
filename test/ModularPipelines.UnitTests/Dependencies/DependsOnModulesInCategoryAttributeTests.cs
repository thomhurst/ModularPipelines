using ModularPipelines.Attributes;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Dependencies;

/// <summary>
/// Unit tests for <see cref="DependsOnModulesInCategoryAttribute"/>.
/// </summary>
public class DependsOnModulesInCategoryAttributeTests : TestBase
{
    [Test]
    public async Task ShouldDependOn_ModuleInCategory_ReturnsTrue()
    {
        var attr = new DependsOnModulesInCategoryAttribute("infrastructure");
        var context = new MockDependencyContext().WithCategory(typeof(TestModule), "infrastructure");

        var result = attr.ShouldDependOn(typeof(TestModule), context);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task ShouldDependOn_ModuleInDifferentCategory_ReturnsFalse()
    {
        var attr = new DependsOnModulesInCategoryAttribute("infrastructure");
        var context = new MockDependencyContext().WithCategory(typeof(TestModule), "build");

        var result = attr.ShouldDependOn(typeof(TestModule), context);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task ShouldDependOn_CaseInsensitive_ReturnsTrue()
    {
        var attr = new DependsOnModulesInCategoryAttribute("INFRASTRUCTURE");
        var context = new MockDependencyContext().WithCategory(typeof(TestModule), "infrastructure");

        var result = attr.ShouldDependOn(typeof(TestModule), context);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task ShouldDependOn_ModuleHasNoCategory_ReturnsFalse()
    {
        var attr = new DependsOnModulesInCategoryAttribute("infrastructure");
        var context = new MockDependencyContext();

        var result = attr.ShouldDependOn(typeof(TestModule), context);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task ShouldDependOn_CategoryMatchesExactly_ReturnsTrue()
    {
        var attr = new DependsOnModulesInCategoryAttribute("build-pipeline");
        var context = new MockDependencyContext().WithCategory(typeof(TestModule), "build-pipeline");

        var result = attr.ShouldDependOn(typeof(TestModule), context);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Constructor_WithNullCategory_ThrowsArgumentException()
    {
        await Assert.That(() => new DependsOnModulesInCategoryAttribute(null!))
            .ThrowsException();
    }

    [Test]
    public async Task Constructor_WithEmptyCategory_ThrowsArgumentException()
    {
        await Assert.That(() => new DependsOnModulesInCategoryAttribute(string.Empty))
            .ThrowsException();
    }

    [Test]
    public async Task Constructor_WithWhitespaceCategory_ThrowsArgumentException()
    {
        await Assert.That(() => new DependsOnModulesInCategoryAttribute("   "))
            .ThrowsException();
    }

    [Test]
    public async Task Category_Property_ReturnsConstructorValue()
    {
        var attr = new DependsOnModulesInCategoryAttribute("my-category");

        await Assert.That(attr.Category).IsEqualTo("my-category");
    }

    private class TestModule { }
}
