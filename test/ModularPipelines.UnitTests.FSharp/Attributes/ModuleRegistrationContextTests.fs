namespace ModularPipelines.UnitTests.FSharp.Attributes

open System
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Options
open ModularPipelines.Attributes.Events
open ModularPipelines.Context
open ModularPipelines.Engine.Dependencies
open ModularPipelines.Modules
open ModularPipelines.Options
open Moq
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module ModuleRegistrationContextTests =
    type ModuleA() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("A")

    type ModuleB() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("B")

    let createContext
            (moduleType: Type)
            (dependencyRegistry: IModuleDependencyRegistry option)
            (metadataRegistry: IModuleMetadataRegistry option)
            (registeredModules: Type list option) =
        let configuration = Mock.Of<IConfiguration>()
        let environment = Mock.Of<IHostEnvironment>()
        let services = ServiceCollection()
        let depReg = defaultArg dependencyRegistry (ModuleDependencyRegistry() :> IModuleDependencyRegistry)
        let metaReg = defaultArg metadataRegistry (ModuleMetadataRegistry(Options.Create(ModuleRegistrationOptions())) :> IModuleMetadataRegistry)
        let regModules = defaultArg registeredModules [ moduleType ] |> List.toArray :> System.Collections.Generic.IReadOnlyList<Type>
        ModuleRegistrationContext(
            moduleType,
            moduleType.GetCustomAttributes(true) |> Array.choose (function :? Attribute as a -> Some a | _ -> None) |> Array.toList :> System.Collections.Generic.IReadOnlyList<Attribute>,
            configuration,
            environment,
            regModules,
            services,
            depReg,
            metaReg)

type ModuleRegistrationContextTests() =
    [<Test>]
    member _.ModuleType_ReturnsCorrectType() = async {
        let context = ModuleRegistrationContextTests.createContext typeof<ModuleRegistrationContextTests.ModuleA> None None None
        do! check(Assert.That(context.ModuleType).IsEqualTo(typeof<ModuleRegistrationContextTests.ModuleA>))
    }

    [<Test>]
    member _.AddDependency_AddsToDependencyRegistry() = async {
        let dependencyRegistry = ModuleDependencyRegistry()
        let context = ModuleRegistrationContextTests.createContext typeof<ModuleRegistrationContextTests.ModuleA> (Some (dependencyRegistry :> IModuleDependencyRegistry)) None None
        context.AddDependency<ModuleRegistrationContextTests.ModuleB>()
        let dependencies = dependencyRegistry.GetDynamicDependencies(typeof<ModuleRegistrationContextTests.ModuleA>)
        do! check(Assert.That(dependencies |> Seq.contains typeof<ModuleRegistrationContextTests.ModuleB>).IsTrue())
    }

    [<Test>]
    member _.IsModuleRegistered_RegisteredModule_ReturnsTrue() = async {
        let registeredModules = [ typeof<ModuleRegistrationContextTests.ModuleA>; typeof<ModuleRegistrationContextTests.ModuleB> ]
        let context = ModuleRegistrationContextTests.createContext typeof<ModuleRegistrationContextTests.ModuleA> None None (Some registeredModules)
        do! check(Assert.That(context.IsModuleRegistered<ModuleRegistrationContextTests.ModuleB>()).IsTrue())
    }

    [<Test>]
    member _.IsModuleRegistered_UnregisteredModule_ReturnsFalse() = async {
        let registeredModules = [ typeof<ModuleRegistrationContextTests.ModuleA> ]
        let context = ModuleRegistrationContextTests.createContext typeof<ModuleRegistrationContextTests.ModuleA> None None (Some registeredModules)
        do! check(Assert.That(context.IsModuleRegistered<ModuleRegistrationContextTests.ModuleB>()).IsFalse())
    }

    [<Test>]
    member _.SetMetadata_GetMetadata_RoundTrips() = async {
        let metadataRegistry = ModuleMetadataRegistry(Options.Create(ModuleRegistrationOptions()))
        let context = ModuleRegistrationContextTests.createContext typeof<ModuleRegistrationContextTests.ModuleA> None (Some (metadataRegistry :> IModuleMetadataRegistry)) None
        context.SetMetadata("key", "value")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(context.GetMetadata<string>("key")), "value"))
    }
