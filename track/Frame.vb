Imports IrrlichtNETCP

Public Class Frame
    Public ImageCount As Integer = 16
    Public Rotation As Single 'required, better than doing problems....
    Public Image As Image = Nothing
    Public Tex As Short

    Public Delay! = 30
    Public UV(3) As Vector2D
    Public Cloned As Boolean = False

    Sub New()
        UV(0) = New Vector2D(0, 0)
        UV(1) = New Vector2D(1, 0)
        UV(2) = New Vector2D(1, 1)
        UV(3) = New Vector2D(0, 1)
    End Sub
End Class