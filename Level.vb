Imports IrrlichtNETCP

''''''''''''''''''''''''''''''''''''{{NewVolt Main Document }}'
'                    Level loader Engine                      '
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

'                               Kallel A.Y - June/July 2010
' modified for nVolt 

Module level
#Region "Structures/Headers"
    Enum TrackType
        RACE_TRACK = 0
        BATTLE_TAG_TRACK = 1
        BATTLE_TRACK = 2
        ' SOCCER_TRACK = 3
        'QUEST_STAR_TRACK = 4
        'FROG_VOLT_TRACK = 5
        STUNT_ARENA_TRACK = 6
        FRONTEND_TRACK = 7
    End Enum
    Enum TrackClass
        OUT_OF_RATE = 0
        EASY = 1
        LITTLE_HARDER = 2
        SEMI_PRO = 3
        HARD = 4
        VERY_HARD = 5
        IMPOSSIBLE_PLATINIUM = 6

    End Enum
    Public Class Level
        Public type As TrackType = TrackType.RACE_TRACK         'type of level
        Public Name As String = ""                              'name of level
        Public FolderName As String = ""                        'folder
        Public creator As String = ""                           'creator
        Public IsLoaded As Boolean = False                      'is loaded?
        Public IsPreLoaded As Boolean = False                   'is loaded type and name?
        Public LevelClass As TrackClass = TrackClass.OUT_OF_RATE           'track class: easy, hard
        Public Length As Double = 0                             'track length
        '  Public MD5CheckSum As String = Space(32)                'MD5 checksum of the track
        Public ChallengeTimeNormal As Long = 0                  'Challenge Time
        Public ChallengeTimeReversed As Long = 0                'challenge time reversemode
        Public music As String = ""                             'music string name
        Public NormalStartPos As Vector3D = New Vector3D(0, 0, 0) 'start position
        Public NormalStartRot As Double = 0                     'start rotation
        Public FarClip As Double = Double.PositiveInfinity      ' far clip distance
        Public FogColor As Color = New Color(0, 0, 0, 0)        ' fog color
        Public FogStart As Double = Double.PositiveInfinity     'fogstart

      
    End Class


#End Region

    Public levels(2 ^ 16) As Level

