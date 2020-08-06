Imports IrrlichtNETCP
Imports System.IO

Public Class Car_Model

    Public Directory As String
    Public FileName As String
    Public DirectoryName As String
    Public MyModel As New MODEL
    Public PolysReadingProgress, VertexReadingProgress As Double
    Public ScnNode As New SceneNode()
    Public Texture_ As String = ""
    Public tex As Texture
    Public VxCount As Single
    Public isMirror As Boolean = False
    Public mesh As Mesh
    Public BoundingBox As New BBOX

    Public Untextured As MeshBuffer
    Sub New(ByVal filepath As String)

        BoundingBox = New BBOX

        filepath = Replace(filepath, ",", ".")

        If IO.File.Exists(filepath) = False Then
            Console.Beep(500, 100)

            Exit Sub
        End If

        Dim old&



        Dim J As New BinaryReader(New FileStream(Replace(filepath, ",", "."), FileMode.Open))
        If J Is Nothing Then Exit Sub


        'Vert/Poly count
        MyModel.polynum = J.ReadInt16()
        MyModel.vertnum = J.ReadInt16()



        ReDim MyModel.polyl(MyModel.polynum)
        For i = 0 To MyModel.polynum - 1

            If old <> Int(100 * i / (MyModel.polynum)) Then
                PolysReadingProgress = Int(100 * i / (MyModel.polynum))
            End If

            old = Int(100 * i / (MyModel.polynum))


            '

            MyModel.polyl(i).type = J.ReadInt16
            MyModel.polyl(i).Tpage = J.ReadInt16




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

        BoundingBox.minX = 32768
        BoundingBox.maxX = -32768
        BoundingBox.minY = 32768
        BoundingBox.maxY = -32768
        BoundingBox.maxZ = -32768
        BoundingBox.minZ = 32768
        ReDim MyModel.vexl(MyModel.vertnum)

        For a = 0 To MyModel.vertnum - 1
            If old <> Int(a * 100 / (MyModel.vertnum)) Then
                VertexReadingProgress = Int(a * 100 / (MyModel.vertnum))
            End If

            old = Int(a * 100 / (MyModel.vertnum))
            Dim x, y, z As Single

            x = J.ReadSingle * Zoom
            y = J.ReadSingle * -1 * Zoom
            z = J.ReadSingle * Zoom


            If x < BoundingBox.minX Then
                BoundingBox.minX = x
            End If

            If x > BoundingBox.maxX Then
                BoundingBox.maxX = x
            End If

            If y < BoundingBox.minY Then
                BoundingBox.minY = y
            End If

            If y > BoundingBox.maxY Then
                BoundingBox.maxY = y
            End If

            If z < BoundingBox.minZ Then
                BoundingBox.minZ = z
            End If

            If z > BoundingBox.maxZ Then
                BoundingBox.maxZ = z
            End If


            MyModel.vexl(a).Position = New Vector3D(x, y, z)

            x = J.ReadSingle * Zoom '* -1
            y = J.ReadSingle * Zoom * -1
            z = J.ReadSingle * Zoom '* -1
            MyModel.vexl(a).normal = New Vector3D(x, y, z)


        Next

        J.Close()
        'let's set Directory and also Filename

        Me.FileName = filepath.Split("\")(UBound(filepath.Split("\")))
        Me.Directory = Replace(filepath, Me.FileName, "", , , CompareMethod.Text)
        Me.DirectoryName = filepath.Split("\")(UBound(filepath.Split("\")) - 1)

        Models.Add(Me)


    End Sub


    Sub Export(ByVal filepath As String)

        filepath = Replace(filepath, ",", ".")





        If IO.File.Exists(Replace(filepath, ",", ".")) Then
            FileCopy(Replace(filepath, ",", "."), Replace(filepath, ",", ".") & ".bak" & Int(Rnd() * 500))
            Kill(Replace(filepath, ",", "."))
        End If
        Dim J As New BinaryWriter(New FileStream(Replace(filepath, ",", "."), FileMode.CreateNew))
        If J Is Nothing Then Exit Sub


        'Vert/Poly count
        J.Write(Convert.ToInt16(MyModel.polynum))
        J.Write(Convert.ToInt16(MyModel.vertnum))

        Dim i As Integer

        For i = 0 To MyModel.polynum - 1




            '
            J.Write(Convert.ToInt16(MyModel.polyl(i).type))
            J.Write(Convert.ToInt16(MyModel.polyl(i).Tpage))

            J.Write(Convert.ToInt16(MyModel.polyl(i).vi0))
            J.Write(Convert.ToInt16(MyModel.polyl(i).vi1))
            J.Write(Convert.ToInt16(MyModel.polyl(i).vi2))
            J.Write(Convert.ToInt16(MyModel.polyl(i).vi3))

            J.Write(Convert.ToUInt32(MyModel.polyl(i).c0))
            J.Write(Convert.ToUInt32(MyModel.polyl(i).c1))
            J.Write(Convert.ToUInt32(MyModel.polyl(i).c2))
            J.Write(Convert.ToUInt32(MyModel.polyl(i).c3))

            J.Write(CSng(MyModel.polyl(i).u0))
            J.Write(CSng(MyModel.polyl(i).v0))
            J.Write(CSng(MyModel.polyl(i).u1))
            J.Write(CSng(MyModel.polyl(i).v1))
            J.Write(CSng(MyModel.polyl(i).u2))
            J.Write(CSng(MyModel.polyl(i).v2))
            J.Write(CSng(MyModel.polyl(i).u3))
            J.Write(CSng(MyModel.polyl(i).v3))


        Next


        Dim a As Integer
        For a = 0 To MyModel.vertnum - 1


            J.Write(CSng(MyModel.vexl(a).Position.X) / Zoom)
            J.Write(CSng(-MyModel.vexl(a).Position.Y) / Zoom)
            J.Write(CSng(MyModel.vexl(a).Position.Z) / Zoom)

            J.Write(CSng(MyModel.vexl(a).normal.X))
            J.Write(CSng(MyModel.vexl(a).normal.Y))
            J.Write(CSng(MyModel.vexl(a).normal.Z))



        Next

        J.Close()

    End Sub
    Function getVertex_0(ByVal i As Integer) As Vertex3D
        Return New Vertex3D(MyModel.vexl(MyModel.polyl(i).vi0).Position, _
                                                    MyModel.vexl(MyModel.polyl(i).vi0).normal, _
                                                    ColorsToRGB(MyModel.polyl(i).c0), _
                                                    New Vector2D(MyModel.polyl(i).u0, MyModel.polyl(i).v0))
    End Function
    Function getVertex_1(ByVal i As Integer) As Vertex3D
        Return New Vertex3D(MyModel.vexl(MyModel.polyl(i).vi1).Position, _
                                                    MyModel.vexl(MyModel.polyl(i).vi1).normal, _
                                                    ColorsToRGB(MyModel.polyl(i).c1), _
                                                    New Vector2D(MyModel.polyl(i).u1, MyModel.polyl(i).v1))
    End Function
    Function getVertex_2(ByVal i As Integer) As Vertex3D
        Return New Vertex3D(MyModel.vexl(MyModel.polyl(i).vi2).Position, _
                                                    MyModel.vexl(MyModel.polyl(i).vi2).normal, _
                                                    ColorsToRGB(MyModel.polyl(i).c2), _
                                                    New Vector2D(MyModel.polyl(i).u2, MyModel.polyl(i).v2))
    End Function
    Function getVertex_3(ByVal i As Integer) As Vertex3D
        Return New Vertex3D(MyModel.vexl(MyModel.polyl(i).vi3).Position, _
                                                    MyModel.vexl(MyModel.polyl(i).vi3).normal, _
                                                    ColorsToRGB(MyModel.polyl(i).c3), _
                                                    New Vector2D(MyModel.polyl(i).u3, MyModel.polyl(i).v3))
    End Function
    Public Vx As MeshBuffer
    Public Dblsd As MeshBuffer
    Public rvVex() As List(Of Integer)
    Public clVex() As List(Of Integer)
    Public Where() As List(Of Integer)

    Sub Render()



        If Me.MyModel.polynum = 0 Then Exit Sub


        Dim quads = 0



        Vx = New MeshBuffer(VertexType.Standard)
        '  System.GC.AddMemoryPressure(System.GC.GetGeneration(Vx))
        Untextured = New MeshBuffer(VertexType.Standard)
        Dblsd = New MeshBuffer(VertexType.Standard)









        Dim vx3d As New Vertex3D()
        Dim polys() = MyModel.polyl 'clone polys (less code will be used)
        Dim vexs() = MyModel.vexl   'clone vertex(s) ( same reason)
        Dim j, k As Int32
        mesh = New Mesh







        tex = nVOLT.Render.VideoDriver.GetTexture(Replace(Texture_, ",", "."))

        tex.MakeColorKey(nVOLT.Render.VideoDriver, Color.Black)
        tex.RegenerateMipMapLevels()


        Vx.Material.Texture1 = tex 'VideoDriver.GetTexture(Directory & DirectoryName & Chr(65 + k) & ".bmp")





            Dblsd.Material.Texture1 = tex


            ' Car_Load.Render.VideoDriver.SetMaterial(Vx.Material)


        '  Dim texA = Car_Load.Render.VideoDriver.GetTexture(RvPath & "\gfx\fxpage1.bmp")



        '  Untextured.Material.Texture1 = texA 'VideoDriver.GetTexture(Directory & DirectoryName & Chr(65 + k) & ".bmp")





        j = -1
        k = -1


        Dim untexturedCount = 0
        For i = 0 To MyModel.polynum


            '  rvVex(i) = New List(Of Integer)




            If polys(i).Tpage <> -1 Then
                ''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''!!!!TEXTURED!!!!!'''''''''''''''''''''''''

                If polys(i).type And 2 Then 'doublesided
                    'TODO:   rvVex(i).Add(i)
                    Dblsd.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_0(i))
                    Dblsd.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_2(i))
                    Dblsd.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_1(i))


                    If polys(i).type And 1 Then 'quad
                        Dblsd.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_2(i))
                        Dblsd.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_0(i))
                        Dblsd.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_3(i))
                    End If

                Else                        'not double sided
                    Vx.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_0(i))
                    Vx.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_2(i))
                    Vx.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_1(i))

                    If polys(i).type And 1 Then 'quad only
                        Vx.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_2(i))
                        Vx.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_0(i))
                        Vx.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_3(i))
                    End If
                End If

                ''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''!!!!UNTEXTURED!!!!!'''''''''''''''''''''''''
            Else
                If polys(i).type And 2 Then 'doublesided
                    Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_0(i))
                    Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_2(i))
                    Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_1(i))

                    Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_0(i))
                    Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_1(i))
                    Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_2(i))

                    If polys(i).type And 1 Then 'quad
                        Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_2(i))
                        Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_0(i))
                        Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_3(i))
                    End If

                Else                        'not double sided
                    Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_0(i))
                    Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_2(i))
                    Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_1(i))

                    If polys(i).type And 1 Then 'quad only
                        Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_2(i))
                        Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_0(i))
                        Untextured.SetVertex(System.Threading.Interlocked.Increment(j), getVertex_3(i))
                    End If
                End If

            End If



        Next
        Dim n


        ' n = -1

        For n = 0 To Vx.VertexCount - 1
            Vx.SetIndex(n, Vx.VertexCount - 1 - n)
        Next

        For n = 0 To Untextured.VertexCount - 1
            Untextured.SetIndex(n, Untextured.VertexCount - 1 - n)
        Next

        For n = 0 To Dblsd.VertexCount - 1
            Dblsd.SetIndex(n, Dblsd.VertexCount - 1 - n)
        Next





        mesh.AddMeshBuffer(Vx)
        mesh.AddMeshBuffer(Untextured)




        Dblsd.Material.Lighting = False
        Dblsd.Material.BackfaceCulling = False
        mesh.AddMeshBuffer(Dblsd)

        ' scnNode(k) = ScnMgr.AddOctTreeSceneNode(mesh, ScnMgr.RootSceneNode, -1, 256)

        ScnNode = ScnMgr.AddMeshSceneNode(mesh, Nothing, -1)



        '  scnNode.GetMaterial(0).Texture1 = Material.Texture1


        ScnNode.SetMaterialTexture(0, tex)
        '  ScnNode.SetMaterialTexture(1, EnvMap)


        ScnNode.SetMaterialFlag(MaterialFlag.ZBuffer, True)

        ScnNode.SetMaterialFlag(MaterialFlag.Lighting, False)
        '  ScnNode.SetMaterialFlag(MaterialFlag.GouraudShading, True)


        ScnNode.SetMaterialType(MaterialType.TransparentAlphaChannelRef)
        ScnNode.SetMaterialFlag(MaterialFlag.AnisotropicFilter, True)
        ScnNode.SetMaterialFlag(MaterialFlag.TrilinearFilter, True)

        With ScnNode.GetMaterial(0)
            .Lighting = False 'Editor.CheckBox1.Checked
            .GouraudShading = True
            .NormalizeNormals = True

            '  .ZBuffer = 

        End With
        '   ScnNode.SetMaterialTexture(1, Vx.Material.Texture1)

        'ScnNode.SetMaterialType(MaterialType.SphereMap)

        '  ScnNode.GetMaterial(1).Texture1 = Car_Load.Render.VideoDriver.GetTexture(Replace(Texture_, ",", "."))




        '   scnNode(k).SetMaterialFlag(MaterialFlag.AnisotropicFilter, True)
        'scnNode(k).SetMaterialType(MaterialType.TransparentAddColor)

        ' scnNode(k).SetMaterialFlag(MaterialFlag.TrilinearFilter, True)




        Models.Add(Me)



    End Sub

    Function mirror() As Car_Model
        Dim newM As New Car_Model(Me.Directory & "\" & Me.FileName)
        For i = 0 To newM.MyModel.vertnum
            newM.MyModel.vexl(i).Position.Y *= -1
        Next i

        '  For Each MODE As Car_Model In Models
        'If MODE.isMirror = True Then ScnMgr.AddToDeletionQueue(MODE.ScnNode)
        '  Next
        newM.Texture_ = Me.Texture_
        newM.Render()
        newM.ScnNode.SetMaterialType(MaterialType.Reflection2Layer)
        newM.ScnNode.Position = Me.ScnNode.Position * New Vector3D(1, -1, 1) - New Vector3D(0, 2, 0)


        For j = 0 To newM.MyModel.polynum

        Next
        newM.isMirror = True



        Models.Add(newM)
        Return newM
    End Function




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
    Public Structure BBOX
        Dim minX, maxX As Single
        Dim minY, maxY As Single
        Dim minZ, maxZ As Single
    End Structure




    Public Shared Function RGBToUint(ByVal color As Color)
        Return CUInt(color.A) << 24 Or CUInt(color.R) << 16 Or CUInt(color.G) << 8 Or CUInt(color.B) << 0
    End Function

    Public Function ColorsToRGB(ByVal cl As UInt32) As Color
        'long rgb value, is composed from 0~255 R, G, B
        'according to net: (2^8)^cn
        ' cn: R = 0 , G = 1, B = 2


        'simple...
        Dim a = cl >> 24

        If a = 0 Then a = 251


        ' 
        ' If a = 0 Then a = 255
        Dim r = cl >> 16 And &HFF

        Dim g = cl >> 8 And &HFF
        Dim b = cl >> 0 And &HFF


        Return New Color(a, r, g, b)


    End Function
    Public Class MODEL
        Public polynum, vertnum As Short
        Public polyl() As MODEL_POLY_LOAD
        Public vexl() As MODEL_VERTEX_MORPH
    End Class
End Class
Module Public_Models
    Public Models As New List(Of Car_Model)
    Public Models2 As New List(Of Car_Model)
End Module