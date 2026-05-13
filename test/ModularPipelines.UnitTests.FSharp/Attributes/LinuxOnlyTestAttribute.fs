namespace ModularPipelines.UnitTests.FSharp.Attributes

open System
open System.Threading.Tasks
open TUnit.Core

type LinuxOnlyTestAttribute() =
    inherit SkipAttribute("Linux only test")
    override _.ShouldSkip(_: TestRegisteredContext) =
        Task.FromResult(not (OperatingSystem.IsLinux()))
