namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System
open ModularPipelines.Attributes
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type CategoryAttributeTestModule() =
    class
    end

type DependsOnModulesInCategoryAttributeTests() =
    [<Test>]
    member _.ShouldDependOn_ModuleInCategory_ReturnsTrue() = async {
        let attr = DependsOnModulesInCategoryAttribute("infrastructure")
        let context = MockDependencyContext().WithCategory(typeof<CategoryAttributeTestModule>, "infrastructure")
        let result = attr.ShouldDependOn(typeof<CategoryAttributeTestModule>, context)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.ShouldDependOn_ModuleInDifferentCategory_ReturnsFalse() = async {
        let attr = DependsOnModulesInCategoryAttribute("infrastructure")
        let context = MockDependencyContext().WithCategory(typeof<CategoryAttributeTestModule>, "build")
        let result = attr.ShouldDependOn(typeof<CategoryAttributeTestModule>, context)
        do! check(Assert.That(result).IsFalse())
    }

    [<Test>]
    member _.ShouldDependOn_CaseInsensitive_ReturnsTrue() = async {
        let attr = DependsOnModulesInCategoryAttribute("INFRASTRUCTURE")
        let context = MockDependencyContext().WithCategory(typeof<CategoryAttributeTestModule>, "infrastructure")
        let result = attr.ShouldDependOn(typeof<CategoryAttributeTestModule>, context)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.ShouldDependOn_ModuleHasNoCategory_ReturnsFalse() = async {
        let attr = DependsOnModulesInCategoryAttribute("infrastructure")
        let context = MockDependencyContext()
        let result = attr.ShouldDependOn(typeof<CategoryAttributeTestModule>, context)
        do! check(Assert.That(result).IsFalse())
    }

    [<Test>]
    member _.ShouldDependOn_CategoryMatchesExactly_ReturnsTrue() = async {
        let attr = DependsOnModulesInCategoryAttribute("build-pipeline")
        let context = MockDependencyContext().WithCategory(typeof<CategoryAttributeTestModule>, "build-pipeline")
        let result = attr.ShouldDependOn(typeof<CategoryAttributeTestModule>, context)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.Constructor_WithNullCategory_ThrowsArgumentException() = async {
        let mutable threw = false

        try
            DependsOnModulesInCategoryAttribute(null) |> ignore
        with :? ArgumentException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Constructor_WithEmptyCategory_ThrowsArgumentException() = async {
        let mutable threw = false

        try
            DependsOnModulesInCategoryAttribute(String.Empty) |> ignore
        with :? ArgumentException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Constructor_WithWhitespaceCategory_ThrowsArgumentException() = async {
        let mutable threw = false

        try
            DependsOnModulesInCategoryAttribute("   ") |> ignore
        with :? ArgumentException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Category_Property_ReturnsConstructorValue() = async {
        let attr = DependsOnModulesInCategoryAttribute("my-category")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attr.Category), "my-category"))
    }
