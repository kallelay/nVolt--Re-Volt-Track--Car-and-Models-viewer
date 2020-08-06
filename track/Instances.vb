Imports System.IO
Imports IrrlichtNETCP

Module Instances_Loader
    Dim FinCount As Integer

    Public Class FILE_INSTANCE
        Public Name(9) As Char
        Public R, G, B As Char
        Public EnvRGB As Int32
        Public Priority, Flag, Pad(2) As Int16
        Public LodBias As Single
        Public WorldPos As Vector3D

        Public WorldMatrix As Matrix4
        Public center As Vector3D
        Public ScnNode As SceneNode
        Public fullPath As String

        Public myPRM As PRM
        Public Sub Render()

            myPRM = New PRM(Me.fullPath)


            'myPRM.scnNode(0).AbsoluteTransformation = WorldMatrix
            MyPRM.Render()
            myPRM.Move(WorldPos * Zoom)
            myPRM.RotationByMat(WorldMatrix)


        End Sub

    End Class

    Public Instances() As FILE_INSTANCE
    Sub Load_FIN()

        Dim J As New FileStream(Main.WorldFile.Directory & "\" & Main.WorldFile.DirectoryName & ".fin", FileMode.Open)
        Dim X As New BinaryReader(J)

        FinCount = X.ReadInt32
        Console.WriteLine("Count:" & FinCount)

        ReDim Instances(FinCount)
        Dim i&, k&
        For i = 0 To FinCount - 1

            Instances(i) = New FILE_INSTANCE



            Instances(i).Name = X.ReadChars(9)

            Instances(i).Name(8) = Chr(0) 'Force EOS




            Instances(i).R = ChrW(X.ReadByte())
            Instances(i).G = ChrW(X.ReadByte)
            Instances(i).B = ChrW(X.ReadByte)

            Instances(i).EnvRGB = X.ReadInt32
            Instances(i).Priority = X.ReadBoolean
            Instances(i).Flag = X.ReadByte
            Instances(i).Pad(0) = X.ReadByte
            Instances(i).Pad(1) = X.ReadByte
            Instances(i).LodBias = X.ReadSingle
            Instances(i).WorldPos = New Vector3D(X.ReadSingle(), X.ReadSingle(), X.ReadSingle())

            Instances(i).WorldMatrix = New Matrix4
            Dim m(8) As Single
            For k = 0 To 8

                m(k) = X.ReadSingle

                ''If k Mod 3 = 1 Then m(k) *= -1
                '  If k = 2 Then m(k) *= -1
                '  If k = 6 Then m(k) *= -1
                ' If k = 8 Then m(k) *= -1
                Instances(i).WorldMatrix.SetM(k \ 3, k Mod 3, m(k))




            Next
            '  Instances(i).WorldMatrix.RotationDegrees += New Vector3D(180, 0, 0)

            'and loading fullpath of prm
            For p = 0 To 8
                If Instances(i).Name(p) = Chr(0) Then Instances(i).Name(p) = "*"
            Next
            'If InStr(Str(Instances(i).Name), "prm", CompareMethod.Text) = 0 Then
            Instances(i).fullPath = IO.Directory.GetFiles(Main.WorldFile.Directory, Instances(i).Name)(0)
            Instances(i).fullPath = Replace(Instances(i).fullPath, "ncp", "prm", , , CompareMethod.Text)
            ' Else
            '  Instances(i).fullPath = IO.Directory.GetFiles(levels(levelID).FolderPath, Split(Str(Instances(i).Name), "prm", , CompareMethod.Text)(0))(0)
            '  End If

            '   MsgBox(Instances(i).fullPath & ":::::" & Instances(i).Name)
        Next








    End Sub
    Sub RenderInstances()
        Dim i&
        Dim FinalI(27) As FILE_INSTANCE

        Dim C As New MODEL
        ReDim C.polyl(32768)
        ReDim C.vexl(32768)

        For i = 0 To FinCount - 1
            For j = 0 To Instances(i).myPRM.MyModel.polynum
                '      C.polyl()
            Next




        Next


        For i = 0 To FinCount - 1
            For j = 0 To 27

                '    FinalI(j).myPRM.Vx(i).
            Next
            Instances(i).Render()

        Next
    End Sub

End Module
