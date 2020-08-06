Imports System.IO
Imports IrrlichtNETCP

' ///////////////////////////////////
' //        File structure         //
' ///////////////////////////////////
' // Last modification August'8th
' /// Hey KDL-myself from 2010.... which August'8th are you talking about,
' //// Yours... KDL-kay from 2020....
' // By theKDL


Module Level_Model
    Public Class PRM
        Public Directory As String
        Public FileName As String
        Public DirectoryName As String
        Public MyModel As New MODEL
        Public scnNode(27) As SceneNode
        Public Vx(27) As MeshBuffer
        Sub Move(ByVal x As Single, ByVal y As Single, ByVal z As Single)
            Dim k&
            For k = 0 To 26
                scnNode(k).Position = New Vector3D(x, -y, z)
            Next
        End Sub
        Sub RotationByMat(ByVal Mat As Matrix4)
            Dim k&
            For k = 0 To 26
                For i = 0 To 3
                    For j = 0 To 3
                        scnNode(k).AbsoluteTransformation.SetM(i, j, Mat.GetM(i, j))
                    Next
                Next
                'scnNode(k).AbsoluteTransformation.SetM = Matrix4.FromUnmanaged(Mat.M)
            Next
            'Console.WriteLine(Mat.RotationDegrees.X & "," & Mat.RotationDegrees.Y & "," & Mat.RotationDegrees.Z)
        End Sub
        Sub Move(ByVal vec As Vector3D)
            Move(vec.X, vec.Y, vec.Z)
        End Sub
        Sub New(ByVal filepath As String)
            Dim old&
            Dim J As New BinaryReader(New FileStream(Replace(filepath, Chr(34), ""), FileMode.Open))


            'Vert/Poly count
            MyModel.polynum = J.ReadInt16()
            MyModel.vertnum = J.ReadInt16()

            doWrite("Poly count:" & Chr(9) & MyModel.polynum)
            doWrite("Vert count:" & Chr(9) & MyModel.vertnum)

            ReDim MyModel.polyl(MyModel.polynum)
            For i = 0 To MyModel.polynum - 1

                If old <> Int(100 * i / (MyModel.polynum)) Then
                    'Console.Clear()
                    doWrite("Poly count:" & Chr(9) & MyModel.polynum)
                    doWrite("Vert count:" & Chr(9) & MyModel.vertnum)
                    doWrite("-------------------------------------")
                    'Try
                    'DoWrite("Reading Percentage: " & Int(100 * i / (MyModel.polynum + MyModel.vertnum)) & "%")

                    ' Catch
                    doWrite("Reading Percentage: [POLYS] " & Int(100 * i / (MyModel.polynum)) & "%")
                End If

                old = Int(100 * i / (MyModel.polynum))
                'End Try
                '

                MyModel.polyl(i).type = J.ReadInt16
                '  doWrite("TYPE:" & Hex(MyModel.polyl(i).type))
                MyModel.polyl(i).Tpage = J.ReadInt16
                If MyModel.polyl(i).Tpage = -1 Then MyModel.polyl(i).Tpage = 26

                MyModel.polyl(i).vi0 = J.ReadInt16
                MyModel.polyl(i).vi1 = J.ReadInt16
                MyModel.polyl(i).vi2 = J.ReadInt16
                MyModel.polyl(i).vi3 = J.ReadInt16



                MyModel.polyl(i).c0 = J.ReadUInt32
                MyModel.polyl(i).c1 = J.ReadUInt32
                MyModel.polyl(i).c2 = J.ReadUInt32
                MyModel.polyl(i).c3 = J.ReadUInt32

                MyModel.polyl(i).u0 = J.ReadSingle
                MyModel.polyl(i).v0 = J.ReadSingle
                MyModel.polyl(i).u1 = J.ReadSingle
                MyModel.polyl(i).v1 = J.ReadSingle
                MyModel.polyl(i).u2 = J.ReadSingle
                MyModel.polyl(i).v2 = J.ReadSingle
                MyModel.polyl(i).u3 = J.ReadSingle
                MyModel.polyl(i).v3 = J.ReadSingle
            Next

            ReDim MyModel.vexl(MyModel.vertnum)

            For a = 0 To MyModel.vertnum - 1
                If old <> Int(a * 100 / (MyModel.vertnum)) Then
                    ' Console.Clear()
                    ' DoWrite("Poly count:" & Chr(9) & MyModel.polynum)
                    ' DoWrite("Vert count:" & Chr(9) & MyModel.vertnum)
                    'DoWrite("-------------------------------------")
                    doWrite("Reading Percentage: [POLYS] 100%")
                    doWrite("Reading Percentage: [VERTICES] " & Int(a * 100 / (MyModel.vertnum)) & "%")
                End If

                old = Int(a * 100 / (MyModel.vertnum))
                Dim x, y, z As Single

                x = J.ReadSingle * Zoom
                y = J.ReadSingle * -1 * Zoom
                z = J.ReadSingle * Zoom


                MyModel.vexl(a).Position = New Vector3D(x, y, z)

                x = J.ReadSingle * Zoom
                y = J.ReadSingle * -1 * Zoom
                z = J.ReadSingle * Zoom
                MyModel.vexl(a).normal = New Vector3D(x, y, z)


            Next

            'let's set Directory and also Filename

            Me.FileName = filepath.Split("\")(UBound(filepath.Split("\")))
            Me.Directory = Replace(filepath, Me.FileName, "", , , CompareMethod.Text)
            Me.DirectoryName = filepath.Split("\")(UBound(filepath.Split("\")) - 1)

            J.Close()
        End Sub
        Sub New()

        End Sub
        Sub Render()

            Dim quads = 0

            Dim vx3d As New Vertex3D()
            Dim polys() = MyModel.polyl 'clone polys (less code will be used)
            Dim vexs() = MyModel.vexl   'clone vertex(s) ( same reason)
            Dim j(27) As Long


            'ok, new vx list...
            '   Dim newVxList() As Vertex3D
            '  Dim maxNewVx As Long


            For k = 0 To 25

                Vx(k) = New MeshBuffer(VertexType.Standard)
                'Dim texa As Bitmap = Bitmap.FromFile(Directory & DirectoryName & Chr(65 + k) & ".bmp")

                Dim tex As Texture


                If IO.File.Exists(Directory & DirectoryName & Chr(65 + k) & ".bmp") = False Then
                    If IO.Directory.GetFiles(Directory, "*" & Chr(65 + k) & ".bmp").Length > 0 Then
                        tex = VideoDriver.GetTexture(IO.Directory.GetFiles(Directory, "*" & Chr(65 + k) & ".bmp")(0))
                    ElseIf IO.Directory.GetFiles(Directory, "*.bmp").Length > 0 Then
                        tex = VideoDriver.GetTexture(IO.Directory.GetFiles(Directory, "*.bmp")(0))
                    End If

                Else
                    tex = VideoDriver.GetTexture(Directory & DirectoryName & Chr(65 + k) & ".bmp")

                End If


                If tex IsNot Nothing Then
                    tex.MakeColorKey(VideoDriver, Color.Black)
                    tex.RegenerateMipMapLevels()

                    Vx(k).Material.Texture1 = tex
                    VideoDriver.SetMaterial(Vx(k).Material)
                Else
                    If mainform.CheckBox1.Checked Then
                        My.Resources.Image1.Save(IO.Path.GetTempPath & "\notex.jpg", Drawing.Imaging.ImageFormat.Jpeg)
                        tex = VideoDriver.GetTexture(IO.Path.GetTempPath & "\notex.jpg")
                        Vx(k).Material.Texture1 = tex
                        VideoDriver.SetMaterial(Vx(k).Material)
                    End If

                    End If


                    j(k) = -1

            Next
            Vx(26) = New MeshBuffer(VertexType.Standard)
            j(26) = -1




            For i = 0 To MyModel.polynum



                'Vx.Material = Directory & DirectoryName & 

                Vx(polys(i).Tpage).SetVertex(System.Threading.Interlocked.Increment(j(polys(i).Tpage)), New Vertex3D(vexs(polys(i).vi0).Position,
                                                        vexs(polys(i).vi0).normal,
                                                        ColorsToRGB(polys(i).c0),
                                                        New Vector2D(polys(i).u0, polys(i).v0)))

                Vx(polys(i).Tpage).SetVertex(System.Threading.Interlocked.Increment(j(polys(i).Tpage)), New Vertex3D(vexs(polys(i).vi1).Position,
                                                             vexs(polys(i).vi1).normal,
                                                             ColorsToRGB(polys(i).c1),
                                                             New Vector2D(polys(i).u1, polys(i).v1)))

                Vx(polys(i).Tpage).SetVertex(System.Threading.Interlocked.Increment(j(polys(i).Tpage)), New Vertex3D(vexs(polys(i).vi2).Position,
                                                       vexs(polys(i).vi2).normal,
                                                       ColorsToRGB(polys(i).c2),
                                                       New Vector2D(polys(i).u2, polys(i).v2)))


                If polys(i).type Mod 2 = 1 Then
                    'it's a quad!!! hey don't panic, I'll split it!

                    Vx(polys(i).Tpage).SetVertex(System.Threading.Interlocked.Increment(j(polys(i).Tpage)), New Vertex3D(vexs(polys(i).vi2).Position,
                                                    vexs(polys(i).vi2).normal,
                                                    ColorsToRGB(polys(i).c2),
                                                    New Vector2D(polys(i).u2, polys(i).v2)))


                    Vx(polys(i).Tpage).SetVertex(System.Threading.Interlocked.Increment(j(polys(i).Tpage)), New Vertex3D(vexs(polys(i).vi0).Position,
                                                     vexs(polys(i).vi0).normal,
                                                     ColorsToRGB(polys(i).c0),
                                                     New Vector2D(polys(i).u0, polys(i).v0)))



                    Vx(polys(i).Tpage).SetVertex(System.Threading.Interlocked.Increment(j(polys(i).Tpage)), New Vertex3D(vexs(polys(i).vi3).Position,
                                       vexs(polys(i).vi3).normal,
                                          ColorsToRGB(polys(i).c3),
                                         New Vector2D(polys(i).u3, polys(i).v3)))

                End If
                Try




                Catch ex As Exception

                End Try

            Next
            Dim n


            For k = 0 To 27
                ' n = -1
                Try
                    If Vx(k) Is Nothing Then Continue For
                    For n = 0 To Vx(k).VertexCount
                        Vx(k).SetIndex(n, n)
                        If n > 65535 Then MsgBox("problem")
                    Next





                    Dim mesh As New Mesh


                    mesh.AddMeshBuffer(Vx(k))
                    'scnNode(k) = ScnMgr.AddOctTreeSceneNode(mesh, ScnMgr.RootSceneNode, -1, 256)
                    '
                    scnNode(k) = ScnMgr.AddMeshSceneNode(mesh, ScnMgr.RootSceneNode, -1)



                    '  scnNode.GetMaterial(0).Texture1 = Material.Texture1



                    scnNode(k).SetMaterialFlag(MaterialFlag.ZBuffer, True)
                    scnNode(k).SetMaterialFlag(MaterialFlag.Lighting, False)
                    scnNode(k).SetMaterialFlag(MaterialFlag.BackFaceCulling, False)
                    scnNode(k).SetMaterialType(MaterialType.TransparentAlphaChannelRef)
                    '   scnNode(k).SetMaterialFlag(MaterialFlag.AnisotropicFilter, True)
                    'scnNode(k).SetMaterialType(MaterialType.TransparentAddColor)
                Catch ex As Exception

                End Try
                ' scnNode(k).SetMaterialFlag(MaterialFlag.TrilinearFilter, True)
            Next k






        End Sub
        Public Sub UnRender()
            guiEnv.Clear()
            For i = 0 To 27
                If scnNode(i) IsNot Nothing Then ScnMgr.AddToDeletionQueue(scnNode(i))
            Next
        End Sub

    End Class

    'Structure

    Public Structure MODEL_POLY_LOAD
        Dim type, Tpage As Int16
        Dim vi0, vi1, vi2, vi3 As Int16
        Dim c0, c1, c2, c3 As UInt32
        Dim u0, v0, u1, v1, u2, v2, u3, v3 As Single
    End Structure

    Public Structure MODEL_VERTEX_MORPH
        Dim Position As Vector3D
        Dim normal As Vector3D
    End Structure
    Public Structure Sphere
        Dim Center As Vector3D
        Dim radius As Single
    End Structure


    Public Class MODEL
        Public polynum, vertnum As Short
        Public polyl() As MODEL_POLY_LOAD
        Public vexl() As MODEL_VERTEX_MORPH


    End Class


End Module
