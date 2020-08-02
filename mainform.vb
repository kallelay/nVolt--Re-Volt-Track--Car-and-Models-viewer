Imports IrrlichtNETCP
Imports System.Runtime.InteropServices

Public Class mainform

    Private Function FindWindow( _
      ByVal lpClassName As String, _
      ByVal lpWindowName As String) As IntPtr
    End Function

    'Public WithEvents BS As New BuildString

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub mainform_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing


        Device.Dispose()
        Process.GetCurrentProcess.Kill()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            If mPRM IsNot Nothing Then
                mPRM.UnRender()
            End If

            mPRM = New PRM(OpenFileDialog1.FileName)
            mPRM.Render()
        End If

    End Sub

    Private Sub mainform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Render.BackColor = New Color(255, 230, 230, 230)
        'Button2.BackColor = My.Settings.backgroundColor
        '   CheckBox1.Checked = My.Settings.chess


    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub mainform_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Main.Main()
        Me.ActiveControl = Panel1
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ColorDialog1.Color = fromIrrColorToColor(Render.BackColor)
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Render.BackColor = fromColorToIrrColor(ColorDialog1.Color)
            Button2.BackColor = ColorDialog1.Color
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If mPRM Is Nothing Then Exit Sub
        My.Settings.chess = Me.CheckBox1.Checked
        ScnMgr.AddToDeletionQueue(mPRM.scnNode(0))
        Main.Start()
        Panel1.Focus()
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        Dim x = cam
        If CheckBox2.Checked Then

            cam = ScnMgr.AddCameraSceneNodeFPS(ScnMgr.RootSceneNode, 50, 100, False)

            Panel1.Focus()
        Else
            cam = ScnMgr.AddCameraSceneNodeMaya(ScnMgr.RootSceneNode, 50, 100, 200, -1)

        End If
        cam.Position = x.Position
        cam.Target = x.Target


        cam.AutomaticCulling = CullingType.Off
        VideoDriver.SetTransform(TransformationState.View, cam.AbsoluteTransformation)
        cam.NearValue = 0.01
        cam.FarValue = 32768


    End Sub

    Private Sub CheckBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CheckBox2.KeyDown


    End Sub

    Private Sub CheckBox2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CheckBox2.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then

        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then

        End If
    End Sub
End Class