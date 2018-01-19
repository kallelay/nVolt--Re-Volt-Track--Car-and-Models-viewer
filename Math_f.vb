Imports IrrlichtNETCP
Imports nVOLT.Car_Model

Module Math_functions
    Public Const Epsilon = Single.Epsilon
    '4 positions
    Public Function getCenter(ByVal BB As BBOX) As Vector3D
        Return New Vector3D(BB.minX / 2 + BB.maxX / 2, BB.minY / 2 + BB.maxY / 2, BB.maxZ / 2 + BB.minZ / 2)
    End Function
    Public Function getBBoxVectors(ByVal BB As BBOX) As Vector3D
        Return New Vector3D(BB.maxX - BB.minX, BB.maxY - BB.minY, BB.maxZ - BB.minZ)
    End Function
    Public Function getVecLength(ByVal v As Vector3D) As Single
        Return v.Length
    End Function

    Public Function isNumber(ByVal S As Single) As Boolean
        Return S <> Single.NaN
    End Function
    Public Function isNumber(ByVal D As Double) As Boolean
        Return Not (Double.IsNaN(D))
    End Function
    Public Function isNumber(ByVal I As Integer) As Boolean
        Return I <> Single.NaN
    End Function
    Function getPositionBlock(ByVal camerapos As Vector3D, ByVal center As Vector3D) As Integer


        '[  \1/0
        '[ 2/3\
        'setting X,Z
        Dim x, z As Single
        x = camerapos.X - center.X + 1.0E-66 'to avoid /0
        z = camerapos.Z - center.Z

        If x < 0 Then
            If z / x < -1 Then
                Return 3
            ElseIf z / x >= -1 And z / x <= 1 Then
                Return 2
            Else 'z >1
                Return 1
            End If


        ElseIf x >= 0 Then
            If z / x < -1 Then
                Return 1
            ElseIf z / x >= -1 And z / x <= 1 Then
                '  If x >= 0 Then Return 2
                Return 0
            Else 'z >1
                Return 3
            End If

        End If


    End Function
End Module
