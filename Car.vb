Imports System.Runtime.InteropServices
Imports IrrlichtNETCP
Imports nVOLT.Main_C

Public Module Global_

    Function readPolys(ByVal file$) As Int16
        Try
            Dim x = New IO.BinaryReader(New IO.FileStream(file, IO.FileMode.Open))
            Dim y = x.ReadInt16()
            x.Close()
            Return y

        Catch ex As Exception
            Return 0
        End Try

    End Function
End Module

Public Class Car_theory
    Public MainInfos As New Main_C
    Public RealInfos As New Real_Inf
    Public Body As New Body
    Public wheel(4) As Wheel
    Public Spring(4) As Spring
    Public PIN(4) As PIN
    Public Axle(4) As Axle
    Public Spinner As Spinner
    Public Aerial As Aerial
    Public carAi As AI

End Class
Public Class Main_C
    Public Name As String
    Public Model(19) As String
    Public Tpage As String
    Public CollFile As String
    Public EnvRGB As Vector3D
    Public TCarBox As String = "NONE"

    Public BESTTIME As Boolean
    Public SELECTABLE As Boolean
    Public car_class As Integer
    Public obtain As Integer
    Public Rating As Integer

    Public TopEnd As Single
    Public Acceleration As Single
    Public Weight As Single
    Public Handling As Single

    Public Trans As Single
    Public MaxRev As Single
End Class



Public Class Real_Inf
    Public SteerRate As Double
    Public SteerMode As Double
    Public EngineRate As Double
    Public TopSpeed As Double
    Public DownForceModifier As Double
    Public COM As Vector3D
    Public WeaponGeneration As Vector3D

    Function Clone() As Real_Inf
        Dim j As New Real_Inf
        With j
            .SteerRate = SteerRate
            .SteerMode = SteerMode
            .EngineRate = EngineRate
            .TopSpeed = TopSpeed
            .DownForceModifier = DownForceModifier
            .COM = COM
            .WeaponGeneration = WeaponGeneration
        End With
        Return j
    End Function
End Class

Public Class Body
    Public modelNumber As Integer
    Public Offset As Vector3D
    Public Mass As Double
    Public Inertia(3) As Vector3D
    '  Public Gravity As Double
    Public Hardness As Double
    Public Resistance As Double
    Public AngleRes As Double
    Public ResMode As Double
    Public Grip As Double
    Public StaticFriction As Double
    Public KinematicFriction As Double

    Function Clone() As Body
        Dim j As New Body
        With j
            .modelNumber = modelNumber
            .Mass = Mass
            .Inertia = Inertia
            .Hardness = Hardness
            .Resistance = Resistance
            .AngleRes = AngleRes
            .ResMode = ResMode
            .Grip = Grip
            .StaticFriction = StaticFriction
            .KinematicFriction = KinematicFriction
        End With

        Return j


    End Function
End Class

Public Class Wheel
    Public modelNumber As Integer
    Public Offset(2) As Vector3D
    Public IsPresent As Boolean
    Public IsPowered As Boolean
    Public IsTurnable As Boolean
    Public SteerRatio As Double
    Public EngineRatio As Double
    Public Radius As Double
    Public Mass As Double
    Public Gravity As Double
    Public MaxPos As Double
    Public SkidWidth As Double
    Public ToeInn As Double
    Public AxleFriction As Double
    Public Grip As Double
    Public StaticFriction As Double
    Public KinematicFriction As Double
End Class

Public Class Spring
    Public modelNumber As Integer
    Public Offset As Vector3D
    Public Length As Double
    Public Stiffness As Double
    Public Damping As Double
    Public Restitution As Double
End Class
Public Class PIN
    Public modelNumber As Integer
    Public offSet As Vector3D
    Public Length As Double
End Class
Public Class Axle
    Public modelNumber As Integer
    Public offSet As Vector3D
    Public Length As Double
