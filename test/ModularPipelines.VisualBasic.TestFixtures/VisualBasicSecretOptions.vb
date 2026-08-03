Imports ModularPipelines.Attributes

Namespace ModularPipelines.VisualBasic.TestFixtures
    Public NotInheritable Class VisualBasicSecretOptions
        <SecretValue>
        Public Property Token As String = "visual-basic-secret"
    End Class

    Public NotInheritable Class VisualBasicCommandOptions
        <CliOption("--value")>
        Public Property Value As String = "visual-basic-value"
    End Class
End Namespace
