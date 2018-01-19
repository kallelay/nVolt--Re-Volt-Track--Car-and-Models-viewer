Imports IrrlichtNETCP
Imports System.Math
Module Main

    'will be left blank for awhile...
    Public mPRM As PRM
    Public WorldFile As WorldFile
    Sub Main()
        'Init graphics
        Render.Init()
        '  ' Dim prm = New PRM("C:\Games\pc\revolt\revolt\levels\garden\elephant.prm")
        ' prm.Render()
        '
        '  Dim WorldFile = New WorldFile(Command)
        ' WorldFile.Render()


        Start()

        mainform.Show()
        'RenderOneprm(New PRM("C:\Games\pc\revolt\revolt\levels\garden\elephant.prm"))

        'OK, graphics now?
        Render.Go()

    End Sub

    Sub Start()
        Dim cmd = Replace(Command, """", "")
        If Right(cmd, 1) = "\" Then cmd = Mid(cmd, 1, Len(cmd) - 1)
        If IO.Directory.Exists(cmd) Then
            If IO.File.Exists(cmd & "\parameters.txt") Then
                cmd &= "\parameters.txt"
            End If
            If IO.File.Exists(cmd & "\" & Split(cmd, "\")(UBound(Split(cmd, "\"))) & ".inf") Then
                cmd &= "\" & Split(cmd, "\")(UBound(Split(cmd, "\"))) & ".inf"
            End If
        End If

        If InStr(cmd, ".prm", CompareMethod.Text) > 0 Then
            mPRM = New PRM(cmd)
            mPRM.Render()

        ElseIf InStr(cmd, ".w", CompareMethod.Text) > 0 Then

          
            WorldFile = New WorldFile(cmd)
            WorldFile.Render()


        ElseIf InStr(cmd, ".inf", CompareMethod.Text) > 0 Then



            Dim tmp = Replace(cmd, ".inf", ".w", , , CompareMethod.Text)
            If IO.File.Exists(tmp) Then

                getInfo(cmd)
                WorldFile = New WorldFile(tmp)
                WorldFile.Render()
                '   Load_FIN()
                '   RenderInstances()

            End If

        ElseIf InStr(cmd, "parameters.txt", CompareMethod.Text) > 0 Then
            RvPath = Replace(cmd, "parameters.txt", "") & "\..\.."
            doCar(cmd)
      
            cam.Position = New Vector3D(10, 8, 8)




        End If




    End Sub
    Sub doCar(ByVal cmd)

        _car = New Car(cmd)
        _car.Load()

        Dim ftex = (Replace(RvPath & "\" & _car.Theory.MainInfos.Tpage, ",", "."))



        If (_car.Theory.Body.modelNumber) <> -1 Then
            If IO.File.Exists(Replace(Replace(RvPath & "\" & _car.Theory.MainInfos.Model(_car.Theory.Body.modelNumber), Chr(34), ""), ",", ".")) = True Then
                cBODY = New Car_Model(Replace(RvPath & "\" & _car.Theory.MainInfos.Model(_car.Theory.Body.modelNumber), Chr(34), ""))
                cBODY.Texture_ = ftex
                cBODY.Render()
                Try
                    cBODY.ScnNode.Position = _car.Theory.Body.Offset ' - _car.Theory.RealInfos.COM / 2
                Catch
                End Try



            End If
        Else

        End If

        Application.DoEvents()


        For i = 0 To 3
            If _car.Theory.wheel(i).modelNumber <> -1 Then
                If IO.File.Exists(Replace(RvPath & "\" & Replace(_car.Theory.MainInfos.Model(_car.Theory.wheel(i).modelNumber), Chr(34), ""), ",", ".")) = True Then
                    _Wheel(i) = New Car_Model(RvPath & "\" & Replace(_car.Theory.MainInfos.Model(_car.Theory.wheel(i).modelNumber), Chr(34), ""))
                    If _Wheel(i) IsNot Nothing Then
                        _Wheel(i).Texture_ = ftex
                        _Wheel(i).Render()
                        '  _Wheel(i).ScnNode.DebugObject = True
                        '  _Wheel(i).ScnNode.DebugDataVisible = DebugSceneType.BoundingBox
                        _Wheel(i).ScnNode.Position = _car.Theory.wheel(i).Offset(1) '+ _car.Theory.RealInfos  '+ _car.Theory.wheel(i).Offset(2) '- _car.Theory.Body.Offset





                    End If
                Else

                End If

            End If
        Next

        Application.DoEvents()

        For i = 0 To 3
            'TODO : springs
            If _car.Theory.Spring(i).modelNumber <> -1 Then

                _Spring(i) = New Car_Model(RvPath & "\" & _car.Theory.MainInfos.Model(_car.Theory.Spring(i).modelNumber).Replace(Chr(34), ""))
                _Spring(i).Texture_ = ftex
                _Spring(i).Render()

                '_Spring(i).ScnNode.Scale = _car.Theory.Spring(i).Length '(, 1)
                _Spring(i).ScnNode.Position = _car.Theory.Spring(i).Offset



                Dim A As New Vector3D
                A = _Wheel(i).ScnNode.Position - _Spring(i).ScnNode.Position

                Dim RotAngXY, RotAngYZ, RotAngXZ As Single

                RotAngXZ = Acos(-A.X / Sqrt(A.X ^ 2 + A.Z ^ 2))

                RotAngYZ = 0 'Acos(-A.Z / Sqrt(A.Y ^ 2 + A.Z ^ 2))

                RotAngXY = Acos(A.Y / Sqrt(A.X ^ 2 + A.Y ^ 2))



                Dim FixedScale = (A.X + _car.Theory.wheel(i).Offset(2).X) / (_Spring(i).ScnNode.BoundingBox.MaxEdge).Length





                _Spring(i).ScnNode.Scale = New Vector3D(1, _car.Theory.Spring(i).Length / 10 * _Spring(i).ScnNode.BoundingBox.MaxEdge.Length / getBBoxVectors(_Spring(i).BoundingBox).Length, 1)

                '    message.Message(A.X / getBBoxVectors(_spring(i).BoundingBox).Z & "," & _car.Theory.spring(i).Length)

                _Spring(i).ScnNode.Rotation = New Vector3D(RotAngYZ * (A.Z / Abs(A.Z)) * 180 / PI, RotAngXZ * (A.Z / Abs(A.Z)) * 180 / PI, 180 + (-1) ^ (i) * RotAngXY * 180 / PI) 'RotAngXY * (A.Y / Abs(A.Y)) * 180 / PI)***90





            End If


        Next

        Application.DoEvents()

        'TODO: PINs

        For i = 0 To 3
            If _car.Theory.PIN(i).modelNumber <> -1 Then
                'If _Pin(i) IsNot Nothing Then
                _Pin(i) = New Car_Model(RvPath & "\" & _car.Theory.MainInfos.Model(_car.Theory.PIN(i).modelNumber).Replace(Chr(34), ""))
                _Pin(i).Texture_ = ftex
                _Pin(i).Render()
                _Pin(i).ScnNode.Position = _car.Theory.PIN(i).offSet

                _Pin(i).ScnNode.Scale *= _car.Theory.PIN(i).Length



                'End If
            End If
        Next

        Application.DoEvents()


        For i = 0 To 3
            If _car.Theory.Axle(i).modelNumber <> -1 Then


                _axle(i) = New Car_Model(RvPath & "\" & _car.Theory.MainInfos.Model(_car.Theory.Axle(i).modelNumber).Replace(Chr(34), ""))
                _axle(i).Texture_ = ftex

                _axle(i).Render()

                _axle(i).ScnNode.Position = _car.Theory.Axle(i).offSet



                Dim A As New Vector3D
                A = _Wheel(i).ScnNode.Position - _axle(i).ScnNode.Position

                Dim RotAngXZ, RotAngXY As Single

                RotAngXZ = Acos(-A.X / Sqrt(A.X ^ 2 + A.Z ^ 2))

                RotAngXY = Acos(A.Y / Sqrt(A.X ^ 2 + A.Y ^ 2))

                Dim FixedScale = (A.X + _car.Theory.wheel(i).Offset(2).X) / (_axle(i).ScnNode.BoundingBox.MaxEdge).Length





                '   _axle(i).ScnNode.Scale = New Vector3D(1, 1, Abs(A.X / getBBoxVectors(_axle(i).BoundingBox).Z))

                '_axle(i).ScnNode.Scale = New Vector3D(1, 1, Abs(A.X / getBBoxVectors(_axle(i).BoundingBox).Z))

                _axle(i).ScnNode.Scale = New Vector3D(1, 1, _car.Theory.Axle(i).Length / 10 * _axle(i).ScnNode.BoundingBox.MaxEdge.Length / getBBoxVectors(_axle(i).BoundingBox).Length)


                '    message.Message(A.X / getBBoxVectors(_axle(i).BoundingBox).Z & "," & _car.Theory.Axle(i).Length)

                Dim Zrepo As Single = (A.Z / Abs(A.Z))
                If Single.IsNaN(Zrepo) Then Zrepo = If(i Mod 2 = 0, -1, 1)




                _axle(i).ScnNode.Rotation = New Vector3D(0, _
                                                        (RotAngXZ * Zrepo * 180 / PI - 90), _
                                                     (180 * (i + 1) + 90 + (-1) ^ (i) * RotAngXY * 180 / PI))
                'RotAngXY * (A.Y / Abs(A.Y)) * 180 / PI)***90

                '  message.Message("FirstCalc:" & _axle(i).ScnNode.BoundingBox.MaxEdge.Length / getBBoxVectors(_axle(i).BoundingBox).Z & "\nCL:" & _car.Theory.Axle(i).Length / 10 * _axle(i).ScnNode.BoundingBox.MaxEdge.Length / getBBoxVectors(_axle(i).BoundingBox).Length & "\nPara:" & _car.Theory.Axle(i).Length)



            End If
        Next

        Application.DoEvents()


        If _car.Theory.Spinner.modelNumber <> -1 Then
            _Spinner = New Car_Model(RvPath & "\" & _car.Theory.MainInfos.Model(_car.Theory.Spinner.modelNumber).Replace(Chr(34), ""))

            _Spinner.Texture_ = ftex
            _Spinner.Render()
            '  MsgBox(_Spinner.PolysReadingProgress)
            _Spinner.ScnNode.Position = _car.Theory.Spinner.offSet

            ' _car.Theory.Spinner.Axis()
            '_Spinner.ScnNode.Scale = _car.Theory.Spinner.Axis 

        End If

        Application.DoEvents()


        If _car.Theory.Aerial.ModelNumber <> -1 Then
            If IO.File.Exists(RvPath & "\" & _car.Theory.MainInfos.Model(_car.Theory.Aerial.ModelNumber).Replace(Chr(34), "")) = False Then
                '    Tip.fShow("~~Error: MODEL(" & _car.Theory.Aerial.ModelNumber & ") doesn't exist" & vbNewLine)
            End If
            _Aerial = New Car_Model(RvPath & "\" & _car.Theory.MainInfos.Model(_car.Theory.Aerial.ModelNumber).Replace(Chr(34), ""))
            If _Aerial IsNot Nothing Then

                _Aerial.Texture_ = RvPath & "\gfx\fxpage1.bmp"
                _Aerial.Render()



                _Aerial.ScnNode.Position = _car.Theory.Aerial.offset '+ New Vector3D(0,  _Aerial.BoundingBox.maxY * _car.Theory.Aerial.length, 0)
                _Aerial.ScnNode.Scale = New Vector3D(1, _car.Theory.Aerial.length * 2, 1)
                _Aerial.ScnNode.Rotation = _car.Theory.Aerial.Direction
                '_Aerial.ScnNode.Scale.SetLength(_car.Theory.Aerial.length)


            End If
        End If

        Application.DoEvents()


        If _car.Theory.Aerial.TopModelNumber <> -1 Then
            Dim aerialtop As New Car_Model(RvPath & "\" & _car.Theory.MainInfos.Model(_car.Theory.Aerial.TopModelNumber).Replace(Chr(34), ""))
            If aerialtop IsNot Nothing Then
                aerialtop.Texture_ = RvPath & "\gfx\fxpage1.bmp"
                aerialtop.Render()
                '   aerialtop.ScnNode.Scale *= 5
                ' aerialtop.ScnNode.Position *= _car.Theory.Aerial.Direction.Y
                aerialtop.ScnNode.Position = _car.Theory.Aerial.offset + New Vector3D(0, _Aerial.BoundingBox.maxY * _car.Theory.Aerial.length * 2, 0) + New Vector3D(0, 0.1, 0)
                aerialtop.ScnNode.Scale = New Vector3D(1, -5, 1)
                '  aerialtop.ScnNode.Position += _car.Theory.Aerial.Direction
                '  aerialtop.ScnNode.Position += _car.Theory.Aerial.offset '+  ' ) '+' Aerial.ScnNode.BoundingBox.MaxEdge



            End If
        End If


    End Sub
    Sub getInfo(ByVal cmd)
        Dim temp = IO.File.ReadAllText(cmd)
        Dim ori = Split(Split(temp, "STARTROT")(1), vbNewLine)(0)

        Dim clr$
        Try
            clr = Replace(Replace(Split(Split(temp, "FOGCOLOR")(1), vbNewLine)(0), vbTab, " "), "  ", " ")

        Catch ex As Exception
            clr = "0 0 0"

        End Try
        temp = Replace(Split(Split(temp, "STARTPOS")(1), vbNewLine)(0), vbTab, " ")

        Do Until InStr(clr, "  ") = 0
            clr = Replace(clr, "  ", " ")
        Loop

        Do Until InStr(temp, "  ") = 0
            temp = Replace(temp, "  ", " ")
        Loop

        If clr(0) = " " Then clr = Mid(clr, 2)
        If temp(0) = " " Then temp = Mid(temp, 2)
        Cleanstr(temp)
        Cleanstr(ori)
        Cleanstr(clr)


        ori = Replace(ori, " ", "")

        temp = Replace(temp, vbTab, "")

        If CStr(CSng(18.5))(2) = "," Then
            temp = Replace(temp, ".", ",")
            ori = Replace(ori, ".", ",")
        End If

        Dim vec As New IrrlichtNETCP.Vector3D(temp.Split(" ")(0) * Zoom, -temp.Split(" ")(1) * Zoom, temp.Split(" ")(2) * Zoom)
        Render.BackColor = New IrrlichtNETCP.Color(255, clr.Split(" ")(0), clr.Split(" ")(1), clr.Split(" ")(2))

        '  MsgBox(tmp)
        cam.Target = New Vector3D(vec.X + 10 * Sin(ori * PI * 2), vec.Y, vec.Z + 10 * Cos(ori * PI * 2))
        'cam.Target = 
        '    cam.Target = New Vector3D(vec.X + 0, vec.Y, ori * 360) ' - vec.Z)
        ' Debugger.Break()
        cam.Position = vec

    End Sub
    Sub Cleanstr(ByRef str$)
        Try
            str = Split(str, ";")(0)
        Catch ex As Exception
        End Try
    End Sub
#Region "Debug"
    'For debug...
    Sub doWrite(ByVal str As String)
        '     Debug.Print(str)
    End Sub
#End Region
End Module
