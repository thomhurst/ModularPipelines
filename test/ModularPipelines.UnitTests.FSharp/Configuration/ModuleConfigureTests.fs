namespace ModularPipelines.UnitTests.FSharp.Configuration

open System
open System.Threading
open System.Threading.Tasks
open ModularPipelines.Configuration
open ModularPipelines.Context
open ModularPipelines.Modules
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type private TestModule() =
    inherit Module<string>()

    override _.ExecuteAsync(_, _) =
        Task.FromResult("test")

type private ConfiguredModule() =
    inherit Module<string>()

    override _.Configure() =
        ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromSeconds(60.0))
            .WithAlwaysRun()
            .Build()

    override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
        Task.FromResult("test")

type ModuleConfigureTests() =
    [<Test>]
    member _.Module_DefaultConfiguration_ReturnsDefault() = async {
        let testModule = TestModule()
        let config = (testModule :> IModule).Configuration
        let isDefaultConfig = obj.ReferenceEquals(config, ModuleConfiguration.Default)

        do! check(Assert.That<bool>(isDefaultConfig).IsTrue())
    }

    [<Test>]
    member _.Module_OverriddenConfigure_ReturnsCustomConfig() = async {
        let configuredModule = ConfiguredModule()
        let config = (configuredModule :> IModule).Configuration

        do! check(Assert.That(config.Timeout.HasValue).IsTrue())
        do! check(Assert.That(config.Timeout.Value = TimeSpan.FromSeconds(60.0)).IsTrue())
        do! check(Assert.That(config.AlwaysRun).IsTrue())
    }

    [<Test>]
    member _.Module_Configuration_IsCached() = async {
        let configuredModule = ConfiguredModule()
        let config1 = (configuredModule :> IModule).Configuration
        let config2 = (configuredModule :> IModule).Configuration
        let isCachedReference = obj.ReferenceEquals(config1, config2)

        do! check(Assert.That<bool>(isCachedReference).IsTrue())
    }
