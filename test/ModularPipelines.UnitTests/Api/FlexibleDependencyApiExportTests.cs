using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DependencyInjection;
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
    public async Task ITaggedModule_IsAccessibleFromModulesNamespace()
    {
        // Verify ITaggedModule is in ModularPipelines.Modules namespace
        var type = typeof(ITaggedModule);

        await Assert.That(type.Namespace).IsEqualTo("ModularPipelines.Modules");
        await Assert.That(type.IsPublic).IsTrue();
        await Assert.That(type.IsInterface).IsTrue();
    }

    [Test]
    public async Task IModuleRegistrationBuilder_IsAccessibleFromDependencyInjectionNamespace()
    {
        // Verify IModuleRegistrationBuilder is in ModularPipelines.DependencyInjection namespace
        var type = typeof(IModuleRegistrationBuilder);

        await Assert.That(type.Namespace).IsEqualTo("ModularPipelines.DependencyInjection");
        await Assert.That(type.IsPublic).IsTrue();
        await Assert.That(type.IsInterface).IsTrue();
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
    public async Task ITaggedModule_HasExpectedProperties()
    {
        // Verify ITaggedModule has all required properties
        var type = typeof(ITaggedModule);
        var properties = type.GetProperties();

        await Assert.That(properties.Any(p => p.Name == "Tags")).IsTrue();
        await Assert.That(properties.Any(p => p.Name == "Category")).IsTrue();
    }

    [Test]
    public async Task IModuleRegistrationBuilder_HasExpectedMembers()
    {
        // Verify IModuleRegistrationBuilder has all required members
        var type = typeof(IModuleRegistrationBuilder);
        var properties = type.GetProperties();
        var methods = type.GetMethods();

        await Assert.That(properties.Any(p => p.Name == "Services")).IsTrue();
        await Assert.That(methods.Any(m => m.Name == "WithTags")).IsTrue();
        await Assert.That(methods.Any(m => m.Name == "WithCategory")).IsTrue();
    }
}
