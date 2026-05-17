namespace ModularPipelines.UnitTests.FSharp.Attributes

open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.Options
open ModularPipelines.Context
open ModularPipelines.Engine.Dependencies
open ModularPipelines.Modules
open ModularPipelines.Options
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module ModuleMetadataRegistryTests =
    type ModuleA() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            Task.FromResult<string>("A")

    let createRegistry () =
        ModuleMetadataRegistry(Options.Create(ModuleRegistrationOptions()))

type ModuleMetadataRegistryTests() =
    [<Test>]
    member _.SetMetadata_GetMetadata_ReturnsValue() = async {
        let registry = ModuleMetadataRegistryTests.createRegistry()
        registry.SetMetadata(typeof<ModuleMetadataRegistryTests.ModuleA>, "key", "value")
        let result = registry.GetMetadata<string>(typeof<ModuleMetadataRegistryTests.ModuleA>, "key")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result), "value"))
    }

    [<Test>]
    member _.GetMetadata_NotSet_ReturnsNull() = async {
        let registry = ModuleMetadataRegistryTests.createRegistry()
        let result = registry.GetMetadata<string>(typeof<ModuleMetadataRegistryTests.ModuleA>, "key")
        do! check(Assert.That(result).IsNull())
    }

    [<Test>]
    member _.SetMetadata_OverwritesExisting() = async {
        let registry = ModuleMetadataRegistryTests.createRegistry()
        registry.SetMetadata(typeof<ModuleMetadataRegistryTests.ModuleA>, "key", "value1")
        registry.SetMetadata(typeof<ModuleMetadataRegistryTests.ModuleA>, "key", "value2")
        let result = registry.GetMetadata<string>(typeof<ModuleMetadataRegistryTests.ModuleA>, "key")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result), "value2"))
    }
