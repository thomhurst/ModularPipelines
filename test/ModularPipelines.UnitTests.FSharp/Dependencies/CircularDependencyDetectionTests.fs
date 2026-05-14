namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System
open System.Collections.Generic
open System.Threading.Tasks
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.Engine
open ModularPipelines.Exceptions
open ModularPipelines.Modules
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

[<DependsOn(typeof<DirectCycleModuleB>)>]
type DirectCycleModuleA() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)
and [<DependsOn(typeof<DirectCycleModuleA>)>] DirectCycleModuleB() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)

[<DependsOn(typeof<TripleCycleModuleB>)>]
type TripleCycleModuleA() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)
and [<DependsOn(typeof<TripleCycleModuleC>)>] TripleCycleModuleB() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)
and [<DependsOn(typeof<TripleCycleModuleA>)>] TripleCycleModuleC() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)

[<DependsOn(typeof<LinearModuleB>)>]
type LinearModuleA() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)
and [<DependsOn(typeof<LinearModuleC>)>] LinearModuleB() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)
and LinearModuleC() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)

type IndependentModuleA() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)

type IndependentModuleB() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)

type ComplexGraphRoot() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)

[<DependsOn(typeof<ComplexGraphRoot>)>]
[<DependsOn(typeof<ComplexGraphCycleB>)>]
type ComplexGraphCycleA() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)
and [<DependsOn(typeof<ComplexGraphCycleA>)>] ComplexGraphCycleB() =
    inherit Module<bool>()
    override _.ExecuteAsync(_, _) = Task.FromResult(true)

type CircularDependencyDetectionTests() =
    [<Test>]
    member _.ValidateNoCycles_WithDirectCycle_ThrowsCircularDependencyException() = async {
        let mutable threw = false

        try
            DependencyGraphValidator.ValidateNoCycles([| typeof<DirectCycleModuleA>; typeof<DirectCycleModuleB> |])
        with :? CircularDependencyException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.ValidateNoCycles_WithTripleCycle_ThrowsCircularDependencyException() = async {
        let mutable threw = false

        try
            DependencyGraphValidator.ValidateNoCycles([| typeof<TripleCycleModuleA>; typeof<TripleCycleModuleB>; typeof<TripleCycleModuleC> |])
        with :? CircularDependencyException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.ValidateNoCycles_WithLinearChain_DoesNotThrow() = async {
        let mutable threw = false

        try
            DependencyGraphValidator.ValidateNoCycles([| typeof<LinearModuleA>; typeof<LinearModuleB>; typeof<LinearModuleC> |])
        with _ ->
            threw <- true

        do! check(Assert.That(threw).IsFalse())
    }

    [<Test>]
    member _.ValidateNoCycles_WithIndependentModules_DoesNotThrow() = async {
        let mutable threw = false

        try
            DependencyGraphValidator.ValidateNoCycles([| typeof<IndependentModuleA>; typeof<IndependentModuleB> |])
        with _ ->
            threw <- true

        do! check(Assert.That(threw).IsFalse())
    }

    [<Test>]
    member _.ValidateNoCycles_WithEmptyCollection_DoesNotThrow() = async {
        let mutable threw = false

        try
            DependencyGraphValidator.ValidateNoCycles(Array.empty<Type>)
        with _ ->
            threw <- true

        do! check(Assert.That(threw).IsFalse())
    }

    [<Test>]
    member _.ValidateNoCycles_WithComplexGraphContainingCycle_ThrowsCircularDependencyException() = async {
        let mutable threw = false

        try
            DependencyGraphValidator.ValidateNoCycles([| typeof<ComplexGraphRoot>; typeof<ComplexGraphCycleA>; typeof<ComplexGraphCycleB> |])
        with :? CircularDependencyException ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.ValidateNoCycles_ExceptionContainsCycleTypes() = async {
        let mutable cycleTypes: IReadOnlyList<Type> = null

        try
            DependencyGraphValidator.ValidateNoCycles([| typeof<DirectCycleModuleA>; typeof<DirectCycleModuleB> |])
        with :? CircularDependencyException as ex ->
            cycleTypes <- ex.CycleTypes

        do! check(Assert.That(obj.ReferenceEquals(cycleTypes, null) = false).IsTrue())
        do! check(Assert.That(cycleTypes.Count >= 2).IsTrue())
    }

    [<Test>]
    member _.ValidateNoCycles_ExceptionMessageShowsCyclePath() = async {
        let mutable message = null

        try
            DependencyGraphValidator.ValidateNoCycles([| typeof<DirectCycleModuleA>; typeof<DirectCycleModuleB> |])
        with :? CircularDependencyException as ex ->
            message <- ex.Message

        do! check(Assert.That(message).IsNotNull())
        do! check(Assert.That(message.Contains("->")).IsTrue())
        do! check(Assert.That(message.Contains("Circular dependency detected")).IsTrue())
    }

    [<Test>]
    member _.AddModulesFromAssembly_WithCircularDependency_ThrowsAtRegistrationTime() = async {
        let mutable message = null

        try
            DependencyGraphValidator.ValidateNoCycles([| typeof<DirectCycleModuleA>; typeof<DirectCycleModuleB> |])
        with :? CircularDependencyException as ex ->
            message <- ex.Message

        do! check(Assert.That(message).IsNotNull())
        do! check(Assert.That(message.Contains("Circular dependency detected at registration")).IsTrue())
    }

    [<Test>]
    member _.CreateWithCyclePath_FormatsMessageCorrectly() = async {
        let cyclePath = List<Type>([ typeof<DirectCycleModuleA>; typeof<DirectCycleModuleB>; typeof<DirectCycleModuleA> ])
        let cycleException = CircularDependencyException.CreateWithCyclePath(cyclePath)

        do! check(Assert.That(cycleException.Message.Contains("**DirectCycleModuleA**")).IsTrue())
        do! check(Assert.That(cycleException.Message.Contains("->")).IsTrue())
        do! check(Assert.That(Seq.toList cycleException.CycleTypes = Seq.toList cyclePath).IsTrue())
    }

    [<Test>]
    member _.CreateWithCyclePath_HighlightsStartAndEndOfCycle() = async {
        let cyclePath =
            List<Type>([
                typeof<TripleCycleModuleA>
                typeof<TripleCycleModuleB>
                typeof<TripleCycleModuleC>
                typeof<TripleCycleModuleA>
            ])

        let cycleException = CircularDependencyException.CreateWithCyclePath(cyclePath)

        do! check(Assert.That(cycleException.Message.Contains("**TripleCycleModuleA**")).IsTrue())
    }
