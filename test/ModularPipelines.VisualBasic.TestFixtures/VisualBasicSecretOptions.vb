Imports ModularPipelines.Attributes
Imports ModularPipelines.Options
Imports ModularPipelines.Secrets

Namespace ModularPipelines.VisualBasic.TestFixtures
    Public NotInheritable Class VisualBasicSecretOptions
        <SecretValue>
        Public Property Token As String = "visual-basic-secret"
    End Class

    Public NotInheritable Class VisualBasicCommandOptions
        <CliOption("--value")>
        Public Property Value As String = "visual-basic-value"
    End Class

    Public Class VisualBasicShadowedCommandBase
        Inherits CommandLineToolOptions

        <CliArgument(0, Required:=True)>
        Public Property Source As String = String.Empty
    End Class

    Public NotInheritable Class VisualBasicShadowedCommandOptions
        Inherits VisualBasicShadowedCommandBase

        Public Shadows Property Source As String
            Get
                Return MyBase.Source
            End Get
            Set(value As String)
                MyBase.Source = value
            End Set
        End Property
    End Class
End Namespace
