namespace ModularPipelines.UnitTests.FSharp.Models

open ModularPipelines.Models
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type RequirementDecisionTests() =
    [<Test>]
    member _.True_Implicit_Cast() = async {
        let requirementDecision: RequirementDecision = true
        do! check(Assert.That(requirementDecision.Success).IsTrue())
        do! check(Assert.That(requirementDecision.Reason).IsNull())
    }

    [<Test>]
    member _.False_Implicit_Cast() = async {
        let requirementDecision: RequirementDecision = false
        do! check(Assert.That(requirementDecision.Success).IsFalse())
        do! check(Assert.That(requirementDecision.Reason).IsNull())
    }

    [<Test>]
    member _.String_Implicit_Cast() = async {
        let requirementDecision: RequirementDecision = "Foo!"
        do! check(Assert.That(requirementDecision.Success).IsFalse())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(requirementDecision.Reason), "Foo!"))
    }

    [<Test>]
    member _.Failed() = async {
        let requirementDecision = RequirementDecision.Failed("Blah!")
        do! check(Assert.That(requirementDecision.Success).IsFalse())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(requirementDecision.Reason), "Blah!"))
    }

    [<Test>]
    member _.Passed() = async {
        let requirementDecision = RequirementDecision.Passed
        do! check(Assert.That(requirementDecision.Success).IsTrue())
        do! check(Assert.That(requirementDecision.Reason).IsNull())
    }

    [<Test>]
    [<Arguments(true)>]
    [<Arguments(false)>]
    member _.Of(success: bool) = async {
        let requirementDecision = RequirementDecision.Of(success, "Blah!")

        if success then
            do! check(Assert.That(requirementDecision.Success).IsTrue())
            do! check(Assert.That(requirementDecision.Reason).IsNull())
        else
            do! check(Assert.That(requirementDecision.Success).IsFalse())
            do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(requirementDecision.Reason), "Blah!"))
    }
