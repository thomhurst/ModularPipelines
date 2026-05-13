namespace ModularPipelines.UnitTests.FSharp.Models

open ModularPipelines.Attributes

type MyModel() =
    [<SecretValue>]
    member _.MySecret = "This is a secret value!"

    member _.NotASecret = "This is NOT a secret value!"
