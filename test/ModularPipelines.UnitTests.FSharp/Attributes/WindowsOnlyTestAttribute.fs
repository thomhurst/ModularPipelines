namespace ModularPipelines.UnitTests.FSharp.Attributes

open System
open System.Threading.Tasks
open TUnit.Core

type WindowsOnlyTestAttribute() =
    inherit SkipAttribute("Windows only test")
    override _.ShouldSkip(_: TestRegisteredContext) =
        Task.FromResult(not (OperatingSystem.IsWindows()))