End Class
Public Class Spinner
    Public modelNumber As Integer
    Public offSet As Vector3D
    Public Axis As Vector3D
    Public angVel As Double

    Function Clone() As Spinner
        Dim j As New Spinner
        With j
            .modelNumber = modelNumber
            .offSet = offSet
            .Axis = Axis
            .angVel = angVel
        End With

        Return j
    End Function
End Class
Public Class Aerial
    Public ModelNumber As Integer
    Public TopModelNumber As Integer
    Public offset As Vector3D
    Public Direction As Vector3D
    Public length As Double
    Public stiffness As Double
    Public damping As Double
    Function Clone() As Aerial
        Dim j As New Aerial
        With j
            .ModelNumber = ModelNumber
            .TopModelNumber = TopModelNumber
            .offset = offset
            .Direction = Direction
            .length = length
            .stiffness = stiffness
            .damping = damping
        End With
        Return j
    End Function
End Class

Public Class AI
    Public UnderThresh As Double
    Public UnderRange As Double
    Public UnderFront As Double
    Public UnderRear As Double
    Public UnderMax As Double
    Public OverThresh As Double
    Public OverRange As Double
    Public OverMax As Double
    Public OverAccThresh As Double
    Public OverAccRange As Double
    Public PickupBias As Double
    Public BlockBias As Double
    Public OvertakeBias As Double
    Public Suspension As Double
    Public Aggression As Double
    Function Clone() As AI
        Dim j As New AI
        With j
            .UnderFront = UnderFront
            .UnderMax = UnderMax
            .UnderRange = UnderRange
            .UnderRear = UnderRear
            .UnderThresh = UnderThresh

            .OverAccRange = OverAccRange
            .OverAccThresh = OverAccThresh
            .OverMax = OverMax
            .OverRange = OverRange
            .OvertakeBias = OvertakeBias
            .OverThresh = OverThresh

            .PickupBias = PickupBias
            .BlockBias = BlockBias
            .Suspension = Suspension
            .Aggression = Aggression
        End With

        Return j
    End Function

End Class

