namespace ModularPipelines.UnitTests.FSharp

open ModularPipelines.TestHelpers
open System.Collections.Generic

type GlobalDummyModule() =
    inherit SimpleTestModule<IDictionary<string, obj> | null>()

    override _.Result : IDictionary<string, obj> | null = null