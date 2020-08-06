Imports System.IO
Imports IrrlichtNETCP

Module level_render

    'Version GVE-nVolt engine: 1.1
    ' 2020/08/06

    Public Timer_List As New List(Of Timers.Timer)

    Public Class WorldFile
        Public Directory As String
        Public FileName As String
        Public DirectoryName As String
        Private meshCount As Long
        Public mMesh(meshCount) As WorldMesh


        Public polyEleven = 0
        Public ENV() As UInt32
        Public AllFrames As New List(Of List(Of Frame))



        Public Frames As New List(Of Frame)

        Public BallC As Int32
        Public Cubes() As FunnyBall
        Public AnimC As Int32

        Public texAnimHandlerList As New List(Of TexAnimHandler)


        Sub New(ByVal filepath As String)
            Dim old&
            Dim J As New BinaryReader(New FileStream(filepath, FileMode.Open))

            meshCount = J.ReadInt32
            ReDim mMesh(meshCount)
            polyEleven = 0

            '  PolyCount = 0
            ' VexCount = 0

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
                    mMesh(k).polyl(i).tpage = J.ReadInt16
                    If mMesh(k).polyl(i).tpage = -1 Then mMesh(k).polyl(i).tpage = 26

                    'GVE fix: Uint16
                    mMesh(k).polyl(i).vi0 = J.ReadUInt16
                    mMesh(k).polyl(i).vi1 = J.ReadUInt16
                    mMesh(k).polyl(i).vi2 = J.ReadUInt16
                    mMesh(k).polyl(i).vi3 = J.ReadUInt16



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



            If J.BaseStream.Position + 3 > J.BaseStream.Length Then GoTo endreading

            BallC = J.ReadInt32


            ReDim Cubes(BallC - 1)
            For i = 0 To BallC - 1
                Cubes(i) = New FunnyBall
                Cubes(i).center = New Vector3D(J.ReadSingle, J.ReadSingle, J.ReadSingle)
                Cubes(i).Radius = J.ReadSingle
                Cubes(i).meshCount = J.ReadInt32
                ReDim Cubes(i).Mesh(Cubes(i).meshCount)
                For k = 0 To Cubes(i).meshCount - 1
                    Cubes(i).Mesh(k) = J.ReadInt32
                Next
            Next

            If J.BaseStream.Position + 3 > J.BaseStream.Length Then GoTo endreading

            AnimC = J.ReadInt32


            For a = 0 To AnimC - 1

                AllFrames.Add(New List(Of Frame))
                For b = 0 To J.ReadInt32 - 1
                    AllFrames.Item(a).Add(New Frame)
                Next



                For c = 0 To AllFrames.Item(a).Count - 1
                    AllFrames(a)(c).Tex = J.ReadInt32
                    AllFrames(a)(c).Delay = J.ReadSingle * 1000
                    AllFrames(a)(c).UV(0) = New Vector2D(J.ReadSingle, J.ReadSingle)
                    AllFrames(a)(c).UV(1) = New Vector2D(J.ReadSingle, J.ReadSingle)
                    AllFrames(a)(c).UV(2) = New Vector2D(J.ReadSingle, J.ReadSingle)
                    AllFrames(a)(c).UV(3) = New Vector2D(J.ReadSingle, J.ReadSingle)
                Next


            Next

            ReDim ENV(polyEleven - 1)
            Try
                For a = 0 To polyEleven - 1
                    ENV(a) = J.ReadUInt32
                Next

            Catch ex As Exception

            End Try

