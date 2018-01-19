Public Class Singletons
    Private Shared newStr As String
    Sub New(ByVal FilePath As String)
        If Not IO.File.Exists(FilePath) Then
            ''tip.fShow("Error:" & FilePath & " couldn't be found")
            Exit Sub
        End If
        newStr = IO.File.ReadAllText(FilePath)
        Clean()
    End Sub
    Sub Clean()
        Dim temp = Split(newStr, vbNewLine)
        Dim CleanStr As String = ""
        For i = 0 To UBound(temp)
            If InStr(temp(i), ";") > 0 Then
                If temp(i).Length > 1 Then
                    If temp(i)(1) = ")" Then
                        CleanStr &= temp(i) & vbNewLine
                        Continue For
                    End If
                End If


                CleanStr &= Split(temp(i), ";")(0) & vbNewLine
            Else

                CleanStr &= temp(i) & vbNewLine
            End If
        Next

        Do Until InStr(CleanStr, vbNewLine & vbNewLine) < 1
            CleanStr = Replace(CleanStr, vbNewLine & vbNewLine, vbNewLine)
        Loop

        newStr = CleanStr
    End Sub
    Public Function getAllSingletons() As String()
        Dim temp() = newStr.Split(vbNewLine)
        Dim header As String = ""
        For i = LBound(temp) To UBound(temp)
            If InStr(temp(i), "{") > 0 Then
                header &= Replace(Replace(Split(temp(i), "{")(0), " " & vbTab, ""), vbTab, "") & ","
            End If
        Next

        Return header.Split(",")
    End Function
    Sub writeHeader(ByRef w As IO.StreamWriter, ByVal Name$)
        w.WriteLine()
        w.WriteLine(";====================")
        w.WriteLine("; {0}", Name)
        w.WriteLine(";====================")
        w.WriteLine()
    End Sub

    Public Sub SaveToFile(ByVal path As String)

        'FileCopy(path, path & "x")


        Dim w As New IO.StreamWriter(New IO.FileStream(Replace(path, "\\", "\", , , CompareMethod.Text), IO.FileMode.Create))

        With _car.Theory.MainInfos
            w.WriteLine("{")
            w.WriteLine()
            w.WriteLine(";============================================================")
            w.WriteLine(";============================================================")
            w.WriteLine("; " & .Name)
            w.WriteLine(";============================================================")
            w.WriteLine(";============================================================")
            Dim x$ = .Name
            Do Until x(0) <> " "
                x = Mid(x, 2)
            Loop
            w.WriteLine("Name      	""{0}""", x)
            w.WriteLine()

            writeHeader(w, "Model Filepaths")

            For i = 0 To 18
                w.WriteLine("MODEL 	{0} 	""{1}""", i, .Model(i))
            Next

            w.WriteLine("TPAGE 	""{0}""", .Tpage)
            w.WriteLine("COLL 	""{0}""", .CollFile)
            w.WriteLine(";)TCARBOX 	""{0}""", .TCarBox)
            w.WriteLine("EnvRGB 	{0} {1} {2}", .EnvRGB.X, .EnvRGB.Y, .EnvRGB.Z)



            writeHeader(w, "Stuff mainly for frontend display and car selectability")



            w.WriteLine("BestTime   	{0}", UCase(.BESTTIME.ToString))
            w.WriteLine("Selectable   	{0}", UCase(.SELECTABLE.ToString))



            w.WriteLine("Class      	{0} 			; Engine type (0=Elec, 1=Glow, 2=Other)", .car_class)
            w.WriteLine("Obtain     	{0} 			; Obtain method", .obtain)
            w.WriteLine("Rating     	{0} 			; Skill level (rookie, amateur, ...)", .Rating)
            w.WriteLine("TopEnd     	{0} 			; Actual top speed (mph) for frontend bars", .TopEnd)
            w.WriteLine("Acc        	{0} 			; Acceleration rating (empirical)", .Acceleration)
            w.WriteLine("Weight     	{0} 			; Scaled weight (for frontend bars)", .Weight)
            w.WriteLine("Handling   	{0} 			; Handling ability (empirical and totally subjective)", .Handling)
            w.WriteLine("Trans      	{0} 			; Transmission type (calculate in game anyway...)", .Trans)
            w.WriteLine("MaxRevs    	{0} 			; Max Revs (for rev counter)", .MaxRev)

        End With

        With _car.Theory.RealInfos
            writeHeader(w, "Handling related stuff")
            w.WriteLine("SteerRate  	{0} 			; Rate at which steer angle approaches value from input", .SteerRate)
            w.WriteLine("SteerMod   	{0} 			; ", .SteerMode)
            w.WriteLine("EngineRate 	{0} 			; Rate at which Engine voltage approaches set value", .EngineRate)
            w.WriteLine("TopSpeed   	{0} 			; Car's theoretical top speed (not including friction...)", .TopSpeed)
            w.WriteLine("DownForceMod	{0} 			; Down force modifier when car on floor", .DownForceModifier)
            w.WriteLine("CoM        	{0} {1} {2} 		; Centre of mass relative to model centre", .COM.X * 1 / Render.Zoom, .COM.Y * 1 / Render.Zoom, .COM.Z * 1 / Render.Zoom)
            w.WriteLine("Weapon     	{0} {1} {2} 		; Weapon genration offset", .WeaponGeneration.X * 1 / Render.Zoom, .WeaponGeneration.Y * -1 / Render.Zoom, .WeaponGeneration.Z * 1 / Render.Zoom)

        End With

        With _car.Theory.Body
            writeHeader(w, "Car Body details")
            w.WriteLine("BODY {		; Start Body")
            w.WriteLine("ModelNum   	{0} 			; Model Number in above list", .modelNumber)
            w.WriteLine("Offset     	{0}, {1}, {2} 		; Calculated in game", .Offset.X * 1 / Render.Zoom, .Offset.Y * -1 / Render.Zoom, .Offset.Z * 1 / Render.Zoom)
            w.WriteLine("Mass       	{0}", .Mass)
            w.WriteLine("Inertia    	{0} {1} {2}", .Inertia(0).X * 1 / Render.Zoom, .Inertia(0).Y * 1 / Render.Zoom, .Inertia(0).Z * 1 / Render.Zoom)
            w.WriteLine("           	{0} {1} {2}", .Inertia(1).X * 1 / Render.Zoom, .Inertia(1).Y * 1 / Render.Zoom, .Inertia(1).Z * 1 / Render.Zoom)
            w.WriteLine("           	{0} {1} {2}", .Inertia(2).X * 1 / Render.Zoom, .Inertia(2).Y * 1 / Render.Zoom, .Inertia(2).Z * 1 / Render.Zoom)
            w.WriteLine("Gravity		2200 			; No longer used")
            w.WriteLine("Hardness   	{0}", .Hardness)
            w.WriteLine("Resistance 	{0} 			; Linear air esistance", .Resistance)
            w.WriteLine("AngRes     	{0} 			; Angular air resistance", .AngleRes)
            w.WriteLine("ResMod     	{0} 			; Ang air resistnce scale when in air", .ResMode)
            w.WriteLine(" Grip       	{0} 			; Converts downforce to friction value", .Grip)
            w.WriteLine("StaticFriction	{0}", .StaticFriction)
            w.WriteLine("KineticFriction {0}", .KinematicFriction)
            w.WriteLine("}     		; End Body")

        End With

        writeHeader(w, "Car Wheel details")

        For i = 0 To 3

            With _car.Theory.wheel(i)
                w.WriteLine("WHEEL " & i & " { 	; Start Wheel")
                w.WriteLine("ModelNum 	    " & .modelNumber)
                w.WriteLine("Offset1  	    {0} {1} {2}", .Offset(1).X * 1 / Render.Zoom, .Offset(1).Y * -1 / Render.Zoom, .Offset(1).Z * 1 / Render.Zoom)
                w.WriteLine("Offset2  	    {0} {1} {2}", .Offset(2).X * 1 / Render.Zoom, .Offset(2).Y * -1 / Render.Zoom, .Offset(2).Z * 1 / Render.Zoom)
                w.WriteLine("IsPresent   	{0}", UCase(.IsPresent.ToString))
                w.WriteLine("IsPowered   	{0}", UCase(.IsPowered.ToString))
                w.WriteLine("IsTurnable   	{0}", UCase(.IsTurnable.ToString))
                w.WriteLine("SteerRatio  	{0}", .SteerRatio)
                w.WriteLine("EngineRatio 	{0}", .EngineRatio)
                w.WriteLine("Radius      	{0}", .Radius)
                w.WriteLine("Mass        	{0}", .Mass)
                w.WriteLine("Gravity     	2200")
                w.WriteLine("MaxPos      	{0}", .MaxPos)
                w.WriteLine("SkidWidth   	{0}", .SkidWidth)
                w.WriteLine("ToeIn       	{0}", .ToeInn)
                w.WriteLine("AxleFriction    	{0}", .AxleFriction)
                w.WriteLine("Grip            	{0}", .Grip)
                w.WriteLine("StaticFriction  	{0}", .StaticFriction)
                w.WriteLine("KineticFriction 	{0}", .KinematicFriction)
                w.WriteLine("}          	; End Wheel")
                w.WriteLine()
            End With

        Next

        writeHeader(w, "Car Spring details")

        For i = 0 To 3

            With _car.Theory.Spring(i)
                w.WriteLine("SPRING " & i & " { 	; Start Spring")
                w.WriteLine("ModelNum    	{0}", .modelNumber)
                w.WriteLine("Offset  	    {0} {1} {2}", .Offset.X * 1 / Render.Zoom, .Offset.Y * -1 / Render.Zoom, .Offset.Z * 1 / Render.Zoom)
                w.WriteLine("Length      	{0}", .Length)
                w.WriteLine("Stiffness   	{0}", .Stiffness)
                w.WriteLine("Damping     	{0}", .Damping)
                w.WriteLine("Restitution 	{0}", .Restitution)

                w.WriteLine("}          	; End Spring")
                w.WriteLine()
            End With

        Next

        writeHeader(w, "Car Pin details")


        For i = 0 To 3

            With _car.Theory.PIN(i)
                w.WriteLine("PIN " & i & " { 	    ; Start Pin")
                w.WriteLine("ModelNum    	{0}", .modelNumber)
                w.WriteLine("Offset  	    {0} {1} {2}", .offSet.X * 1 / Render.Zoom, .offSet.Y * -1 / Render.Zoom, .offSet.Z * 1 / Render.Zoom)
                w.WriteLine("Length      	{0}", .Length)
                w.WriteLine("}          	; End Pin")
                w.WriteLine()
            End With

        Next


        writeHeader(w, "Car axle details")


        For i = 0 To 3

            With _car.Theory.Axle(i)
                w.WriteLine("AXLE " & i & " { 	; Start Axle")
                w.WriteLine("ModelNum    	{0}", .modelNumber)
                w.WriteLine("Offset  	{0} {1} {2}", .offSet.X * 1 / Render.Zoom, .offSet.Y * -1 / Render.Zoom, .offSet.Z * 1 / Render.Zoom)
                w.WriteLine("Length      	{0}", .Length)
                w.WriteLine("}          	; End Axle")
                w.WriteLine()
            End With

        Next

        writeHeader(w, "Car Spinner details")



        With _car.Theory.Spinner
            w.WriteLine("SPINNER { 	    ; Start spinner")
            w.WriteLine("ModelNum    	{0}", .modelNumber)
            w.WriteLine("Offset      	{0} {1} {2}", .offSet.X * 1 / Render.Zoom, .offSet.Y * -1 / Render.Zoom, .offSet.Z * 1 / Render.Zoom)
            w.WriteLine("Axis        	{0} {1} {2}", .Axis.X * 1 / Render.Zoom, .Axis.Y * -1 / Render.Zoom, .Axis.Z * 1 / Render.Zoom)
            w.WriteLine("AngVel      	{0}", .angVel)
            w.WriteLine("}          	; End Spinner")
            w.WriteLine()
        End With

        writeHeader(w, "Car Aerial details")



        With _car.Theory.Aerial
            w.WriteLine("AERIAL { 	; Start Aerial")
            w.WriteLine("SecModelNum 	{0}", .ModelNumber)
            w.WriteLine("TopModelNum 	{0}", .TopModelNumber)
            w.WriteLine("Offset      	{0} {1} {2}", .offset.X * 1 / Render.Zoom, .offset.Y * -1 / Render.Zoom, .offset.Z * 1 / Render.Zoom)
            w.WriteLine("Direction   	{0} {1} {2}", .Direction.X * 1 / Render.Zoom, .Direction.Y * -1 / Render.Zoom, .Direction.Z * 1 / Render.Zoom)
            w.WriteLine("Length      	{0}", .length)
            w.WriteLine("Stiffness   	{0}", .stiffness)
            w.WriteLine("Damping     	{0}", .damping)

            w.WriteLine("}          	; End Aerial")
            w.WriteLine()
        End With
        Try


            If _car.Theory.carAi IsNot Nothing Then

                With _car.Theory.carAi

                    writeHeader(w, "Car AI details")
                    w.WriteLine("AI { 	; Start AI")
                    w.WriteLine("UnderThresh 	{0}", .UnderThresh)
                    w.WriteLine("UnderRange  	{0}", .UnderRange)
                    w.WriteLine("UnderFront	 	{0}", .UnderFront)
                    w.WriteLine("UnderRear   	{0}", .UnderRear)
                    w.WriteLine("UnderMax    	{0}", .UnderMax)
                    w.WriteLine("OverThresh  	{0}", .OverThresh)
                    w.WriteLine("OverRange   	{0}", .OverRange)
                    w.WriteLine("OverMax     	{0}", .OverMax)
                    w.WriteLine("OverAccThresh  	{0}", .OverAccThresh)
                    w.WriteLine("OverAccRange   	{0}", .OverAccRange)
                    w.WriteLine("PickupBias     	{0}", .PickupBias)
                    w.WriteLine("BlockBias      	{0}", .BlockBias)
                    w.WriteLine("OvertakeBias   	{0}", .OvertakeBias)
                    w.WriteLine("Suspension     	{0}", .Suspension)
                    w.WriteLine("Aggression     	{0}", .Aggression)
                    w.WriteLine("}          	; End AI")
                    w.WriteLine()
                End With

            End If
        Catch

        End Try
        w.WriteLine("}")
        w.Flush()
        w.Close()

        'patching file
        If InStr(Str(CSng(0.2)), ",", CompareMethod.Text) > 0 Then
            Dim mystr = IO.File.ReadAllText(path)
            mystr = Replace(mystr, ",", ".")
            IO.File.WriteAllText(path, mystr)
        End If



    End Sub

    Public Function getSingleton(ByVal header) As SingletonItem
        If InStr(newStr, header) < 1 Then
            Return SingletonItem.Null
        End If


        Dim temp As String = ""
        If header = "" Or header = " " Then
            temp = Split(newStr, "{")(1)

        Else
            temp = Split(Split(newStr, header)(1), "{")(1)
        End If

        If InStr(Split(temp, "}")(0), "{") < 1 Then
            'lucky us!
            Dim l = Split(temp, "}")(0)
            Dim torep = Split(l, vbNewLine)(UBound(Split(l, vbNewLine)))

            Return SingletonItem.ToSingletonItem(Replace(l, torep, ""))


        Else
            'how unlucky...
            Dim tmp As String = temp
            Do Until InStr(tmp, "{") = 0
                Dim splt = Split(Split(Split(tmp, "{")(0), vbNewLine)(1), "}")(0)
                tmp = Replace(tmp, splt, "")
            Loop
            Return SingletonItem.ToSingletonItem(tmp)
        End If
        Return SingletonItem.Null
    End Function

End Class
Public Class SingletonItem
    Private Shared items() As String
    Public Shared Null = Nothing

    Public Shared Function ToSingletonItem(ByVal str As String) As SingletonItem
        Dim nSing As New SingletonItem
        Dim splitted() = Split(str, vbNewLine)
        ReDim items(splitted.Length)
        SingletonItem.items = splitted
        SingletonItem.items = splitted
        Return nSing
    End Function

    Public Function getValue(ByVal key)



        For i = LBound(items) To UBound(items)

            If InStr(items(i), key) > 0 Then
                Dim tmp = Replace(Split(items(i), key)(1), " " & vbTab, "") ', ".", ",")


                ' If IO.File.Exists(RvPath & "\" & Replace(tmp, """", "")) Then Return Replace(tmp, """", "")

                If InStr(CSng(2.15), ",") <> 0 Then
                    Dim cnt As Integer, sp As Integer
                    For j = 0 To tmp.Length - 1
                        If CChar(tmp(j)) >= "0" And CChar(tmp(j)) <= "9" Then cnt += 1
                        If CChar(tmp(j)) = " " Or CChar(tmp(j)) = vbTab Then sp += 1
                    Next

                    If cnt / (tmp.Length - sp) * 100 > 40 Then
                        tmp = Replace(tmp, ".", ",")
                    End If

                    Return tmp

                End If

                Return tmp

            End If
        Next
        Return Nothing
    End Function
    Public Function get3LinesValue(ByVal key)

        For i = LBound(items) To UBound(items)

            If InStr(items(i), key) > 0 Then
                Dim tmp = Replace(Split(items(i), key)(1), " " & vbTab, "") & vbNewLine  ', ".", ",")

                tmp &= Replace(items(i + 1), " " & vbTab, "") & vbNewLine  ', ".", ",")
                tmp &= Replace(items(i + 2), " " & vbTab, "") ', ".", ",")

                If InStr(CSng(2.15), ",") <> 0 Then
                    tmp = Replace(tmp, ".", ",")
                End If

                Return tmp

            End If
        Next
        Return Nothing
    End Function
    Public Function get2LinesValue(ByVal key)

        For i = LBound(items) To UBound(items)

            If InStr(items(i), key) > 0 Then
                Dim tmp = Replace(Split(items(i), key)(1), " " & vbTab, "") & vbNewLine  ', ".", ",") & 

                tmp &= Replace(items(i + 1), " " & vbTab, "") ', ".", ",")

                If InStr(CSng(2.15), ",") <> 0 Then
                    tmp = Replace(tmp, ".", ",")
                End If

                Return tmp

            End If
        Next
        Return Nothing
    End Function
    Public Function getAllKeys()
        Dim allVal(items.Length) As String
        For i = LBound(items) To UBound(items)
            If InStr(items(i), " " & vbTab) > 0 Then
                allVal(i) = Split(items(i), " " & vbTab)(0)
            Else
                allVal(i) = Split(items(i), vbTab)(0)
            End If



        Next
        Return allVal
    End Function
    Public Sub setValue(ByVal key As String, ByVal value As String)
        For i = LBound(items) To UBound(items)

            If InStr(items(i), key) > 0 Then

                items(i) = Replace(items(i), Split(items(i), key)(1), value)

                Exit Sub
            End If
        Next
        Dim nItems(items.Length + 1)

        For i = LBound(items) To UBound(items)
            nItems(i) = items(i)
        Next

        ReDim items(items.Length + 1)

        For i = LBound(items) To UBound(items) - 1
            items(i) = nItems(i)
        Next
        items(UBound(items)) = key & " " & vbTab & value

    End Sub
    Public Function GetEverything() As String()
        Return items
    End Function


End Class