namespace ModularPipelines.UnitTests.FSharp.Api

open System
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.DependencyInjection
open ModularPipelines.Modules
open TUnit.Core
open TUnit.Assertions.FSharp.Operations
open TUnit.Assertions.Extensions
open TUnit.Assertions

/// <summary>
/// Verifies that all public types from the flexible dependency API are accessible from their expected namespaces. This
/// ensures the API surface is correctly exported and consumable by library users.
/// </summary>
type FlexibleDependencyApiExportTests() =
    [<Test>]
    member _.IDependencyContext_IsAccessibleFromContextNamespace() =
        async {
            let dependencyContextType = typeof<IDependencyContext>

            do! check(
                StringEqualsAssertionExtensions.IsEqualTo(
                    Assert.That(dependencyContextType.Namespace),
                    "ModularPipelines.Context"
                )
            )

            do! check(Assert.That(dependencyContextType.IsPublic).IsTrue())
            do! check(Assert.That(dependencyContextType.IsInterface).IsTrue())
        }
    [<Test>]
    member _.DependsOnBaseAttribute_IsAccessibleFromAttributesNamespace() = async {
        // Verify DependsOnBaseAttribute is in ModularPipelines.Attributes namespace
        let dependsOnBaseAttributeType = typeof<DependsOnBaseAttribute>
        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(dependsOnBaseAttributeType.Namespace),
                "ModularPipelines.Attributes"
            )
        )
        do! check(Assert.That(dependsOnBaseAttributeType.IsPublic).IsTrue())
        do! check(Assert.That(dependsOnBaseAttributeType.IsAbstract).IsTrue())
        do! check(Assert.That(dependsOnBaseAttributeType.IsSubclassOf(typeof<Attribute>)).IsTrue())
    }

    [<Test>]
    member _.ModuleTagAttribute_IsAccessibleFromAttributesNamespace() = async {
        // Verify ModuleTagAttribute is in ModularPipelines.Attributes namespace
        let moduleTagAttributeType = typeof<ModuleTagAttribute>
        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(moduleTagAttributeType.Namespace),
                "ModularPipelines.Attributes"
            )
        )
        do! check(Assert.That(moduleTagAttributeType.IsPublic).IsTrue())
        do! check(Assert.That(moduleTagAttributeType.IsSealed).IsTrue())
        do! check(Assert.That(moduleTagAttributeType.IsSubclassOf(typeof<Attribute>)).IsTrue())
    }

    [<Test>]
    member _.ModuleCategoryAttribute_IsAccessibleFromAttributesNamespace() = async {
        // Verify ModuleCategoryAttribute is in ModularPipelines.Attributes namespace
        let moduleCategoryAttributeType = typeof<ModuleCategoryAttribute>
        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(moduleCategoryAttributeType.Namespace),
                "ModularPipelines.Attributes"
            )
        )
        do! check(Assert.That(moduleCategoryAttributeType.IsPublic).IsTrue())
        do! check(Assert.That(moduleCategoryAttributeType.IsSealed).IsTrue())
        do! check(Assert.That(moduleCategoryAttributeType.IsSubclassOf(typeof<Attribute>)).IsTrue())
    }

    [<Test>]
    member _.DependsOnModulesWithTagAttribute_IsAccessibleFromAttributesNamespace() = async {
        // Verify DependsOnModulesWithTagAttribute is in ModularPipelines.Attributes namespace
        let dependsOnModulesWithTagAttributeType = typeof<DependsOnModulesWithTagAttribute>

        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(dependsOnModulesWithTagAttributeType.Namespace),
                "ModularPipelines.Attributes"
            )
        )
        do! check(Assert.That(dependsOnModulesWithTagAttributeType.IsPublic).IsTrue())
        do! check(Assert.That(dependsOnModulesWithTagAttributeType.IsSealed).IsTrue())
        do! check(Assert.That(dependsOnModulesWithTagAttributeType.IsSubclassOf(typeof<DependsOnBaseAttribute>)).IsTrue())
    }

    [<Test>]
    member _.DependsOnModulesInCategoryAttribute_IsAccessibleFromAttributesNamespace() = async {
        // Verify DependsOnModulesInCategoryAttribute is in ModularPipelines.Attributes namespace
        let dependsOnModulesInCategoryAttributeType = typeof<DependsOnModulesInCategoryAttribute>

        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(dependsOnModulesInCategoryAttributeType.Namespace),
                "ModularPipelines.Attributes"
            )
        )
        do! check(Assert.That(dependsOnModulesInCategoryAttributeType.IsPublic).IsTrue())
        do! check(Assert.That(dependsOnModulesInCategoryAttributeType.IsSealed).IsTrue())
        do! check(Assert.That(dependsOnModulesInCategoryAttributeType.IsSubclassOf(typeof<DependsOnBaseAttribute>)).IsTrue())
    }

    [<Test>]
    member _.DependsOnModulesWithAttributeAttribute_IsAccessibleFromAttributesNamespace() = async {
        // Verify DependsOnModulesWithAttributeAttribute<T> is in ModularPipelines.Attributes namespace
        let dependsOnModulesWithAttributeAttributeType = typedefof<DependsOnModulesWithAttributeAttribute<_>>

        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(dependsOnModulesWithAttributeAttributeType.Namespace),
                "ModularPipelines.Attributes"
            )
        )
        do! check(Assert.That(dependsOnModulesWithAttributeAttributeType.IsPublic).IsTrue())
        do! check(Assert.That(dependsOnModulesWithAttributeAttributeType.IsGenericTypeDefinition).IsTrue())

        let closedType = typeof<DependsOnModulesWithAttributeAttribute<ObsoleteAttribute>>
        do! check(Assert.That(closedType.IsSubclassOf(typeof<DependsOnBaseAttribute>)).IsTrue())
    }

    [<Test>]
    member _.ITaggedModule_IsAccessibleFromModulesNamespace() = async {
        // Verify ITaggedModule is in ModularPipelines.Modules namespace
        let taggedModuleType = typeof<ITaggedModule>

        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(taggedModuleType.Namespace),
                "ModularPipelines.Modules"
            )
        )
        do! check(Assert.That(taggedModuleType.IsPublic).IsTrue())
        do! check(Assert.That(taggedModuleType.IsInterface).IsTrue())
    }

    [<Test>]
    member _.IModuleRegistrationBuilder_IsAccessibleFromDependencyInjectionNamespace() = async {
        // Verify IModuleRegistrationBuilder is in ModularPipelines.DependencyInjection namespace
        let moduleRegistrationBuilderType = typeof<IModuleRegistrationBuilder>

        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(moduleRegistrationBuilderType.Namespace),
                "ModularPipelines.DependencyInjection"
            )
        )
        do! check(Assert.That(moduleRegistrationBuilderType.IsPublic).IsTrue())
        do! check(Assert.That(moduleRegistrationBuilderType.IsInterface).IsTrue())
    }

    [<Test>]
    member _.AllFlexibleDependencyAttributes_HaveCorrectAttributeUsage() = async {
        // Verify all dependency attributes allow multiple usage and inheritance
        let dependencyAttributes =
            [|
                typeof<ModuleTagAttribute>
                typeof<DependsOnModulesWithTagAttribute>
                typeof<DependsOnModulesInCategoryAttribute>
                typeof<DependsOnModulesWithAttributeAttribute<_>>
            |]

        for attrType in dependencyAttributes do
            let usage =
                attrType.GetCustomAttributes(typeof<AttributeUsageAttribute>, false)
                |> Array.choose (function
                    | :? AttributeUsageAttribute as attributeUsage -> Some attributeUsage
                    | _ -> None)
                |> Array.tryHead

            do! check(Assert.That(usage.IsSome).IsTrue())

            match usage with
            | Some attributeUsage ->
                do! check(Assert.That(attributeUsage.AllowMultiple).IsTrue())
                do! check(Assert.That(attributeUsage.Inherited).IsTrue())
            | None -> ()
    }

    [<Test>]
    member _.ModuleCategoryAttribute_DoesNotAllowMultiple() = async {
        // Verify ModuleCategoryAttribute does NOT allow multiple (only one category per module)
        let usage =
            typeof<ModuleCategoryAttribute>.GetCustomAttributes(typeof<AttributeUsageAttribute>, false)
            |> Array.choose (function
                | :? AttributeUsageAttribute as attributeUsage -> Some attributeUsage
                | _ -> None)
            |> Array.tryHead

        do! check(Assert.That(usage.IsSome).IsTrue())

        match usage with
        | Some attributeUsage ->
            do! check(Assert.That(attributeUsage.AllowMultiple).IsFalse())
            do! check(Assert.That(attributeUsage.Inherited).IsTrue())
        | None -> ()
    }

    [<Test>]
    member _.IDependencyContext_HasExpectedMethods() = async {
        // Verify IDependencyContext has all required methods for dependency resolution
        let dependencyContextType = typeof<IDependencyContext>
        let methods = dependencyContextType.GetMethods()

        do! check(Assert.That(methods |> Array.exists (fun m -> m.Name = "GetTags")).IsTrue())
        do! check(Assert.That(methods |> Array.exists (fun m -> m.Name = "GetCategory")).IsTrue())
        do! check(Assert.That(methods |> Array.exists (fun m -> m.Name = "HasAttribute")).IsTrue())
        do! check(Assert.That(methods |> Array.exists (fun m -> m.Name = "GetAttribute")).IsTrue())
        do! check(Assert.That(methods |> Array.exists (fun m -> m.Name = "GetAttributes")).IsTrue())
    }

    [<Test>]
    member _.ITaggedModule_HasExpectedProperties() = async {
        // Verify ITaggedModule has all required properties
        let taggedModuleType = typeof<ITaggedModule>
        let properties = taggedModuleType.GetProperties()

        do! check(Assert.That(properties |> Array.exists (fun p -> p.Name = "Tags")).IsTrue())
        do! check(Assert.That(properties |> Array.exists (fun p -> p.Name = "Category")).IsTrue())
    }

    [<Test>]
    member _.IModuleRegistrationBuilder_HasExpectedMembers() = async {
        // Verify IModuleRegistrationBuilder has all required members
        let moduleRegistrationBuilderType = typeof<IModuleRegistrationBuilder>
        let properties = moduleRegistrationBuilderType.GetProperties()
        let methods = moduleRegistrationBuilderType.GetMethods()

        do! check(Assert.That(properties |> Array.exists (fun p -> p.Name = "Services")).IsTrue())
        do! check(Assert.That(methods |> Array.exists (fun m -> m.Name = "WithTags")).IsTrue())
        do! check(Assert.That(methods |> Array.exists (fun m -> m.Name = "WithCategory")).IsTrue())
    }