endreading:




            'let's set Directory and also Filename

            Me.FileName = filepath.Split("\")(UBound(filepath.Split("\")))
            Me.Directory = Replace(filepath, Me.FileName, "", , , CompareMethod.Text)
            Me.DirectoryName = filepath.Split("\")(UBound(filepath.Split("\")) - 1)

        End Sub

        Dim Current_animation_index = -1


        Function curanimidx(j%)
            Current_animation_index += 1

            Return Current_animation_index

        End Function

        Public Material(26) As Material
        Sub Render()
            'TODO: erm this is kay from 2020... looking at his own N years old code....
            'How bad can this code be? Can someone optimize it?

            'UNDONE: The bucket sorting into env/alpha/transp


            'animation tracker:
            Current_animation_index = 0
            texAnimHandlerList.Clear()

            'Materials




            For k = 0 To 25 '26 bitmaps
                Material(k) = New Material



                Material(k).Texture1 = VideoDriver.GetTexture(Directory & DirectoryName & Chr(65 + k) & ".bmp")
                If Material(k).Texture1 IsNot Nothing Then


                    Dim img = New Bitmap(Directory & DirectoryName & Chr(65 + k) & ".bmp", True)
                    If img.PixelFormat <> Imaging.PixelFormat.Format32bppArgb And img.PixelFormat <> Imaging.PixelFormat.Format32bppRgb And img.PixelFormat <> Imaging.PixelFormat.Format32bppPArgb Then
                        Material(k).Texture1.MakeColorKey(VideoDriver, Color.Black) '(on 24bits) black goes transparrrr 

                    End If
                    img.Dispose()
                    img = Nothing


                    '  Material(k).Texture1.RegenerateMipMapLevels()
                End If

            Next
            Material(26) = New Material


            For p = 0 To meshCount - 1

                Dim add_mesh_to_texanim_bucket As Boolean = False

                Dim j As Long = 0
                mainform.Text = "Loading " & Strings.Format(p / (meshCount - 1) * 100, "00.00") & "%"

                Dim quads = 0
                Dim vx3d As New Vertex3D()
                Dim polys() = mMesh(p).polyl 'clone polys (less code will be used)
                Dim vexs() = mMesh(p).vexl   'clone vertex(s) ( same reason)



                Dim Texbuckets(26) As List(Of Integer)
                Dim TexbucketsVx(26) As List(Of Integer)





                For i = 0 To mMesh(p).polynum
                    If Texbuckets(polys(i).tpage) Is Nothing Then
                        Texbuckets(polys(i).tpage) = New List(Of Integer)
                        TexbucketsVx(polys(i).tpage) = New List(Of Integer)
                    End If

                    Texbuckets(polys(i).tpage).Add(i)
                Next i
                For k = 0 To 26 '26th = last material no gfx
                    add_mesh_to_texanim_bucket = False
                    If Texbuckets(k) Is Nothing Then Continue For
                    If Texbuckets(k).Count < 2 Then Continue For

                    'ok, new vx list...
                    '   Dim newVxList() As Vertex3D
                    '  Dim maxNewVx As Long

                    Dim alphavxk As New List(Of Integer)
                    Dim firstRun As Boolean = True


                    Dim mesh As New Mesh()

                    Dim VxNor As New MeshBuffer(VertexType.Standard)

                    Dim VxAnim As New List(Of MeshBuffer)

neg:
                    For kk = 0 To Texbuckets(k).Count - 1


                        Dim vx = VxNor


                        Dim i = Texbuckets(k)(kk)

                        '  If polys(i).type And 512 Then Continue For

                        If Not (polys(i).type And 4) And firstRun Then GoTo endit ' 4: trans vertex alpha





                        '   Next
                        If k <> 26 Then vx.Material.Texture1 = Material(k).Texture1

                        If (polys(i).type And 512) Then 'Texanim
                            add_mesh_to_texanim_bucket = True

                            VxAnim.Add(New MeshBuffer(VertexType.Standard))
                            vx = VxAnim(VxAnim.Count - 1)

                            texAnimHandlerList.Add(New TexAnimHandler)

                            ' texAnimHandlerList(texAnimHandlerList.Count - 1).animation = AllFrames(curanimidx(k))
                            texAnimHandlerList(texAnimHandlerList.Count - 1).animation = AllFrames(polys(i).tpage)
                            texAnimHandlerList(texAnimHandlerList.Count - 1).animMesh = Nothing
                            texAnimHandlerList(texAnimHandlerList.Count - 1).firstVxIdx = 0
                            texAnimHandlerList(texAnimHandlerList.Count - 1).isQuad = (polys(i).type Mod 2) = 1
                            texAnimHandlerList(texAnimHandlerList.Count - 1).MeshBufferIdx = VxAnim.Count

                            vx.SetVertex(0, New Vertex3D(vexs(polys(i).vi0).Position,
                                                           vexs(polys(i).vi0).normal,
                                                           ColorsToRGB(polys(i).c0),
                                                           New Vector2D(polys(i).u0, polys(i).v0)))

                            vx.SetVertex(1, New Vertex3D(vexs(polys(i).vi1).Position,
                                                                     vexs(polys(i).vi1).normal,
                                                                     ColorsToRGB(polys(i).c1),
                                                                     New Vector2D(polys(i).u1, polys(i).v1)))

                            vx.SetVertex(2, New Vertex3D(vexs(polys(i).vi2).Position,
                                                               vexs(polys(i).vi2).normal,
                                                               ColorsToRGB(polys(i).c2),
                                                               New Vector2D(polys(i).u2, polys(i).v2)))




                            vx.SetIndex(0, 0)
                            vx.SetIndex(1, 1)
                            vx.SetIndex(2, 2)



                            If polys(i).type Mod 2 = 1 Then
                                'it's a quad!!! hey don't panic, I'll split it!

                                vx.SetVertex(3, New Vertex3D(vexs(polys(i).vi0).Position,
                                                             vexs(polys(i).vi0).normal,
                                                             ColorsToRGB(polys(i).c0),
                                                             New Vector2D(polys(i).u0, polys(i).v0)))

                                vx.SetVertex(4, New Vertex3D(vexs(polys(i).vi2).Position,
                                                            vexs(polys(i).vi2).normal,
                                                            ColorsToRGB(polys(i).c2),
                                                            New Vector2D(polys(i).u2, polys(i).v2)))






                                vx.SetVertex(5, New Vertex3D(vexs(polys(i).vi3).Position,
                                               vexs(polys(i).vi3).normal,
                                                  ColorsToRGB(polys(i).c3),
                                                 New Vector2D(polys(i).u3, polys(i).v3)))

                                vx.SetIndex(3, 3)
                                vx.SetIndex(4, 4)
                                vx.SetIndex(5, 5)
                            End If

                            vx.Material.Texture1 = Material(AllFrames(Current_animation_index)(0).Tex).Texture1

                        Else


                            vx = VxNor

                            vx.SetVertex(j, New Vertex3D(vexs(polys(i).vi0).Position,
                                                        vexs(polys(i).vi0).normal,
                                                        ColorsToRGB(polys(i).c0),
                                                        New Vector2D(polys(i).u0, polys(i).v0)))

                            vx.SetVertex(j + 1, New Vertex3D(vexs(polys(i).vi1).Position,
                                                                     vexs(polys(i).vi1).normal,
                                                                     ColorsToRGB(polys(i).c1),
                                                                     New Vector2D(polys(i).u1, polys(i).v1)))

                            vx.SetVertex(j + 2, New Vertex3D(vexs(polys(i).vi2).Position,
                                                               vexs(polys(i).vi2).normal,
                                                               ColorsToRGB(polys(i).c2),
                                                               New Vector2D(polys(i).u2, polys(i).v2)))




                            vx.SetIndex(j, j)
                            vx.SetIndex(j + 1, j + 1)
                            vx.SetIndex(j + 2, j + 2)
                            j += 3



                            If polys(i).type Mod 2 = 1 Then
                                'it's a quad!!! hey don't panic, I'll split it!

                                vx.SetVertex(j, New Vertex3D(vexs(polys(i).vi0).Position,
                                                             vexs(polys(i).vi0).normal,
                                                             ColorsToRGB(polys(i).c0),
                                                             New Vector2D(polys(i).u0, polys(i).v0)))

                                vx.SetVertex(j + 1, New Vertex3D(vexs(polys(i).vi2).Position,
                                                            vexs(polys(i).vi2).normal,
                                                            ColorsToRGB(polys(i).c2),
                                                            New Vector2D(polys(i).u2, polys(i).v2)))






                                vx.SetVertex(j + 2, New Vertex3D(vexs(polys(i).vi3).Position,
                                               vexs(polys(i).vi3).normal,
                                                  ColorsToRGB(polys(i).c3),
                                                 New Vector2D(polys(i).u3, polys(i).v3)))

                                vx.SetIndex(j, j)
                                vx.SetIndex(j + 1, j + 1)
                                vx.SetIndex(j + 2, j + 2)

                                j += 3
                            End If
                        End If



endit:
                        If kk = Texbuckets(k).Count - 1 And firstRun = True Then
                            firstRun = False
                            GoTo neg
                        End If
                    Next

                    VxNor.Material.NormalizeNormals = True
                    mesh.AddMeshBuffer(VxNor)
                    For Each mb In VxAnim
                        mesh.AddMeshBuffer(mb)
                    Next

                    'For each animation add this current mesh
                    If VxAnim.Count > 0 Then
                        For a = texAnimHandlerList.Count - 1 To 0 Step -1
                            If texAnimHandlerList(a).animMesh Is Nothing Then texAnimHandlerList(a).animMesh = mesh
                        Next
                    End If


                    Dim scnNode As New SceneNode

                    '

                    '  setMaterialType(video: EMT_ONETEXTURE_BLEND);
                    'getMaterial(0).MaterialTypeParam = IRR() :
                    'video : pack_textureBlendFunc(IRR: video : EBF_SRC_ALPHA, IRR: video : EBF_ONE_MINUS_SRC_ALPHA, IRR: video : EMFN_MODULATE_1X, IRR: video : EAS_TEXTURE | irr:video : EAS_VERTEX_COLOR);
                    '                       getMaterial(0).setFlag(EMF_BLEND_OPERATION, True);


                    If firstRun Then scnNode = ScnMgr.AddMeshSceneNode(mesh, ScnMgr.RootSceneNode, -1) Else scnNode = ScnMgr.AddMeshSceneNode(mesh, tScnNode, -1)



                    scnNode.SetMaterialFlag(MaterialFlag.BackFaceCulling, False)
                    scnNode.AutomaticCulling = CullingType.Off
                    ' If k <> 26 Then scnNode.SetMaterialTexture(0, Material(k).Texture1)
                    If k <> 26 Then scnNode.GetMaterial(0).DiffuseColor.Set(255, 255, 255, 255)
                    If k <> 26 Then scnNode.SetMaterialType(MaterialType.TransparentAlphaChannel) 'TransparentVertexAlpha




                    If Not firstRun Then 'first run is alpha
                        '  scnNode.SetMaterialTexture(1, Material(polys(i).tpage).Texture1)
                        'scnNode.SetMaterialFlag(MaterialFlag.ZWriteEnable, True)                        'scnNode.SetMaterialType(MaterialType.TransparentVertexAlpha) 'TransparentVertexAlpha

                        '  scnNode.SetMaterialType(MaterialType.TransparentReflection2Layer) 'TransparentVertexAlpha
                    End If


                    If k <> 26 Then scnNode.SetMaterialFlag(MaterialFlag.Lighting, False)

                    '  scnNode.SetMaterialType(MaterialType.TransparentAlphaChannel)
                    '   scnNode(k).SetMaterialFlag(MaterialFlag.AnisotropicFilter, True)
                    'scnNode(k).SetMaterialType(MaterialType.TransparentAddColor)
                    scnNode.SetMaterialFlag(MaterialFlag.BilinearFilter, True)
                    ' scnNode(k).SetMaterialFlag(MaterialFlag.TrilinearFilter, True)




                Next








            Next



            ' ScnMgr.RegisterNodeForRendering(tScnNode, SceneNodeRenderPass.Transparent)
        End Sub

    End Class

    Public Structure MODEL_POLY_LOAD
        Dim type As Int16
        Dim tpage As Int16
        Dim vi0, vi1, vi2, vi3 As uInt16
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
        Public polynum, vertnum As UShort
        Public polyl() As MODEL_POLY_LOAD
        Public vexl() As MODEL_VERTEX_MORPH


    End Class

End Module
