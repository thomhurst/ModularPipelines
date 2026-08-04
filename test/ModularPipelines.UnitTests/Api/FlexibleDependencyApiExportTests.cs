using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Api;

/// <summary>
/// Verifies that all public types from the flexible dependency API are accessible from their expected namespaces.
/// This ensures the API surface is correctly exported and consumable by library users.
/// </summary>
public class FlexibleDependencyApiExportTests
{
    [Test]
    public async Task IDependencyContext_IsAccessibleFromContextNamespace()
    {
        // Verify IDependencyContext is in ModularPipelines.Context namespace
        var type = typeof(IDependencyContext);

        await Assert.That(type.Namespace).IsEqualTo("ModularPipelines.Context");
        await Assert.That(type.IsPublic).IsTrue();
        await Assert.That(type.IsInterface).IsTrue();
    }

    [Test]
    public async Task DependsOnBaseAttribute_IsAccessibleFromAttributesNamespace()
    {
        // Verify DependsOnBaseAttribute is in ModularPipelines.Attributes namespace
        var type = typeof(DependsOnBaseAttribute);

        await Assert.That(type.Namespace).IsEqualTo("ModularPipelines.Attributes");
        await Assert.That(type.IsPublic).IsTrue();
        await Assert.That(type.IsAbstract).IsTrue();
        await Assert.That(type.IsSubclassOf(typeof(Attribute))).IsTrue();
    }

    [Test]
    public async Task ModuleTagAttribute_IsAccessibleFromAttributesNamespace()
    {
        // Verify ModuleTagAttribute is in ModularPipelines.Attributes namespace
        var type = typeof(ModuleTagAttribute);

        await Assert.That(type.Namespace).IsEqualTo("ModularPipelines.Attributes");
        await Assert.That(type.IsPublic).IsTrue();
        await Assert.That(type.IsSealed).IsTrue();
        await Assert.That(type.IsSubclassOf(typeof(Attribute))).IsTrue();
    }

    [Test]
    public async Task ModuleCategoryAttribute_IsAccessibleFromAttributesNamespace()
    {
        // Verify ModuleCategoryAttribute is in ModularPipelines.Attributes namespace
        var type = typeof(ModuleCategoryAttribute);

        await Assert.That(type.Namespace).IsEqualTo("ModularPipelines.Attributes");
        await Assert.That(type.IsPublic).IsTrue();
        await Assert.That(type.IsSealed).IsTrue();
        await Assert.That(type.IsSubclassOf(typeof(Attribute))).IsTrue();
    }

    [Test]
    public async Task DependsOnModulesWithTagAttribute_IsAccessibleFromAttributesNamespace()
    {
        // Verify DependsOnModulesWithTagAttribute is in ModularPipelines.Attributes namespace
        var type = typeof(DependsOnModulesWithTagAttribute);

        await Assert.That(type.Namespace).IsEqualTo("ModularPipelines.Attributes");
        await Assert.That(type.IsPublic).IsTrue();
        await Assert.That(type.IsSealed).IsTrue();
        await Assert.That(type.IsSubclassOf(typeof(DependsOnBaseAttribute))).IsTrue();
    }

    [Test]
    public async Task DependsOnModulesInCategoryAttribute_IsAccessibleFromAttributesNamespace()
    {
        // Verify DependsOnModulesInCategoryAttribute is in ModularPipelines.Attributes namespace
        var type = typeof(DependsOnModulesInCategoryAttribute);

        await Assert.That(type.Namespace).IsEqualTo("ModularPipelines.Attributes");
        await Assert.That(type.IsPublic).IsTrue();
        await Assert.That(type.IsSealed).IsTrue();
        await Assert.That(type.IsSubclassOf(typeof(DependsOnBaseAttribute))).IsTrue();
    }

    [Test]
    public async Task DependsOnModulesWithAttributeAttribute_IsAccessibleFromAttributesNamespace()
    {
        // Verify DependsOnModulesWithAttributeAttribute<T> is in ModularPipelines.Attributes namespace
        var type = typeof(DependsOnModulesWithAttributeAttribute<>);

        await Assert.That(type.Namespace).IsEqualTo("ModularPipelines.Attributes");
        await Assert.That(type.IsPublic).IsTrue();
        await Assert.That(type.IsGenericTypeDefinition).IsTrue();

        // Verify it inherits from DependsOnBaseAttribute
        var closedType = typeof(DependsOnModulesWithAttributeAttribute<ObsoleteAttribute>);
        await Assert.That(closedType.IsSubclassOf(typeof(DependsOnBaseAttribute))).IsTrue();
    }

    [Test]
    public async Task AllFlexibleDependencyAttributes_HaveCorrectAttributeUsage()
    {
        // Verify all dependency attributes allow multiple usage and inheritance
        var dependencyAttributes = new[]
        {
            typeof(ModuleTagAttribute),
            typeof(DependsOnModulesWithTagAttribute),
            typeof(DependsOnModulesInCategoryAttribute),
            typeof(DependsOnModulesWithAttributeAttribute<>)
        };

        foreach (var attrType in dependencyAttributes)
        {
            var usage = attrType.GetCustomAttributes(typeof(AttributeUsageAttribute), false)
                .Cast<AttributeUsageAttribute>()
                .FirstOrDefault();

            await Assert.That(usage).IsNotNull();
            await Assert.That(usage!.AllowMultiple).IsTrue();
            await Assert.That(usage.Inherited).IsTrue();
        }
    }

    [Test]
    public async Task ModuleCategoryAttribute_DoesNotAllowMultiple()
    {
        // Verify ModuleCategoryAttribute does NOT allow multiple (only one category per module)
        var usage = typeof(ModuleCategoryAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        await Assert.That(usage).IsNotNull();
        await Assert.That(usage!.AllowMultiple).IsFalse();
        await Assert.That(usage.Inherited).IsTrue();
    }

    [Test]
    public async Task IDependencyContext_HasExpectedMethods()
    {
        // Verify IDependencyContext has all required methods for dependency resolution
        var type = typeof(IDependencyContext);
        var methods = type.GetMethods();

        await Assert.That(methods.Any(m => m.Name == "GetTags")).IsTrue();
        await Assert.That(methods.Any(m => m.Name == "GetCategory")).IsTrue();
        await Assert.That(methods.Any(m => m.Name == "HasAttribute")).IsTrue();
        await Assert.That(methods.Any(m => m.Name == "GetAttribute")).IsTrue();
        await Assert.That(methods.Any(m => m.Name == "GetAttributes")).IsTrue();
    }

    [Test]
    public async Task Module_Does_Not_Expose_Metadata_Properties()
    {
        var propertyNames = typeof(Module<>).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(property => property.Name)
            .ToArray();

        await Assert.That(propertyNames).DoesNotContain("Tags");
        await Assert.That(propertyNames).DoesNotContain("Category");
    }

    [Test]
    public async Task ModuleRegistration_Type_Is_Not_Exported()
    {
        var assembly = typeof(Module<>).Assembly;

        await Assert.That(assembly.GetType("ModularPipelines.ModuleRegistration`1")).IsNull();
    }

    [Test]
    public async Task Legacy_Metadata_Types_Are_Not_Exported()
    {
        var assembly = typeof(Module<>).Assembly;

        await Assert.That(assembly.GetType("ModularPipelines.Modules.ITaggedModule")).IsNull();
        await Assert.That(assembly.GetType("ModularPipelines.Options.ModuleRegistrationOptions")).IsNull();
    }
}
