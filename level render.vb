﻿Imports System.IO
Imports IrrlichtNETCP

Module level_render

    Public Class WorldFile
        Public Directory As String
        Public FileName As String
        Public DirectoryName As String
        Private meshCount As Long
        Public mMesh(meshCount) As WorldMesh

        Sub New(ByVal filepath As String)
            Dim old&
            Dim J As New BinaryReader(New FileStream(filepath, FileMode.Open))

            meshCount = J.ReadInt32
            ReDim mMesh(meshCount)

            For k = 0 To meshCount - 1
                mMesh(k) = New WorldMesh

                'Bounding Sphere...
                mMesh(k).BoundingSphere.Center.X = J.ReadSingle
                mMesh(k).BoundingSphere.Center.Y = J.ReadSingle
                mMesh(k).BoundingSphere.Center.Z = J.ReadSingle
                mMesh(k).BoundingSphere.radius = J.ReadSingle

                'BBOX as well.....
                mMesh(k).bbox.minX = J.ReadSingle
                mMesh(k).bbox.maxX = J.ReadSingle
                mMesh(k).bbox.minY = J.ReadSingle
                mMesh(k).bbox.maxY = J.ReadSingle
                mMesh(k).bbox.minZ = J.ReadSingle
                mMesh(k).bbox.maxZ = J.ReadSingle


                'Vert/Poly count
                mMesh(k).polynum = J.ReadInt16()
                mMesh(k).vertnum = J.ReadInt16()

                doWrite("Poly count:" & Chr(9) & mMesh(k).polynum)
                doWrite("Vert count:" & Chr(9) & mMesh(k).vertnum)

                ReDim mMesh(k).polyl(mMesh(k).polynum)
                For i = 0 To mMesh(k).polynum - 1

                    If old <> Int(100 * i / (mMesh(k).polynum)) Then
                        'Console.Clear()
                        doWrite("Poly count:" & Chr(9) & mMesh(k).polynum)
                        doWrite("Vert count:" & Chr(9) & mMesh(k).vertnum)
                        doWrite("-------------------------------------")
                        'Try
                        'DoWrite("Reading Percentage: " & Int(100 * i / (mmesh(k).polynum + mmesh(k).vertnum)) & "%")

                        ' Catch
                        doWrite("Reading Percentage: [POLYS] " & Int(100 * i / (mMesh(k).polynum)) & "%")
                    End If

                    old = Int(100 * i / (mMesh(k).polynum))
                    'End Try
                    '

                    mMesh(k).polyl(i).type = J.ReadInt16
                    '  doWrite("TYPE:" & Hex(mmesh(k).polyl(i).type))
                    mMesh(k).polyl(i).Tpage = J.ReadInt16
                    If mMesh(k).polyl(i).Tpage = -1 Then mMesh(k).polyl(i).Tpage = 26

                    mMesh(k).polyl(i).vi0 = J.ReadInt16
                    mMesh(k).polyl(i).vi1 = J.ReadInt16
                    mMesh(k).polyl(i).vi2 = J.ReadInt16
                    mMesh(k).polyl(i).vi3 = J.ReadInt16



                    mMesh(k).polyl(i).c0 = J.ReadUInt32
                    mMesh(k).polyl(i).c1 = J.ReadUInt32
                    mMesh(k).polyl(i).c2 = J.ReadUInt32
                    mMesh(k).polyl(i).c3 = J.ReadUInt32

                    mMesh(k).polyl(i).u0 = J.ReadSingle
                    mMesh(k).polyl(i).v0 = J.ReadSingle
                    mMesh(k).polyl(i).u1 = J.ReadSingle
                    mMesh(k).polyl(i).v1 = J.ReadSingle
                    mMesh(k).polyl(i).u2 = J.ReadSingle
                    mMesh(k).polyl(i).v2 = J.ReadSingle
                    mMesh(k).polyl(i).u3 = J.ReadSingle
                    mMesh(k).polyl(i).v3 = J.ReadSingle
                Next

                ReDim mMesh(k).vexl(mMesh(k).vertnum)

                For a = 0 To mMesh(k).vertnum - 1
                    If old <> Int(a * 100 / (mMesh(k).vertnum)) Then
                        ' Console.Clear()
                        ' DoWrite("Poly count:" & Chr(9) & mmesh(k).polynum)
                        ' DoWrite("Vert count:" & Chr(9) & mmesh(k).vertnum)
                        'DoWrite("-------------------------------------")
                        doWrite("Reading Percentage: [POLYS] 100%")
                        doWrite("Reading Percentage: [VERTICES] " & Int(a * 100 / (mMesh(k).vertnum)) & "%")
                    End If

                    old = Int(a * 100 / (mMesh(k).vertnum))
                    Dim x, y, z As Single

                    x = J.ReadSingle * Zoom
                    y = J.ReadSingle * -1 * Zoom
                    z = J.ReadSingle * Zoom


                    mMesh(k).vexl(a).Position = New Vector3D(x, y, z)

                    x = J.ReadSingle * Zoom
                    y = J.ReadSingle * -1 * Zoom
                    z = J.ReadSingle * Zoom
                    mMesh(k).vexl(a).normal = New Vector3D(x, y, z)


                Next
            Next
            'let's set Directory and also Filename

            Me.FileName = filepath.Split("\")(UBound(filepath.Split("\")))
            Me.Directory = Replace(filepath, Me.FileName, "", , , CompareMethod.Text)
            Me.DirectoryName = filepath.Split("\")(UBound(filepath.Split("\")) - 1)

        End Sub
        Sub Render()
            Dim old = Nothing


            Dim Material(26) As Material

            For k = 0 To 25
                Material(k) = New Material
                Material(k).Texture1 = VideoDriver.GetTexture(Directory & DirectoryName & Chr(65 + k) & ".bmp")
                If Material(k).Texture1 IsNot Nothing Then
                    Material(k).Texture1.MakeColorKey(VideoDriver, Color.Black)
                    Material(k).Texture1.RegenerateMipMapLevels()

                End If

            Next
            Material(26) = New Material


            For p = 0 To meshCount - 1

                Dim Vx As MeshBuffer

                Dim quads = 0
                Dim vx3d As New Vertex3D()
                Dim polys() = mMesh(p).polyl 'clone polys (less code will be used)
                Dim vexs() = mMesh(p).vexl   'clone vertex(s) ( same reason)
                Dim j As Long


                Dim Texbuckets(26) As List(Of Integer)
                Dim TexbucketsVx(26) As List(Of Integer)


                For i = 0 To mMesh(p).polynum
                    If Texbuckets(polys(i).tpage) Is Nothing Then
                        Texbuckets(polys(i).tpage) = New List(Of Integer)
                        TexbucketsVx(polys(i).tpage) = New List(Of Integer)
                    End If

                    Texbuckets(polys(i).tpage).Add(i)
                Next i

                For k = 0 To 26
                    If Texbuckets(k) Is Nothing Then Continue For

                    'ok, new vx list...
                    '   Dim newVxList() As Vertex3D
                    '  Dim maxNewVx As Long





                    For kk = 0 To Texbuckets(k).Count - 1
                        j = 0
                        Vx = New MeshBuffer(VertexType.Standard)

                        Dim i = Texbuckets(k)(kk)


                        '   Next
                        Vx.Material.Texture1 = Material(k).Texture1
                        '  old = polys(i).tpage

                        Vx.SetVertex(polys(i).vi0, New Vertex3D(vexs(polys(i).vi0).Position,
                                                            vexs(polys(i).vi0).normal,
                                                            ColorsToRGB(polys(i).c0),
                                                            New Vector2D(polys(i).u0, polys(i).v0)))

                        Vx.SetVertex(polys(i).vi1, New Vertex3D(vexs(polys(i).vi1).Position,
                                                                 vexs(polys(i).vi1).normal,
                                                                 ColorsToRGB(polys(i).c1),
                                                                 New Vector2D(polys(i).u1, polys(i).v1)))

                        Vx.SetVertex(polys(i).vi2, New Vertex3D(vexs(polys(i).vi2).Position,
                                                           vexs(polys(i).vi2).normal,
                                                           ColorsToRGB(polys(i).c2),
                                                           New Vector2D(polys(i).u2, polys(i).v2)))

                        Vx.SetIndex(j, polys(i).vi0)
                        Vx.SetIndex(j + 1, polys(i).vi1)
                        Vx.SetIndex(j + 2, polys(i).vi2)
                        j += 3

                        If polys(i).type Mod 2 = 1 Then
                            'it's a quad!!! hey don't panic, I'll split it!

                            Vx.SetVertex(polys(i).vi2, New Vertex3D(vexs(polys(i).vi2).Position,
                                                        vexs(polys(i).vi2).normal,
                                                        ColorsToRGB(polys(i).c2),
                                                        New Vector2D(polys(i).u2, polys(i).v2)))



                            Vx.SetVertex(polys(i).vi0, New Vertex3D(vexs(polys(i).vi0).Position,
                                                         vexs(polys(i).vi0).normal,
                                                         ColorsToRGB(polys(i).c0),
                                                         New Vector2D(polys(i).u0, polys(i).v0)))



                            Vx.SetVertex(polys(i).vi3, New Vertex3D(vexs(polys(i).vi3).Position,
                                           vexs(polys(i).vi3).normal,
                                              ColorsToRGB(polys(i).c3),
                                             New Vector2D(polys(i).u3, polys(i).v3)))

                            Vx.SetIndex(j, polys(i).vi2)
                            Vx.SetIndex(j + 1, polys(i).vi0)
                            Vx.SetIndex(j + 2, polys(i).vi3)
                            j += 3
                        End If





                        Dim mesh As New Mesh
                        '  Vx.Material.NormalizeNormals = True

                        mesh.AddMeshBuffer(Vx)

                        Dim scnNode As New SceneNode

                        scnNode = ScnMgr.AddMeshSceneNode(mesh, ScnMgr.RootSceneNode, -1)


                        scnNode.SetMaterialFlag(MaterialFlag.BackFaceCulling, False)
                        scnNode.AutomaticCulling = CullingType.Off


                        '  Dim texta As Texture = VideoDriver.GetTexture("C:\Games\pc\revolt\revolt\gfx\envstill.bmp")

                        scnNode.SetMaterialTexture(0, Material(polys(i).tpage).Texture1)
                        '  scnNode.SetMaterialTexture(1, texta)


                        scnNode.SetMaterialType(MaterialType.TransparentAlphaChannel)
                        'cnNode.GetMaterial(1).Texture1 = texta

                        '  scnNode.GetMaterial(0).Texture1 = Material.Texture1
                        ' D
                        '

                        '       scnNode.SetMaterialTexture(2, texta)


                        scnNode.SetMaterialFlag(MaterialFlag.Lighting, False)

                        '  scnNode.SetMaterialType(MaterialType.TransparentAlphaChannelRef)
                        '   scnNode(k).SetMaterialFlag(MaterialFlag.AnisotropicFilter, True)
                        'scnNode(k).SetMaterialType(MaterialType.TransparentAddColor)
                        scnNode.SetMaterialFlag(MaterialFlag.BilinearFilter, True)
                        ' scnNode(k).SetMaterialFlag(MaterialFlag.TrilinearFilter, True)

                    Next
                Next







            Next



        End Sub

    End Class

    Public Structure MODEL_POLY_LOAD
        Dim type As Int16
        Dim tpage As Int16
        Dim vi0, vi1, vi2, vi3 As Int16
        Dim c0, c1, c2, c3 As Long
        Dim u0, v0, u1, v1, u2, v2, u3, v3 As Single
    End Structure

    Public Structure MODEL_VERTEX_MORPH
        Dim Position As Vector3D
        Dim normal As Vector3D
    End Structure
    Public Class WorldMesh
        Public BoundingSphere As Sphere
        Public bbox As Car_Model.BBOX
        Public polynum, vertnum As Short
        Public polyl() As MODEL_POLY_LOAD
        Public vexl() As MODEL_VERTEX_MORPH


    End Class

End Module
