namespace ModularPipelines.UnitTests.FSharp.Attributes

open System.Reflection
open ModularPipelines.Attributes
open ModularPipelines.Options
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

[<AbstractClass>]
[<CliTool("git")>]
type private TestGitOptions() =
    inherit CommandLineToolOptions()

[<AbstractClass>]
type private TestGitCommitOptions() =
    inherit TestGitOptions()

[<AbstractClass>]
[<CliTool("test")>]
type private TestOptionsWithAttribute() =
    inherit CommandLineToolOptions()

type CliToolAttributeTests() =
    [<Test>]
    member _.CliToolAttribute_StoresToolName() = async {
        let attribute = CliToolAttribute("git")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attribute.Tool), "git"))
    }

    [<Test>]
    member _.CliToolAttribute_CanBeAppliedToClass() = async {
        let attribute = typeof<TestGitOptions>.GetCustomAttribute<CliToolAttribute>()
        do! check(Assert.That(attribute).IsNotNull())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attribute.Tool), "git"))
    }

    [<Test>]
    member _.CliToolAttribute_ThrowsOnNullOrWhitespace() = async {
        let mutable threw1 = false
        try
            CliToolAttribute(null) |> ignore
        with :? System.ArgumentException ->
            threw1 <- true

        let mutable threw2 = false
        try
            CliToolAttribute("") |> ignore
        with :? System.ArgumentException ->
            threw2 <- true

        let mutable threw3 = false
        try
            CliToolAttribute("   ") |> ignore
        with :? System.ArgumentException ->
            threw3 <- true

        do! check(Assert.That(threw1).IsTrue())
        do! check(Assert.That(threw2).IsTrue())
        do! check(Assert.That(threw3).IsTrue())
    }

    [<Test>]
    member _.CliToolAttribute_IsInheritedByDerivedClasses() = async {
        let attribute = typeof<TestGitCommitOptions>.GetCustomAttribute<CliToolAttribute>(true)
        do! check(Assert.That(attribute).IsNotNull())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attribute.Tool), "git"))
    }
