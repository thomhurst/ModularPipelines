namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System
open ModularPipelines.Attributes
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type TagAttributeTestModule() =
    class
    end

type DependsOnModulesWithTagAttributeTests() =
    [<Test>]
    member _.ShouldDependOn_ModuleHasTag_ReturnsTrue() = async {
        let attr = DependsOnModulesWithTagAttribute("database")
        let context = MockDependencyContext().WithTags(typeof<TagAttributeTestModule>, "database")
        let result = attr.ShouldDependOn(typeof<TagAttributeTestModule>, context)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.ShouldDependOn_ModuleLacksTag_ReturnsFalse() = async {
        let attr = DependsOnModulesWithTagAttribute("database")
        let context = MockDependencyContext().WithTags(typeof<TagAttributeTestModule>, "other")
        let result = attr.ShouldDependOn(typeof<TagAttributeTestModule>, context)
        do! check(Assert.That(result).IsFalse())
    }

    [<Test>]
    member _.ShouldDependOn_ModuleHasNoTags_ReturnsFalse() = async {
        let attr = DependsOnModulesWithTagAttribute("database")
        let context = MockDependencyContext()
        let result = attr.ShouldDependOn(typeof<TagAttributeTestModule>, context)
        do! check(Assert.That(result).IsFalse())
    }

    [<Test>]
    member _.ShouldDependOn_CaseInsensitive_ReturnsTrue() = async {
        let attr = DependsOnModulesWithTagAttribute("DATABASE")
        let context = MockDependencyContext().WithTags(typeof<TagAttributeTestModule>, "database")
        let result = attr.ShouldDependOn(typeof<TagAttributeTestModule>, context)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.ShouldDependOn_ModuleHasMultipleTagsIncludingMatch_ReturnsTrue() = async {
        let attr = DependsOnModulesWithTagAttribute("database")
        let context = MockDependencyContext().WithTags(typeof<TagAttributeTestModule>, "infrastructure", "database", "critical")
        let result = attr.ShouldDependOn(typeof<TagAttributeTestModule>, context)
        do! check(Assert.That(result).IsTrue())
    }

    [<Test>]
    member _.Constructor_WithNullTag_ThrowsArgumentException() = async {
        let mutable threw = false

        try
            DependsOnModulesWithTagAttribute(null) |> ignore
        with :? ArgumentException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Constructor_WithEmptyTag_ThrowsArgumentException() = async {
        let mutable threw = false

        try
            DependsOnModulesWithTagAttribute(String.Empty) |> ignore
        with :? ArgumentException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Constructor_WithWhitespaceTag_ThrowsArgumentException() = async {
        let mutable threw = false

        try
            DependsOnModulesWithTagAttribute("   ") |> ignore
        with :? ArgumentException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Tag_Property_ReturnsConstructorValue() = async {
        let attr = DependsOnModulesWithTagAttribute("my-tag")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attr.Tag), "my-tag"))
    }
