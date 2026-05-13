namespace ModularPipelines.UnitTests.FSharp.Validation

open System.Reflection
open ModularPipelines.Context
open ModularPipelines.Validation
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type ValidationInterfaceTests() =
    [<Test>]
    member _.IPipelineValidationService_ShouldBeInternal() = async {
        let assembly = typeof<IModuleContext>.Assembly
        let iface = assembly.GetType("ModularPipelines.Validation.IPipelineValidationService")

        do! check(Assert.That(iface).IsNotNull())
        do! check(Assert.That(iface.IsPublic).IsFalse())
    }

    [<Test>]
    member _.IPipelineValidator_ShouldRemainPublic() = async {
        let validatorType = typeof<IPipelineValidator>
        do! check(Assert.That(validatorType.IsPublic).IsTrue())
    }

    [<Test>]
    member _.IPipelineValidator_ShouldHaveOrderAndValidateMembers() = async {
        let validatorType = typeof<IPipelineValidator>

        let orderProperty = validatorType.GetProperty("Order")
        do! check(Assert.That(orderProperty).IsNotNull())

        let validateMethod = validatorType.GetMethod("Validate")
        do! check(Assert.That(validateMethod).IsNotNull())
    }