Public Class Car
    Public DirName As String = ""
    Public Path As String
    Public Theory As Car_theory
    Public isLoading As Boolean = False
    Sub New(ByVal Path_ As String)
        Me.DirName = Split(Path_, "\")(UBound(Split(Path_, "\")))
        Path = Path_
    End Sub
    Public Sing As Singletons
    Sub Load()
        _car.isLoading = True
        'THIS IS DARNED!!!! TOOK ME 6 HOURS TO LOAD THE CAR WHAT THE HACKING WORLD ??!!
        Sing = New Singletons(Path)

        Dim Main = Sing.getSingleton("")
        Me.Theory = New Car_theory
        With Me.Theory.MainInfos
            .Name = Replace(Main.getValue("Name"), Chr(34), "")
            For t = 0 To 18

                .Model(t) = Replace(Main.getValue("MODEL " & vbTab & t), Chr(34), "")

                'incompetent car makers... goes here
                If .Model(t) = Nothing Then
                    .Model(t) = Main.getValue(" " & t)

                    'hotfix: KR car
                    If .Model(t) = Nothing Then .Model(t) = """NONE"""


                    Do Until .Model(t)(0) = Chr(34)
                        .Model(t) = Mid(.Model(t), 2)
                        If .Model(t) = "" Then .Model(t) = """NONE"""
                    Loop
                    .Model(t) = Replace(.Model(t), Chr(34), "")

                End If
            Next t

            .Tpage = Replace(Main.getValue("TPAGE"), Chr(34), "")
            If .Tpage.Length > 4 Then
                Do Until .Tpage(0) = "c" Or .Tpage(0) = "C"
                    .Tpage = Mid(.Tpage, 2)
                Loop
            End If


            .TCarBox = Replace(Main.getValue(";)TCARBOX"), Chr(34), "")
            Try

                If .TCarBox = Nothing Then Exit Try
                Do Until LCase(.TCarBox(0)) = "c"  ' Or .TCarBox(0) = "C"
                    .TCarBox = Mid(.TCarBox, 2)
                    If .TCarBox = Nothing Then Exit Do
                Loop


            Catch

            End Try




            .CollFile = Replace(Main.getValue("COLL"), Chr(34), "")
            .EnvRGB = StrToVector(Replace(Main.getValue("EnvRGB"), Chr(34), "")) * New Vector3D(1 / Render.Zoom, -1 / Render.Zoom, 1 / Render.Zoom)


            .BESTTIME = StrToBool(Main.getValue("BestTime"))
            .SELECTABLE = StrToBool(Main.getValue("Selectable"))
            .car_class = Int(Main.getValue("Class"))
            .obtain = Int(Main.getValue("Obtain"))
            .Rating = Main.getValue("Rating")
            .TopEnd = Main.getValue("TopEnd")

            If InStr(.Name, "Acc") > 0 Then
                .Acceleration = Main.getValue("Acc ")
            Else
                .Acceleration = Main.getValue("Acc")
            End If

            .Weight = Main.getValue("Weight")
            .Handling = Main.getValue("Handling")
            .Trans = Main.getValue("Trans")
            .MaxRev = Main.getValue("MaxRevs")
        End With

        With Me.Theory.RealInfos
            .SteerRate = Main.getValue("SteerRate")
            .SteerMode = Main.getValue("SteerMod")
            .EngineRate = Main.getValue("EngineRate")
            .TopSpeed = Main.getValue("TopSpeed")
            .DownForceModifier = Main.getValue("DownForceMod")

            .COM = StrToVector(Main.getValue("CoM")) * New Vector3D(1, -1, 1)

            .WeaponGeneration = StrToVector(Main.getValue("Weapon"))


        End With

        Dim body = Sing.getSingleton("BODY")
        With Me.Theory.Body
            .modelNumber = body.getValue("ModelNum")
            .Offset = StrToVector(body.getValue("Offset"))
            .Mass = body.getValue("Mass")

            Dim _inertia$ = body.get3LinesValue("Inertia")
            .Inertia(0) = StrToVector(Split(_inertia, vbNewLine)(0))
            .Inertia(1) = StrToVector(Split(_inertia, vbNewLine)(1))
            .Inertia(2) = StrToVector(Split(_inertia, vbNewLine)(2))


            '.Inertia(1) = StrToVector(body.getValue(" " & vbTab))
            ' MsgBox(.Inertia(1).X)


            '   .Gravity = body.getValue("Gravity		2200"))
            .Hardness = body.getValue("Hardness")
            .Resistance = body.getValue("Resistance")
            .AngleRes = body.getValue("AngRes")
            .ResMode = body.getValue("ResMod")
            .Grip = body.getValue("Grip")
            .StaticFriction = body.getValue("StaticFriction")
            .KinematicFriction = body.getValue("KineticFriction")
        End With

        For u = 0 To 3
            Dim Wheel = Sing.getSingleton("WHEEL " & u)
            Me.Theory.wheel(u) = New Wheel
            With Me.Theory.wheel(u)
                .modelNumber = Wheel.getValue("ModelNum")
                .Offset(1) = StrToVector(Wheel.getValue("Offset1"))
                .Offset(2) = StrToVector(Wheel.getValue("Offset2"))
                .IsPresent = StrToBool(Wheel.getValue("IsPresent"))
                .IsPowered = StrToBool(Wheel.getValue("IsPowered"))
                .IsTurnable = StrToBool(Wheel.getValue("IsTurnable"))
                .SteerRatio = Wheel.getValue("SteerRatio")
                .EngineRatio = Wheel.getValue("EngineRatio")
                .Radius = Wheel.getValue("Radius")
                .Mass = Wheel.getValue("Mass")
                .Gravity = Wheel.getValue("Gravity")
                .MaxPos = Wheel.getValue("MaxPos")
                .SkidWidth = Wheel.getValue("SkidWidth")
                .ToeInn = Wheel.getValue("ToeIn")
                .AxleFriction = Wheel.getValue("AxleFriction")
                .Grip = Wheel.getValue("Grip")
                .StaticFriction = Wheel.getValue("StaticFriction")

            End With
        Next

        For u = 0 To 3
            Dim spring = Sing.getSingleton("SPRING " & u)
            Me.Theory.Spring(u) = New Spring
            With Me.Theory.Spring(u)
                .modelNumber = spring.getValue("ModelNum")
                .Offset = StrToVector(spring.getValue("Offset"))
                .Length = spring.getValue("Length")
                .Stiffness = spring.getValue("Stiffness")
                .Damping = spring.getValue("Damping")
                .Restitution = spring.getValue("Restitution")
            End With
        Next


        For u = 0 To 3
            Dim PIN = Sing.getSingleton("PIN " & u)
            Me.Theory.PIN(u) = New PIN
            With Me.Theory.PIN(u)
                .modelNumber = PIN.getValue("ModelNum")
                .offSet = StrToVector(PIN.getValue("Offset"))
                .Length = PIN.getValue("Length")
            End With
        Next

        For u = 0 To 3
            Dim axle = Sing.getSingleton("AXLE " & u)
            Me.Theory.Axle(u) = New Axle
            With Me.Theory.Axle(u)
                .modelNumber = axle.getValue("ModelNum")
                .offSet = StrToVector(axle.getValue("Offset"))
                .Length = axle.getValue("Length")
            End With
        Next

        Dim Spinner = Sing.getSingleton("SPINNER")
        Me.Theory.Spinner = New Spinner
        With Me.Theory.Spinner
            .modelNumber = Spinner.getValue("ModelNum")
            .offSet = StrToVector(Spinner.getValue("Offset"))
            .Axis = StrToVector(Spinner.getValue("Axis"))
            .angVel = Spinner.getValue("AngVel")

        End With

        Dim Aerial = Sing.getSingleton("AERIAL")
        Me.Theory.Aerial = New Aerial
        With Me.Theory.Aerial
            .ModelNumber = Aerial.getValue("SecModelNum")
            .TopModelNumber = Aerial.getValue("TopModelNum")
            .offset = StrToVector(Aerial.getValue("Offset"))
            .Direction = StrToVector(Aerial.getValue("Direction"))
            .length = Aerial.getValue("Length")
            .stiffness = Aerial.getValue("Stiffness")
            .damping = Aerial.getValue("Damping")
        End With

        Dim Ai = Sing.getSingleton("AI")
        If Ai Is Nothing Then Exit Sub
        Me.Theory.carAi = New AI
        With Me.Theory.carAi
            .UnderThresh = Ai.getValue("UnderThresh")
            .UnderRange = Ai.getValue("UnderRange")
            .UnderFront = Ai.getValue("UnderFront	")
            .UnderRear = Ai.getValue("UnderRear")
            .UnderMax = Ai.getValue("UnderMax")
            .OverThresh = Ai.getValue("OverThresh")
            .OverRange = Ai.getValue("OverRange")
            .OverMax = Ai.getValue("OverMax")
            .OverAccThresh = Ai.getValue("OverAccThresh")
            .OverAccRange = Ai.getValue("OverAccRange")
            .PickupBias = Ai.getValue("PickupBias")
            .BlockBias = Ai.getValue("BlockBias")
            .OvertakeBias = Ai.getValue("OvertakeBias")
            .Suspension = Ai.getValue("Suspension")
            .Aggression = Ai.getValue("Aggression")
        End With
        'Debugger.Break()
        'MsgBox(Me.Theory.RealInfos.TopSpeed)
    End Sub

    Function StrToVector(ByVal str As String) As Vector3D
        'On Error Resume Next




        If InStr(str, ",") > 0 And InStr(CSng(0.5), ".") > 0 Then str = str.Replace(",", "")


        'Our vector is X Y Z

        str = Replace(Replace(str, Chr(9), " ", , , CompareMethod.Text), Space(2), Space(1), , , CompareMethod.Text)
        str = str.Replace("�", "")
        Do Until str(0) <> " " Or Len(str) = 0
            str = Mid(str, 2)
        Loop
        Dim j = 1
        Do Until j = str.Length - 1
            If str(j) = " " And str(j - 1) = "," Then
                str = str.Remove(j - 1, 1)
                j -= 1
            End If
            j += 1
        Loop





        'If str.Split(" ")(0).Split(".").Length > 2 Then str = Replace(str, str.Split(" ")(0), str.Split(" ")(0).Split(".")(0) & "." & str.Split(" ")(0).Split(".")(1))
        'If str.Split(" ")(1).Split(".").Length > 2 Then str = Replace(str, str.Split(" ")(1), str.Split(" ")(1).Split(".")(0) & "." & str.Split(" ")(1).Split(".")(1))
        'If str.Split(" ")(2).Split(".").Length > 2 Then str = Replace(str, str.Split(" ")(2), str.Split(" ")(2).Split(".")(0) & "." & str.Split(" ")(2).Split(".")(1))

        Return New Vector3D(CDbl(str.Split(" ")(0)) * Render.Zoom, _
        -CDbl(str.Split(" ")(1)) * Render.Zoom, _
        CDbl(str.Split(" ")(2)) * Render.Zoom)

    End Function

    Function StrToBool(ByVal str As String) As Boolean
        On Error Resume Next
        Return CBool(Replace(Replace(str, " ", ""), vbTab, ""))
    End Function
    Function StrToStr(ByVal str As String)
        Return Replace(Split(str, Chr(34))(1), Chr(34), "")
    End Function

End Class


Module Upperclass
    Public Unix = False

    Public WHEELMODE As Boolean

    Public _t As Double = 0
    'Public _Render = S
    Public _car As New Car("")
    Public RvPath As String
    Public Initialized As Boolean = False
    Public Car_Init = False


    Public cBODY As Car_Model
    Public _Wheel(4) As Car_Model
    Public _Spring(4) As Car_Model
    Public _Pin(4) As Car_Model
    Public _axle(4) As Car_Model
    Public _Spinner As Car_Model
    Public _Aerial As Car_Model

    Public TextureFloorFile As String


    Public Function RGBToLong(ByVal color As Drawing.Color) As UInt32
        Return Convert.ToUInt32(color.A) << 24 Or CUInt(color.R) << 16 Or CUInt(color.G) << 8 Or CUInt(color.B) << 0
    End Function

    Public Function ColorsToRGB(ByVal cl As UInt32) As IrrlichtNETCP.Color
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


        Return New IrrlichtNETCP.Color(a, r, g, b)


    End Function

    Sub CarTheory()

        _car.Theory = New Car_theory



    End Sub





    Public Function AxleFit(ByVal Axle As IrrlichtNETCP.Vector3D, ByVal Wheel As IrrlichtNETCP.Vector3D) As Double
        Dim Alpha As Double
        Alpha = Math.Asin((Wheel.X - Axle.X) / (Math.Sqrt((Wheel.X - Axle.X) ^ 2 + (Wheel.Z - Axle.Z) ^ 2)))
        Return Alpha
    End Function
    Public Function GetLength(ByVal Axle As IrrlichtNETCP.Vector3D, ByVal Wheel As IrrlichtNETCP.Vector3D) As Double
        Return (Math.Sqrt((Wheel.X - Axle.X) ^ 2 + (Wheel.Z - Axle.Z) ^ 2))
    End Function
    Public Function setRotate(ByVal origin As IrrlichtNETCP.Vector3D, ByVal Angle As IrrlichtNETCP.Vector3D) As IrrlichtNETCP.SceneNode
        Dim node As New IrrlichtNETCP.SceneNode
        Dim pos = node.AbsolutePosition - origin
        Dim newrot = node.Rotation + Angle
        Dim distance = pos.Length
        Dim newpos As New IrrlichtNETCP.Vector3D(0, 0, distance)

        Dim m As New IrrlichtNETCP.Matrix4
        m.RotationDegrees = newrot
        m.RotateVect(newpos)
        newpos += origin
        node.Rotation = newrot

        Return node

    End Function
    Function LookAt(ByVal vPos As Vector3D, ByVal vLookAt As Vector3D, ByVal vWorldUp As Vector3D) As Matrix4
        Dim vRight As Vector3D = New Vector3D(0, 0, 0)
        Dim vUp As Vector3D = New Vector3D(0, 0, 0)
        Dim resMat(16) As Single
        Dim vDir = vPos - vLookAt
        Dim Mat As New Matrix4

        vDir.Normalize()
        vRight = vWorldUp.CrossProduct(vDir)

        resMat(0) = vRight.X
        resMat(1) = vRight.Z
        resMat(2) = vRight.Y
        resMat(3) = 0
        resMat(4) = vUp.X
        resMat(5) = vUp.Z
        resMat(6) = vUp.Y
        resMat(7) = 0
        resMat(8) = vDir.X
        resMat(9) = vDir.Z
        resMat(10) = vDir.Y
        resMat(11) = 0
        resMat(12) = 0
        resMat(13) = 0
        resMat(14) = 0
        resMat(15) = 1

        Mat = Matrix4.FromUnmanaged(resMat)
        Return Mat
    End Function
    Function VectorPlusScalarVec(ByVal vleft As Vector3D, ByVal scalar As Single, ByVal vright As Vector3D) As Vector3D
        Return vleft + scalar * vright
    End Function
#Region "Aero"

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure MARGINS
        Public cxLeftWidth As Integer
        Public cxRightWidth As Integer
        Public cyTopHeight As Integer
        Public cyButtomheight As Integer
    End Structure

    <DllImport("dwmapi.dll")> _
    Public Function DwmExtendFrameIntoClientArea(ByVal hWnd As IntPtr, ByRef pMarinset As MARGINS) As Integer
    End Function

    Public Sub Aero(ByVal Form_ As Form, ByVal left As Integer, ByVal right As Integer, ByVal top As Integer, ByVal bottom As Integer)
        On Error Resume Next
        Dim margins As MARGINS = New MARGINS
        margins.cxLeftWidth = left
        margins.cxRightWidth = right
        margins.cyTopHeight = top
        margins.cyButtomheight = bottom
        'set all the four value -1 to apply glass effect to the whole window
        'set your own value to make specific part of the window glassy.
        Dim hwnd As IntPtr = Form_.Handle
        DwmExtendFrameIntoClientArea(hwnd, margins)
    End Sub
    Public Sub Aero(ByVal Form_ As Form)
        Aero(Form_, -1, -1, -1, -1)

    End Sub
    Private Declare Function RemoveMenu Lib "user32" (ByVal hMenu As IntPtr, ByVal nPosition As Integer, ByVal wFlags As Long) As IntPtr
    Private Declare Function GetSystemMenu Lib "user32" (ByVal hWnd As IntPtr, ByVal bRevert As Boolean) As IntPtr
    Private Declare Function GetMenuItemCount Lib "user32" (ByVal hMenu As IntPtr) As Integer
    Private Declare Function DrawMenuBar Lib "user32" (ByVal hwnd As IntPtr) As Boolean
    Private Const MF_BYPOSITION = &H400
    Private Const MF_REMOVE = &H1000
    Private Const MF_DISABLED = &H2

    Public Sub DisableCloseButton(ByVal hwnd As IntPtr)
        Dim hMenu As IntPtr
        Dim menuItemCount As Integer
        hMenu = GetSystemMenu(hwnd, False)
        menuItemCount = GetMenuItemCount(hMenu)
        Call RemoveMenu(hMenu, menuItemCount - 1, _
        MF_DISABLED Or MF_BYPOSITION)
        Call RemoveMenu(hMenu, menuItemCount - 2, _
        MF_DISABLED Or MF_BYPOSITION)
        Call DrawMenuBar(hwnd)
    End Sub
#End Region
End Module