namespace ModularPipelines.UnitTests.FSharp.Attributes

open System
open System.Threading
open System.Threading.Tasks
open ModularPipelines.Attributes.Events
open ModularPipelines.Context
open ModularPipelines.Enums
open ModularPipelines.Extensions
open ModularPipelines.Models
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module MetadataCrossPhaseIntegrationTests =
    let eventLog = ResizeArray<string>()

    type SetMetadataOnRegistrationAttribute(key: string, value: string) =
        inherit Attribute()
        interface IModuleRegistrationEventReceiver with
            member _.OnRegistrationAsync(context: IModuleRegistrationContext) =
                context.SetMetadata(key, value)
                eventLog.Add($"Registration:SetMetadata:{key}={value}")
                Task.CompletedTask

    type ReadMetadataOnStartAttribute(key: string) =
        inherit Attribute()
        interface IModuleStartHandler with
            member _.ContinueOnError = false
            member _.OnModuleStartAsync(context: IModuleHookContext) =
                let value = context.GetMetadata<string>(key)
                let valueText = if value = null then "null" else value
                eventLog.Add(sprintf "Start:ReadMetadata:%s=%s" key valueText)
                Task.CompletedTask

    type ReadMetadataOnEndAttribute(key: string) =
        inherit Attribute()
        interface IModuleEndHandler with
            member _.ContinueOnError = false
            member _.OnModuleEndAsync(context: IModuleHookContext, _: IModuleResult) =
                let value = context.GetMetadata<string>(key)
                let valueText = if value = null then "null" else value
                eventLog.Add(sprintf "End:ReadMetadata:%s=%s" key valueText)
                Task.CompletedTask

    [<SetMetadataOnRegistration("config", "value-from-registration")>]
    [<ReadMetadataOnStart("config")>]
    [<ReadMetadataOnEnd("config")>]
    type MetadataModule() =
        inherit Module<string>()
        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "Done"
            }

[<NotInParallel(nameof MetadataCrossPhaseIntegrationTests)>]
type MetadataCrossPhaseIntegrationTests() =
    inherit TestBase()

    [<Before(HookType.Test)>]
    member _.ClearEventLog() =
        MetadataCrossPhaseIntegrationTests.eventLog.Clear()

    [<Test>]
    member _.Metadata_SetDuringRegistration_AvailableDuringLifecycleEvents() = async {
        let! result =
            TestPipelineHostBuilder.Create()
                .AddModule<MetadataCrossPhaseIntegrationTests.MetadataModule>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(result.Status = Status.Successful).IsTrue())
        do! check(Assert.That(MetadataCrossPhaseIntegrationTests.eventLog |> Seq.contains "Registration:SetMetadata:config=value-from-registration").IsTrue())
        do! check(Assert.That(MetadataCrossPhaseIntegrationTests.eventLog |> Seq.contains "Start:ReadMetadata:config=value-from-registration").IsTrue())
        do! check(Assert.That(MetadataCrossPhaseIntegrationTests.eventLog |> Seq.contains "End:ReadMetadata:config=value-from-registration").IsTrue())
    }
