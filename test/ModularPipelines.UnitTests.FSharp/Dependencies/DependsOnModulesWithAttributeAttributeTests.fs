namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System
open ModularPipelines.Attributes
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

[<AttributeUsage(AttributeTargets.Class)>]
type CriticalAttribute() =
    inherit Attribute()

[<AttributeUsage(AttributeTargets.Class)>]
type OtherAttribute() =
    inherit Attribute()

[<AttributeUsage(AttributeTargets.Class, Inherited = true)>]
type InheritableAttribute() =
    inherit Attribute()

[<Critical>]
type CriticalModule() =
    class
    end

type NonCriticalModule() =
    class
    end

[<Inheritable>]
type BaseModuleWithInheritableAttribute() =
    class
    end

type DerivedModuleWithInheritedAttribute() =
    inherit BaseModuleWithInheritableAttribute()

[<Other>]
type ModuleWithDifferentAttribute() =
    class
    end

[<Critical>]
[<Other>]
type ModuleWithMultipleAttributes() =
    class
    end

[<Serializable>]
type SerializableModule() =
    class
    end

type DependsOnModulesWithAttributeAttributeTests() =
    [<Test>]
    member _.ShouldDependOn_ModuleHasAttribute_ReturnsTrue() = async {
        let attr = DependsOnModulesWithAttributeAttribute<CriticalAttribute>()
        let context = MockDependencyContext()
        let result = attr.ShouldDependOn(typeof<CriticalModule>, context)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.ShouldDependOn_ModuleLacksAttribute_ReturnsFalse() = async {
        let attr = DependsOnModulesWithAttributeAttribute<CriticalAttribute>()
        let context = MockDependencyContext()
        let result = attr.ShouldDependOn(typeof<NonCriticalModule>, context)
        do! check(Assert.That(result).IsFalse())
    }

    [<Test>]
    member _.ShouldDependOn_ModuleHasInheritedAttribute_ReturnsTrue() = async {
        let attr = DependsOnModulesWithAttributeAttribute<InheritableAttribute>()
        let context = MockDependencyContext()
        let result = attr.ShouldDependOn(typeof<DerivedModuleWithInheritedAttribute>, context)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.ShouldDependOn_ModuleHasDifferentAttribute_ReturnsFalse() = async {
        let attr = DependsOnModulesWithAttributeAttribute<CriticalAttribute>()
        let context = MockDependencyContext()
        let result = attr.ShouldDependOn(typeof<ModuleWithDifferentAttribute>, context)
        do! check(Assert.That(result).IsFalse())
    }

    [<Test>]
    member _.ShouldDependOn_ModuleHasMultipleAttributesIncludingMatch_ReturnsTrue() = async {
        let attr = DependsOnModulesWithAttributeAttribute<CriticalAttribute>()
        let context = MockDependencyContext()
        let result = attr.ShouldDependOn(typeof<ModuleWithMultipleAttributes>, context)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.ShouldDependOn_CheckingForSerializableAttribute_ReturnsTrue() = async {
        let attr = DependsOnModulesWithAttributeAttribute<SerializableAttribute>()
        let context = MockDependencyContext()
        let result = attr.ShouldDependOn(typeof<SerializableModule>, context)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.ShouldDependOn_CheckingForSerializableAttribute_ReturnsFalse() = async {
        let attr = DependsOnModulesWithAttributeAttribute<SerializableAttribute>()
        let context = MockDependencyContext()
        let result = attr.ShouldDependOn(typeof<NonCriticalModule>, context)
        do! check(Assert.That(result).IsFalse())
    }
