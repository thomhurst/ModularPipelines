namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System
open System.Reflection
open ModularPipelines.Attributes
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type ModuleCategoryAttributeTests() =
    [<Test>]
    member _.Constructor_WithValidCategory_SetsCategoryProperty() = async {
        let attr = ModuleCategoryAttribute("infrastructure")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attr.Category), "infrastructure"))
    }

    [<Test>]
    member _.Constructor_WithNullCategory_ThrowsArgumentException() = async {
        let mutable threw = false

        try
            ModuleCategoryAttribute(null) |> ignore
        with :? ArgumentException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Constructor_WithEmptyCategory_ThrowsArgumentException() = async {
        let mutable threw = false

        try
            ModuleCategoryAttribute(String.Empty) |> ignore
        with :? ArgumentException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Attribute_DoesNotAllowMultiple() = async {
        let usage = typeof<ModuleCategoryAttribute>.GetCustomAttribute<AttributeUsageAttribute>()
        do! check(Assert.That(usage.AllowMultiple).IsFalse())
    }

    [<Test>]
    member _.Attribute_IsInheritable() = async {
        let usage = typeof<ModuleCategoryAttribute>.GetCustomAttribute<AttributeUsageAttribute>()
        do! check(Assert.That(usage.Inherited).IsTrue())
    }
