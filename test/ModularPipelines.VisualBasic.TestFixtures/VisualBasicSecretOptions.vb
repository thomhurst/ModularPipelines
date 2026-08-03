Imports ModularPipelines.Attributes

Namespace ModularPipelines.VisualBasic.TestFixtures
    Public NotInheritable Class VisualBasicSecretOptions
        <SecretValue>
        Public Property Token As String = "visual-basic-secret"
    End Class
End Namespace
