namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System
open System.Reflection
open ModularPipelines.Attributes
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type ModuleTagAttributeTests() =
    [<Test>]
    member _.Constructor_WithValidTag_SetsTagProperty() = async {
        let attr = ModuleTagAttribute("database")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attr.Tag), "database"))
    }

    [<Test>]
    member _.Constructor_WithNullTag_ThrowsArgumentException() = async {
        let mutable threw = false

        try
            ModuleTagAttribute(null) |> ignore
        with :? ArgumentException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Constructor_WithEmptyTag_ThrowsArgumentException() = async {
        let mutable threw = false

        try
            ModuleTagAttribute(String.Empty) |> ignore
        with :? ArgumentException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Constructor_WithWhitespaceTag_ThrowsArgumentException() = async {
        let mutable threw = false

        try
            ModuleTagAttribute("   ") |> ignore
        with :? ArgumentException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Attribute_AllowsMultiple() = async {
        let usage = typeof<ModuleTagAttribute>.GetCustomAttribute<AttributeUsageAttribute>()
        do! check(Assert.That(usage.AllowMultiple).IsTrue())
    }

    [<Test>]
    member _.Attribute_IsInheritable() = async {
        let usage = typeof<ModuleTagAttribute>.GetCustomAttribute<AttributeUsageAttribute>()
        do! check(Assert.That(usage.Inherited).IsTrue())
    }
