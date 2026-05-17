namespace ModularPipelines.UnitTests.FSharp

open System
open System.Diagnostics.CodeAnalysis
open TUnit.Core

type GlobalHooks() =
    [<Before(HookType.TestDiscovery)>]
    static member SetUp() =
        Environment.CurrentDirectory <- TestContext.OutputDirectory;