#Region "Loaders/Unloaders"
    Sub LoadOneLevel(ByVal levelID)
        Dim levelfolder$ = levels(levelID).FolderName   'level folder [get]
        levels(levelID) = ParseInf("tracks\" & levelfolder & "\" & levelfolder & ".inf") 'parse .inf

        '        MyCam.FarClipDistance = levels(levelID).FarClip 'far clip
        '        MyCam.NearClipDistance = 0.01                   'near clip




    End Sub

    Sub PreLoadAllLevels()

    End Sub

#End Region



#Region "Parsers, helpers, aids"
    Function ParseInf(ByVal info$) As Level 'procedure actually...

        Dim lvl As New Level
        'open file
        Dim infofile$ = IO.File.ReadAllText(info)

        'avoid tabs problems... (tab = chr(9) = vbtab)
        infofile = Replace(infofile, vbTab & vbTab & vbTab & vbTab & vbTab, vbTab)
        infofile = Replace(infofile, vbTab & vbTab & vbTab, vbTab)
        infofile = Replace(infofile, vbTab & vbTab, vbTab)



        'parse
        Dim lines$() = Split(infofile, vbNewLine) 'splitting commands


        For j = 0 To UBound(lines) - 1 'loop...

            If Not (lines(j)(1) = ";" Or Len(lines(j)) < 4) Then  'comment or blank file avoiding
                If InStr(lines(j), ";", CompareMethod.Text) > 0 Then lines(j) = lines(j).Split(";")(0)
                If InStr(lines(j), vbTab, CompareMethod.Text) Then
                    Select Case LCase(Split(lines(j), vbTab)(0))
                        Case "name"
                            lvl.Name = Split(lines(j), vbTab)(1)
                        Case "music"
                            lvl.music = Replace(Replace(Split(lines(j), vbTab)(1), Chr(34), ""), "'", "")
                        Case "farclip"
                            lvl.FarClip = CDbl(Split(lines(j), vbTab)(1))
                        Case "creator"
                            lvl.creator = Split(lines(j), vbTab)(1)
                        Case "levelclass"
                            lvl.LevelClass = Int(Split(lines(j), vbTab)(1))
                        Case "startrot"
                            lvl.NormalStartRot = CDbl(Split(lines(j), vbTab)(1))
                        Case "startpos"
                            lvl.NormalStartPos = New Vector3D(CDbl(Split(Split(lines(j), vbTab)(1), " ")(0)), CDbl(Split(Split(lines(j), vbTab)(1), " ")(1)), CDbl(Split(Split(lines(j), vbTab)(1), " ")(2)))
                        Case "type"
                            lvl.type = CDbl(Split(lines(j), vbTab)(1))
                        Case "fogstart"
                            lvl.FogStart = CDbl(Split(lines(j), vbTab)(1))
                        Case "fogcolor"
                            If Split(lines(j), vbTab)(1).Split("\").Length = 4 Then
                                lvl.FogColor = Color.From(CDbl(Split(Split(lines(j), vbTab)(1), " ")(0)), CDbl(Split(Split(lines(j), vbTab)(1), " ")(1)), CDbl(Split(Split(lines(j), vbTab)(1), " ")(2)), CDbl(Split(Split(lines(j), vbTab)(1), " ")(3)))
                            Else
                                lvl.FogColor = Color.From(0, CDbl(Split(Split(lines(j), vbTab)(1), " ")(0)), CDbl(Split(Split(lines(j), vbTab)(1), " ")(1)), CDbl(Split(Split(lines(j), vbTab)(1), " ")(2)))
                            End If

                            '    Case "startgrid"
                            '      lvl.StartGrid = Split(lines(j), vbTab)(1)

                    End Select


                End If

            End If

        Next

        Return lvl

    End Function
    Function SaveVal(ByVal key As String, ByVal value As String) As String
        Return key & vbTab & vbTab & value

    End Function
    Function SaveVal2(ByVal key As String, ByVal value As String) As String
        Return key & vbTab & value

    End Function
    Function getTrackType(ByVal trackfolder$) As String
        'open file
        Dim infofile$ = IO.File.ReadAllText(trackfolder)

        'avoid tabs problems... (tab = chr(9) = vbtab)
        infofile = Replace(infofile, vbTab & vbTab & vbTab & vbTab & vbTab, vbTab)
        infofile = Replace(infofile, vbTab & vbTab & vbTab, vbTab)
        infofile = Replace(infofile, vbTab & vbTab, vbTab)

        'search "type"
        If InStr(infofile, "type", CompareMethod.Text) > 0 Then

            'get what's after type
            Return Split(Split(infofile, "type", -1, CompareMethod.Text)(1), vbNewLine)(0)

        Else
            Return "ERROR" 'error!!
        End If
    End Function
    Function getTrackName(ByVal trackfolder$) As String
        'open file
        Dim infofile$ = IO.File.ReadAllText(trackfolder)

        'avoid tabs problems... (tab = chr(9) = vbtab)
        infofile = Replace(infofile, vbTab & vbTab & vbTab & vbTab & vbTab, vbTab)
        infofile = Replace(infofile, vbTab & vbTab & vbTab, vbTab)
        infofile = Replace(infofile, vbTab & vbTab, vbTab)

        'search "name"
        If InStr(infofile, "name", CompareMethod.Text) > 0 Then

            'get what's after the name
            Return Split(Split(infofile, "name", -1, CompareMethod.Text)(1), vbNewLine)(0)

        Else
            Return "ERROR" 'error!!
        End If

    End Function
    Function getTrackCreator(ByVal trackfolder$) As String
        'open file
        Dim infofile$ = IO.File.ReadAllText(trackfolder)

        'avoid tabs problems... (tab = chr(9) = vbtab)
        infofile = Replace(infofile, vbTab & vbTab & vbTab & vbTab & vbTab, vbTab)
        infofile = Replace(infofile, vbTab & vbTab & vbTab, vbTab)
        infofile = Replace(infofile, vbTab & vbTab, vbTab)

        'search "creator"
        If InStr(infofile, "creator", CompareMethod.Text) > 0 Then

            'get what's after the creator
            Return Split(Split(infofile, "creator", -1, CompareMethod.Text)(1), vbNewLine)(0)

        Else
            Return "ERROR" 'error!!
        End If

    End Function

  
#End Region

End Module
