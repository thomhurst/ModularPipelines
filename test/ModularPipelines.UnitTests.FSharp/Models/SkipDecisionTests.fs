namespace ModularPipelines.UnitTests.FSharp.Models

open ModularPipelines.Models
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type SkipDecisionTests() =
    [<Test>]
    member _.True_Implicit_Cast() = async {
        let skipDecision: SkipDecision = true
        do! check(Assert.That(skipDecision.ShouldSkip).IsTrue())
        do! check(Assert.That(skipDecision.Reason).IsNull())
    }

    [<Test>]
    member _.String_Implicit_Cast() = async {
        let skipDecision: SkipDecision = "Foo!"
        do! check(Assert.That(skipDecision.ShouldSkip).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(skipDecision.Reason), "Foo!"))
    }

    [<Test>]
    member _.False_Implicit_Cast() = async {
        let skipDecision: SkipDecision = false
        do! check(Assert.That(skipDecision.ShouldSkip).IsFalse())
        do! check(Assert.That(skipDecision.Reason).IsNull())
    }

    [<Test>]
    member _.Skip() = async {
        let skipDecision = SkipDecision.Skip("Blah!")
        do! check(Assert.That(skipDecision.ShouldSkip).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(skipDecision.Reason), "Blah!"))
    }

    [<Test>]
    member _.DoNotSkip() = async {
        let skipDecision = SkipDecision.DoNotSkip
        do! check(Assert.That(skipDecision.ShouldSkip).IsFalse())
        do! check(Assert.That(skipDecision.Reason).IsNull())
    }

    [<Test>]
    [<Arguments(true)>]
    [<Arguments(false)>]
    member _.Of(shouldSkip: bool) = async {
        let skipDecision = SkipDecision.Of(shouldSkip, "Blah!")

        if shouldSkip then
            do! check(Assert.That(skipDecision.ShouldSkip).IsTrue())
            do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(skipDecision.Reason), "Blah!"))
        else
            do! check(Assert.That(skipDecision.ShouldSkip).IsFalse())
            do! check(Assert.That(skipDecision.Reason).IsNull())
    }
