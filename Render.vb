Imports IrrlichtNETCP
Module Render
    Public Zoom = 1 / 10

    Public BackColor As Color = New Color(159, 35, 138, 255)

    Public Device As IrrlichtDevice
    Public VideoDriver As VideoDriver
    Public ScnMgr As SceneManager
    Public guiEnv As GUIEnvironment
    Public mEvent As IrrlichtNETCP.Event

    Public cam As CameraSceneNode

    Dim Width = 1024, Height = 800
    Dim DvType As DriverType = DriverType.Direct3D9
    Dim bits = 32
    Dim FullScreen As Boolean = False
    Dim Vsync As Boolean = True
    Dim Stencil As Boolean = False
    Dim AntiAlias As Boolean = False

    Sub Init()
        'ok, what about init?
        Device = New IrrlichtDevice(DvType, New Dimension2D(Width, Height), bits, FullScreen, Stencil, Vsync, AntiAlias, mainform.Panel1.Handle)
        VideoDriver = Device.VideoDriver
        ScnMgr = Device.SceneManager
        Device.CursorControl.Visible = True
        Device.Resizeable = True
        AddHandler Device.OnEvent, AddressOf onEvent
        guiEnv = Device.GUIEnvironment
      Device.WindowCaption = "nVolt"
        InitCamera()


    End Sub
    Function onEvent(ByVal Ev As IrrlichtNETCP.Event) As Boolean
        If Ev.KeyPressedDown <> True Then Exit Function
        If Ev.KeyCode = KeyCode.Escape Or Ev.KeyCode = KeyCode.Space Then
            If mainform.CheckBox2.Checked And True Then
                mainform.CheckBox2.Checked = False
            Else
                mainform.CheckBox2.Checked = True
            End If
            '   = False
        End If
        'Device.Dispose()
        ' End
        ' End If

    End Function

    Sub Go()
        'For Start rendering & should be the last thing
        'Try
        'Device.Run()


        Do Until Device.Run = False
            Device.WindowCaption = "nVolt ~ fps:" & VideoDriver.FPS          'fps?
            mainform.Label1.Text = String.Format("Cam: {0}   {1}  {2}. Press ESC to exit FPS mode", cam.Position.X / Zoom, cam.Position.Y / Zoom, cam.Position.Z / Zoom)
            mainform.Text = "nVolt ~ fps:" & VideoDriver.FPS          'fps?
            VideoDriver.BeginScene(True, True, BackColor)    'clear buffer
            'VideoDriver.BeginScene(True, True, Color.Black)    'clear buffer
            ScnMgr.DrawAll()
            guiEnv.DrawAll()

            'draw everything
            VideoDriver.EndScene()
            'Scene end
            '      Loop



            ' Catch
            ' End Try
        Loop

       
    End Sub
    Sub InitCamera()
        cam = ScnMgr.AddCameraSceneNodeMaya(ScnMgr.RootSceneNode, 50, 100, 200, -1)
        '  cam = ScnMgr.AddCameraSceneNodeFPS(ScnMgr.RootSceneNode, 50, 100, False)
        ' cam = ScnMgr.AddCameraSceneNode(ScnMgr.RootSceneNode)
        cam.Position = New Vector3D(-40, 40, 80)
        '  Debugger.Break()
        cam.Target = New Vector3D(0, 0, 0)
        cam.AutomaticCulling = CullingType.Off
        VideoDriver.SetTransform(TransformationState.View, cam.AbsoluteTransformation)
        cam.NearValue = 0.01
        cam.FarValue = 32768


    End Sub


End Module

Public Module Conv
    Function fromIrrColorToColor(ByVal irrColor As IrrlichtNETCP.Color) As System.Drawing.Color
        Return System.Drawing.Color.FromArgb(irrColor.A, irrColor.R, irrColor.G, irrColor.B)
    End Function
    Function fromColorToIrrColor(ByVal c As System.Drawing.Color) As Color
        Return New Color(c.A, c.R, c.G, c.B)
    End Function
End Module
