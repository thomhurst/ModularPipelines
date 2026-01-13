using ModularPipelines.Attributes;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Dependencies;

/// <summary>
/// Unit tests for <see cref="DependsOnModulesWithAttributeAttribute{TAttribute}"/>.
/// </summary>
public class DependsOnModulesWithAttributeAttributeTests : TestBase
{
    [Test]
    public async Task ShouldDependOn_ModuleHasAttribute_ReturnsTrue()
    {
        var attr = new DependsOnModulesWithAttributeAttribute<CriticalAttribute>();
        var context = new MockDependencyContext();

        var result = attr.ShouldDependOn(typeof(CriticalModule), context);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task ShouldDependOn_ModuleLacksAttribute_ReturnsFalse()
    {
        var attr = new DependsOnModulesWithAttributeAttribute<CriticalAttribute>();
        var context = new MockDependencyContext();

        var result = attr.ShouldDependOn(typeof(NonCriticalModule), context);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task ShouldDependOn_ModuleHasInheritedAttribute_ReturnsTrue()
    {
        var attr = new DependsOnModulesWithAttributeAttribute<InheritableAttribute>();
        var context = new MockDependencyContext();

        var result = attr.ShouldDependOn(typeof(DerivedModuleWithInheritedAttribute), context);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task ShouldDependOn_ModuleHasDifferentAttribute_ReturnsFalse()
    {
        var attr = new DependsOnModulesWithAttributeAttribute<CriticalAttribute>();
        var context = new MockDependencyContext();

        var result = attr.ShouldDependOn(typeof(ModuleWithDifferentAttribute), context);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task ShouldDependOn_ModuleHasMultipleAttributesIncludingMatch_ReturnsTrue()
    {
        var attr = new DependsOnModulesWithAttributeAttribute<CriticalAttribute>();
        var context = new MockDependencyContext();

        var result = attr.ShouldDependOn(typeof(ModuleWithMultipleAttributes), context);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task ShouldDependOn_CheckingForSerializableAttribute_ReturnsTrue()
    {
        var attr = new DependsOnModulesWithAttributeAttribute<SerializableAttribute>();
        var context = new MockDependencyContext();

        var result = attr.ShouldDependOn(typeof(SerializableModule), context);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task ShouldDependOn_CheckingForSerializableAttribute_ReturnsFalse()
    {
        var attr = new DependsOnModulesWithAttributeAttribute<SerializableAttribute>();
        var context = new MockDependencyContext();

        var result = attr.ShouldDependOn(typeof(NonCriticalModule), context);

        await Assert.That(result).IsFalse();
    }

    #region Test Attributes and Modules

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class CriticalAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class OtherAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    private sealed class InheritableAttribute : Attribute { }

    [Critical]
    private class CriticalModule { }

    private class NonCriticalModule { }

    [Inheritable]
    private class BaseModuleWithInheritableAttribute { }

    private class DerivedModuleWithInheritedAttribute : BaseModuleWithInheritableAttribute { }

    [Other]
    private class ModuleWithDifferentAttribute { }

    [Critical]
    [Other]
    private class ModuleWithMultipleAttributes { }

    [Serializable]
    private class SerializableModule { }

    #endregion
}
