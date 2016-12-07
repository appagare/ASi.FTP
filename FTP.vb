
Imports System.IO
Imports Mabry.Net.Clients.Ftp.Ftp
Public Class FTP


    Private _License As Mabry.Net.Clients.Ftp.Licenser
    Private _FTP As Mabry.Net.Clients.Ftp.Ftp 'used by the discrete connection/disconnection methods
    Private _Timeout As Integer = 20 'used by the discrete connection/disconnection methods
    'Private _PassiveMode As Boolean = False

    Public Enum TransferType
        Ascii = 0
        Binary = 1
    End Enum
    'Public Property PassiveMode() As Boolean
    '    Get
    '        Return _PassiveMode
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        _PassiveMode = Value
    '    End Set
    'End Property
#Region "Non-Discrete Methods"


    '    Public Sub DeleteFile(ByVal pstrServer As String, _
    '       ByVal pstrUsername As String, _
    '       ByVal pstrPassword As String, _
    '       ByVal pstrFileName As String, _
    '       Optional ByVal pintTimeout As Integer = 20, _
    '       Optional ByVal pintPort As Integer = 21)
    '        'Purpose:   Delete a file on the server.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrFilename = string name on the server file to delete.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        'Return:    None.

    '        On Error GoTo DeleteFile_Error

    '        'does not support batch deletes
    '        If InStr(pstrFileName, "*") > 0 Or InStr(pstrFileName, "?") > 0 Then
    '            Throw New Exception("FileName parameter cannot contain wildcard characters.")
    '            Exit Sub
    '        End If

    '        'create an instance of the control
    '        Dim FTP As New Mabry.Net.Clients.Ftp.Ftp
    '        'connect to the server
    '        FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, pintTimeout)

    '        'if the file exists, delete it
    '        If Me._FileExists(FTP, pstrFileName, pintTimeout) Then
    '            FTP.DeleteFile(pstrFileName, pintTimeout)
    '        End If

    '        'clean up
    '        FTP.Disconnect()
    '        FTP.Dispose()
    '        FTP = Nothing
    '        Exit Sub

    'DeleteFile_Error:
    '        Dim Message As String = Err.Description
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Throw New Exception(Message)

    '    End Sub

    '    Public Function FileDate(ByVal pstrServer As String, _
    '           ByVal pstrUsername As String, _
    '           ByVal pstrPassword As String, _
    '           ByVal pstrFileName As String, _
    '           Optional ByVal pintTimeout As Integer = 20, _
    '           Optional ByVal pintPort As Integer = 21) As Date
    '        'Purpose:   Returns the date of a given file.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrFilename = string name on the server file whose date is to be returned.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        'Return:    Date of the file if found. Otherwise, throws an exception.

    '        On Error GoTo FileDate_Error

    '        'error if wildcards 
    '        If InStr(pstrFileName, "*") > 0 Or InStr(pstrFileName, "?") > 0 Then
    '            Throw New Exception("FileName parameter cannot contain wildcard characters")
    '            'ElseIf FileExists(pstrServer, pstrUsername, pstrPassword, pstrFileName, pintTimeout, pintPort) = False Then
    '            '    Throw New Exception("File not found.")
    '        End If

    '        'create an instance of the control
    '        Dim FTP As New Mabry.Net.Clients.Ftp.Ftp
    '        Dim ReturnValue As Date

    '        'connect
    '        FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, pintTimeout)

    '        'user should call FileExists first; since we will not find a date for a non-existent file,
    '        'throw exception now
    '        If Me._FileExists(FTP, pstrFileName, pintTimeout) = False Then
    '            FTP.Disconnect()
    '            FTP.Dispose()
    '            FTP = Nothing
    '            Throw New Exception("File not found")
    '        End If

    '        'find the file and return its date
    '        Dim a As Array = FTP.GetFileList(Trim(pstrFileName), pintTimeout)
    '        Dim e As IEnumerator = a.GetEnumerator
    '        While e.MoveNext
    '            If CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Type = Mabry.Net.Clients.Ftp.Ftp.FileTypes.File AndAlso _
    '                LCase(CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Name) = LCase(Right(pstrFileName, Len(CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Name))) Then
    '                ReturnValue = CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Timestamp
    '                Exit While
    '            End If
    '        End While
    '        FTP.Disconnect()
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Return ReturnValue

    'FileDate_Error:
    '        Dim Message As String = Err.Description
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Throw New Exception(Message)

    '    End Function

    '    Public Function FileExists(ByVal pstrServer As String, _
    '       ByVal pstrUsername As String, _
    '       ByVal pstrPassword As String, _
    '       ByVal pstrFileName As String, _
    '       Optional ByVal pintTimeout As Integer = 20, _
    '       Optional ByVal pintPort As Integer = 21) As Boolean
    '        'Purpose:   Public method to determine whether a file is present on the server.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrFilename = string name on the server file being sought.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        'Return:    Boolean if the file exists on the server; otherwise, False

    '        On Error GoTo FileExists_Error

    '        'error on wildcards
    '        If InStr(pstrFileName, "*") > 0 Or InStr(pstrFileName, "?") > 0 Then
    '            Throw New Exception("FileName parameter cannot contain wildcard characters.")
    '            Exit Function
    '        End If

    '        'create instance of control
    '        Dim FTP As New Mabry.Net.Clients.Ftp.Ftp
    '        Dim ReturnValue As Boolean = False

    '        'connect
    '        FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, pintTimeout)

    '        'find
    '        ReturnValue = Me._FileExists(FTP, pstrFileName, pintTimeout)

    '        'clean up
    '        FTP.Disconnect()
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Return ReturnValue

    '        Exit Function

    'FileExists_Error:
    '        Dim Message As String = Err.Description
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Throw New Exception(Message)


    '    End Function

    '    Public Function FileList(ByVal pstrServer As String, _
    '       ByVal pstrUsername As String, _
    '       ByVal pstrPassword As String, _
    '       Optional ByVal pstrFileSpec As String = "", _
    '       Optional ByVal pintTimeout As Integer = 20, _
    '       Optional ByVal pintPort As Integer = 21) As ArrayList
    '        'Purpose:   Return a list of server filenames.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrFileSpec = optional string name, including wildcard search pattern, for 
    '        '           files being sought.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        'Return:    ArrayList of filenames.


    '        On Error GoTo FileList_Error

    '        'create instance
    '        Dim FTP As New Mabry.Net.Clients.Ftp.Ftp
    '        Dim ReturnValue As New ArrayList

    '        'connect
    '        FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, pintTimeout)

    '        'fetch list
    '        Dim a As Array = FTP.GetFileList(pstrFileSpec, pintTimeout)
    '        Dim e As IEnumerator = a.GetEnumerator
    '        While e.MoveNext
    '            If CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Type = Mabry.Net.Clients.Ftp.Ftp.FileTypes.File Then
    '                'add to return value
    '                ReturnValue.Add(CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Name)
    '            End If
    '        End While
    '        'clean up
    '        FTP.Disconnect()
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Return ReturnValue

    'FileList_Error:
    '        Dim Message As String = Err.Description
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Throw New Exception(Message)

    '    End Function

    '    Public Function FileListAsString(ByVal pstrServer As String, _
    '       ByVal pstrUsername As String, _
    '       ByVal pstrPassword As String, _
    '       Optional ByVal pstrFileSpec As String = "", _
    '       Optional ByVal pintTimeout As Integer = 20, _
    '       Optional ByVal pintPort As Integer = 21) As String
    '        'Purpose:   Return a list of server filenames.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrFileSpec = optional string name, including wildcard search pattern, for 
    '        '           files being sought.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        'Return:    Comma delimited string of filenames.

    '        'call the normal FileList method, convert to a string array, then flatten to string
    '        Return Join(FileList(pstrServer, pstrUsername, pstrPassword, pstrFileSpec, pintTimeout, pintPort).ToArray, ",")

    '    End Function

    '    Public Function FileSize(ByVal pstrServer As String, _
    '       ByVal pstrUsername As String, _
    '       ByVal pstrPassword As String, _
    '       ByVal pstrFileName As String, _
    '       Optional ByVal pintTimeout As Integer = 20, _
    '       Optional ByVal pintPort As Integer = 21) As Long
    '        'Purpose:   Return the size of a server file.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrFilename = string name of the file whose size is being sought.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        'Return:    Size of the file in bytes.

    '        On Error GoTo FileSize_Error

    '        'error on wildcards
    '        If InStr(pstrFileName, "*") > 0 Or InStr(pstrFileName, "?") > 0 Then
    '            Throw New Exception("FileName parameter cannot contain wildcard characters")
    '        End If

    '        'create instance
    '        Dim FTP As New Mabry.Net.Clients.Ftp.Ftp
    '        Dim ReturnValue As Integer = 0

    '        'connect
    '        FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, pintTimeout)

    '        'user should call FileExists first; penalize them if they don't
    '        If Me._FileExists(FTP, pstrFileName, pintTimeout) = False Then
    '            FTP.Disconnect()
    '            FTP.Dispose()
    '            FTP = Nothing
    '            Throw New Exception("File not found")
    '        End If

    '        'find the file's attributes and capture its size
    '        Dim a As Array = FTP.GetFileList(Trim(pstrFileName), pintTimeout)
    '        Dim e As IEnumerator = a.GetEnumerator
    '        While e.MoveNext
    '            If CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Type = Mabry.Net.Clients.Ftp.Ftp.FileTypes.File AndAlso _
    '                LCase(CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Name) = LCase(Right(pstrFileName, Len(CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Name))) Then
    '                ReturnValue = CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Size
    '                Exit While
    '            End If
    '        End While
    '        'clean up
    '        FTP.Disconnect()
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Return ReturnValue

    'FileSize_Error:
    '        Dim Message As String = Err.Description
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Throw New Exception(Message)

    '    End Function

    '    Public Function FolderExists(ByVal pstrServer As String, _
    '       ByVal pstrUsername As String, _
    '       ByVal pstrPassword As String, _
    '       ByVal pstrFolderName As String, _
    '       Optional ByVal pintTimeout As Integer = 20, _
    '       Optional ByVal pintPort As Integer = 21) As Boolean
    '        'Purpose:   Return the size of a server file.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrFoldername = string name of the folder being sought.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        'Return:    True if folder exists; otherwise False.

    '        On Error GoTo FolderExists_Error

    '        'error on wildcards
    '        If InStr(pstrFolderName, "*") > 0 Or InStr(pstrFolderName, "?") > 0 Then
    '            Throw New Exception("FolderName parameter cannot contain wildcard characters.")
    '            Exit Function
    '        End If

    '        Dim ReturnValue As Boolean = False

    '        'try to change to the directory; will return true if Dir change was successful
    '        ReturnValue = Me._ChangeDirectory(pstrServer, pstrUsername, pstrPassword, pstrFolderName, pintTimeout, pintPort)

    '        Return ReturnValue

    'FolderExists_Error:

    '        Return ReturnValue

    '    End Function

    '    Public Function FolderList(ByVal pstrServer As String, _
    '       ByVal pstrUsername As String, _
    '       ByVal pstrPassword As String, _
    '       Optional ByVal pstrFolderSpec As String = "", _
    '       Optional ByVal pintTimeout As Integer = 20, _
    '       Optional ByVal pintPort As Integer = 21) As ArrayList
    '        'Purpose:   Return a list of server folders.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrFolderSpec = optional string name for the root folder being searched.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        'Return:    ArrayList of sub-folder names.

    '        On Error GoTo FolderList_Error

    '        'create
    '        Dim FTP As New Mabry.Net.Clients.Ftp.Ftp
    '        Dim ReturnValue As New ArrayList

    '        'connect
    '        FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, pintTimeout)

    '        'find all sub-folders in the folder specified by pstrFolderSpec
    '        Dim a As Array = FTP.GetFileList(pstrFolderSpec, pintTimeout)
    '        Dim e As IEnumerator = a.GetEnumerator
    '        While e.MoveNext
    '            If CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Type = Mabry.Net.Clients.Ftp.Ftp.FileTypes.Directory Then
    '                'add to ArrayList if folder
    '                ReturnValue.Add(CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Name)
    '            End If
    '        End While
    '        'clean up
    '        FTP.Disconnect()
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Return ReturnValue

    'FolderList_Error:
    '        Dim Message As String = Err.Description
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Throw New Exception(Message)

    '    End Function

    '    Public Function FolderListAsString(ByVal pstrServer As String, _
    '       ByVal pstrUsername As String, _
    '       ByVal pstrPassword As String, _
    '       Optional ByVal pstrFolderSpec As String = "", _
    '       Optional ByVal pintTimeout As Integer = 20, _
    '       Optional ByVal pintPort As Integer = 21) As String
    '        'Purpose:   Return a list of server folders.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrFolderSpec = optional string name for the root folder being searched.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        'Return:    Comma delimited string of sub-folder names.

    '        'call the normal FolderList method, convert to a string array, then flatten to string
    '        Return Join(FolderList(pstrServer, pstrUsername, pstrPassword, pstrFolderSpec, pintTimeout, pintPort).ToArray, ",")

    '    End Function

    '    Public Sub GetFile(ByVal pstrServer As String, _
    '       ByVal pstrUsername As String, _
    '       ByVal pstrPassword As String, _
    '       ByVal pstrSourceFile As String, _
    '       ByVal pstrDestinationFile As String, _
    '       Optional ByVal TransferMode As FTP.TransferType = FTP.TransferType.Ascii, _
    '       Optional ByVal pintTimeout As Integer = 20, _
    '       Optional ByVal pintPort As Integer = 21, _
    '       Optional ByVal pstrPreCommand As String = "", _
    '       Optional ByVal pstrPostCommand As String = "")
    '        'Purpose:   Copy a server file to a local disk file
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrSourceFile = string name for the server file being copied.
    '        '           pstrDestinationFile = string name for the local file that will be created/overwritten.
    '        '           TransferMode = indicates whether to transfer the file as ASCII or binary.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        '           pstrPreCommand = semi-colon delimited string of pre-GetFile commands that will
    '        '           be sent prior to the GetFile command.
    '        '           pstrPostCommand = semi-colon delimited string of post-GetFile commands that will
    '        '           be sent after the GetFile command.
    '        'Return:    None.


    '        On Error GoTo GetFile_Error

    '        'create
    '        Dim FTP As New Mabry.Net.Clients.Ftp.Ftp
    '        'connect
    '        FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, pintTimeout)
    '        'set transfer mode
    '        FTP.TransferType = TransferMode

    '        'pre-file commands
    '        If pstrPreCommand <> "" Then
    '            Dim strCmds() As String = Split(pstrPreCommand, ";")
    '            Dim Enumerator As IEnumerator = strCmds.GetEnumerator
    '            While Enumerator.MoveNext
    '                'execute the command
    '                _ExecuteCommand(FTP, Enumerator.Current, pintTimeout)
    '            End While
    '        End If

    '        'get file
    '        FTP.GetFile(pstrSourceFile, pstrDestinationFile, pintTimeout)

    '        'post-file commands
    '        If pstrPostCommand <> "" Then
    '            Dim strCmds() As String = Split(pstrPostCommand, ";")
    '            Dim Enumerator As IEnumerator = strCmds.GetEnumerator
    '            While Enumerator.MoveNext
    '                'execute the command
    '                _ExecuteCommand(FTP, Enumerator.Current, pintTimeout)
    '            End While
    '        End If

    '        'disconnect
    '        FTP.Disconnect()

    '        Exit Sub
    'GetFile_Error:
    '        Dim Message As String = Err.Description
    '        If FTP.Connected = True Then
    '            FTP.Disconnect()
    '        End If
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Throw New Exception(Message)

    '    End Sub

    '    Private Sub GetFile(ByVal pstrServer As String, _
    '        ByVal pstrUsername As String, _
    '        ByVal pstrPassword As String, _
    '        ByVal pstrSourceFile As String, _
    '        ByVal DestinationStream As System.IO.Stream, _
    '        Optional ByVal TransferMode As FTP.TransferType = FTP.TransferType.Ascii, _
    '        Optional ByVal pintTimeout As Integer = 20, _
    '        Optional ByVal pintPort As Integer = 21, _
    '        Optional ByVal pstrPreCommand As String = "", _
    '        Optional ByVal pstrPostCommand As String = "")
    '        'Purpose:   Copy a server file to a local stream
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrSourceFile = string name for the server file being copied.
    '        '           DestinationStream = stream object to receive the data.
    '        '           TransferMode = indicates whether to transfer the file as ASCII or binary.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        '           pstrPreCommand = semi-colon delimited string of pre-GetFile commands that will
    '        '           be sent prior to the GetFile command.
    '        '           pstrPostCommand = semi-colon delimited string of post-GetFile commands that will
    '        '           be sent after the GetFile command.
    '        'Return:    None.
    '        On Error GoTo GetFile_Error

    '        'create
    '        Dim FTP As New Mabry.Net.Clients.Ftp.Ftp
    '        'connect
    '        FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, pintTimeout)
    '        'set transfer mode
    '        FTP.TransferType = TransferMode

    '        'pre-file commands
    '        If pstrPreCommand <> "" Then
    '            Dim strCmds() As String = Split(pstrPreCommand, ";")
    '            Dim Enumerator As IEnumerator = strCmds.GetEnumerator
    '            While Enumerator.MoveNext
    '                'execute the command
    '                _ExecuteCommand(FTP, Enumerator.Current, pintTimeout)
    '            End While
    '        End If

    '        'get file
    '        FTP.GetFile(pstrSourceFile, DestinationStream, pintTimeout)

    '        'post-file commands
    '        If pstrPostCommand <> "" Then
    '            Dim strCmds() As String = Split(pstrPostCommand, ";")
    '            Dim Enumerator As IEnumerator = strCmds.GetEnumerator
    '            While Enumerator.MoveNext
    '                'execute the command
    '                _ExecuteCommand(FTP, Enumerator.Current, pintTimeout)
    '            End While
    '        End If
    '        'disconnect
    '        FTP.Disconnect()

    '        Exit Sub
    'GetFile_Error:
    '        Dim Message As String = Err.Description
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Throw New Exception(Message)

    '    End Sub

    '    Public Sub PutFile(ByVal pstrServer As String, _
    '       ByVal pstrUsername As String, _
    '       ByVal pstrPassword As String, _
    '       ByVal pstrSourceFile As String, _
    '       ByVal pstrDestinationFile As String, _
    '       Optional ByVal TransferMode As FTP.TransferType = FTP.TransferType.Ascii, _
    '       Optional ByVal pintTimeout As Integer = 20, _
    '       Optional ByVal pintPort As Integer = 21, _
    '       Optional ByVal pstrPreCommand As String = "", _
    '       Optional ByVal pstrPostCommand As String = "")
    '        'Purpose:   Overloads the PutFile method that passes a source stream w/out byte size.
    '        'Return:    None.

    '        Dim FileStream As FileStream
    '        Try
    '            FileStream = New FileStream(pstrSourceFile, FileMode.Open)
    '            PutFile(pstrServer, pstrUsername, pstrPassword, FileStream, pstrDestinationFile, TransferMode, pintTimeout, pintPort, pstrPreCommand, pstrPostCommand)
    '            FileStream.Close()
    '        Catch ex As Exception
    '            'close the filestream
    '            FileStream.Close()
    '            'bubble the exception up
    '            Throw New Exception(ex.Message)
    '        End Try
    '    End Sub

    '    Private Sub PutFile(ByVal pstrServer As String, _
    '        ByVal pstrUsername As String, _
    '        ByVal pstrPassword As String, _
    '        ByVal SourceStream As System.IO.Stream, _
    '        ByVal pstrDestinationFile As String, _
    '        Optional ByVal TransferMode As FTP.TransferType = FTP.TransferType.Ascii, _
    '        Optional ByVal pintTimeout As Integer = 20, _
    '        Optional ByVal pintPort As Integer = 21, _
    '        Optional ByVal pstrPreCommand As String = "", _
    '        Optional ByVal pstrPostCommand As String = "")
    '        'Purpose:   Copy a local stream to the server.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           SourceStream = local stream to be copied to the server.
    '        '           pstrDestinationFile = string name for the server file.
    '        '           TransferMode = indicates whether to transfer the file as ASCII or binary.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        '           pstrPreCommand = semi-colon delimited string of pre-GetFile commands that will
    '        '           be sent prior to the GetFile command.
    '        '           pstrPostCommand = semi-colon delimited string of post-GetFile commands that will
    '        '           be sent after the GetFile command.
    '        'Return:    None.

    '        On Error GoTo PutFile_Error

    '        'create
    '        Dim FTP As New Mabry.Net.Clients.Ftp.Ftp
    '        'connect
    '        FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, pintTimeout)
    '        'set transfer mode
    '        FTP.TransferType = TransferMode

    '        'pre-file commands
    '        If pstrPreCommand <> "" Then
    '            Dim strCmds() As String = Split(pstrPreCommand, ";")
    '            Dim Enumerator As IEnumerator = strCmds.GetEnumerator
    '            While Enumerator.MoveNext
    '                'execute the command
    '                _ExecuteCommand(FTP, Enumerator.Current, pintTimeout)
    '            End While
    '        End If

    '        'put file
    '        FTP.PutFile(SourceStream, pstrDestinationFile, pintTimeout)

    '        'post-file commands
    '        If pstrPostCommand <> "" Then
    '            Dim strCmds() As String = Split(pstrPostCommand, ";")
    '            Dim Enumerator As IEnumerator = strCmds.GetEnumerator
    '            While Enumerator.MoveNext
    '                'execute the command
    '                _ExecuteCommand(FTP, Enumerator.Current, pintTimeout)
    '            End While
    '        End If

    '        'disconnect
    '        FTP.Disconnect()

    '        Exit Sub
    'PutFile_Error:
    '        Dim Message As String = Err.Description
    '        If FTP.Connected = True Then
    '            FTP.Disconnect()
    '        End If
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Throw New Exception(Message)

    '    End Sub

    '    Public Sub PutFile(ByVal pstrServer As String, _
    '       ByVal pstrUsername As String, _
    '       ByVal pstrPassword As String, _
    '       ByVal pstrSourceFile As String, _
    '       ByVal pstrDestinationFile As String, _
    '       ByVal pintBytes As Integer, _
    '       Optional ByVal TransferMode As FTP.TransferType = FTP.TransferType.Ascii, _
    '       Optional ByVal pintTimeout As Integer = 20, _
    '       Optional ByVal pintPort As Integer = 21)
    '        'Purpose:   Overloads the PutFile method that passes a source Stream and Byte size.

    '        Dim FileStream As FileStream
    '        Try
    '            FileStream = New FileStream(pstrSourceFile, FileMode.Open)
    '            PutFile(pstrServer, pstrUsername, pstrPassword, FileStream, pstrDestinationFile, pintBytes, TransferMode, pintTimeout, pintPort)
    '            FileStream.Close()
    '        Catch ex As Exception
    '            'close the filestream
    '            FileStream.Close()
    '            'bubble the exception up
    '            Throw New Exception(ex.Message)
    '        End Try

    '    End Sub

    '    Private Sub PutFile(ByVal pstrServer As String, _
    '            ByVal pstrUsername As String, _
    '            ByVal pstrPassword As String, _
    '            ByVal SourceStream As System.IO.Stream, _
    '            ByVal pstrDestinationFile As String, _
    '            ByVal pintBytes As Integer, _
    '            Optional ByVal TransferMode As FTP.TransferType = FTP.TransferType.Ascii, _
    '            Optional ByVal pintTimeout As Integer = 20, _
    '            Optional ByVal pintPort As Integer = 21)
    '        'Purpose:   Copy a local file to the server.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           SourceStream = local stream to be copied to the server.
    '        '           pstrDestinationFile = string name for the server file.
    '        '           pintBytes = size to Allocate
    '        '           TransferMode = indicates whether to transfer the file as ASCII or binary.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        'Return:    None.
    '        On Error GoTo PutFile_Error

    '        'create
    '        Dim FTP As New Mabry.Net.Clients.Ftp.Ftp

    '        'connect
    '        FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, pintTimeout)

    '        'send the allocate command
    '        _ExecuteCommand(FTP, "ALLO " & CStr(pintBytes), pintTimeout)

    '        'set transfer mode
    '        FTP.TransferType = TransferMode
    '        'put file
    '        FTP.PutFile(SourceStream, pstrDestinationFile, pintTimeout)
    '        'disconnect
    '        FTP.Disconnect()

    '        Exit Sub
    'PutFile_Error:
    '        Dim Message As String = Err.Description
    '        If FTP.Connected = True Then
    '            FTP.Disconnect()
    '        End If
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Throw New Exception(Message)

    '    End Sub

    '    Private Function _ChangeDirectory(ByVal pstrServer As String, _
    '        ByVal pstrUsername As String, _
    '        ByVal pstrPassword As String, _
    '        ByVal pstrTargetDirectory As String, _
    '        Optional ByVal pintTimeout As Integer = 20, _
    '        Optional ByVal pintPort As Integer = 21) As Boolean
    '        'Purpose:   Change to a different directory on the server.
    '        'Input:     pstrServer = string hostname or IP address of the FTP server to connect to.
    '        '           pstrUsername = string username used to login to the FTP server.
    '        '           pstrPassword = string password used to login to the FTP server.
    '        '           pstrTargetDirectory = string folder name on the FTP server to change to.
    '        '           pintTimeout = optional integer indicating the number of seconds 
    '        '           to wait for the method to complete before throwing an error. 
    '        '           If a timeout error occurs, SocketException with an ErrorCode 
    '        '           value of 10061 will be thrown.
    '        '           pintPort = optional integer used to connect to the FTP server.
    '        'Return:    Boolean if the method completes successfully; otherwise, an exception is thrown.


    '        On Error GoTo ChangeDirectory_Error

    '        Dim FTP As New Mabry.Net.Clients.Ftp.Ftp
    '        Dim ReturnValue As Boolean = False

    '        FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, pintTimeout)
    '        FTP.ChangeDirectory(pstrTargetDirectory, pintTimeout)
    '        ReturnValue = True
    '        FTP.Disconnect()
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Return ReturnValue

    'ChangeDirectory_Error:
    '        Dim Message As String = Err.Description
    '        FTP.Dispose()
    '        FTP = Nothing

    '        Throw New Exception(Message)

    '    End Function

    Private Sub _ExecuteCommand(ByVal FTP As Mabry.Net.Clients.Ftp.Ftp, _
        ByVal pstrCommand As String, _
        ByVal pintTimeout As Integer)

        'send the command
        FTP.SendFtpCommand(pstrCommand, pintTimeout)

        'capture the response
        Dim arrResponse As System.Array = FTP.ReadServerResponse(pintTimeout)

        'verify response contains a 2XX level code
        If Left(arrResponse.GetValue(LBound(arrResponse)), 1) <> "2" Then
            'negative response to ALLOcate

            'build the response string for the error message
            Dim strResponse As String = ""
            Dim Enumerator As IEnumerator = arrResponse.GetEnumerator
            While Enumerator.MoveNext
                strResponse &= Enumerator.Current & vbCrLf
            End While
            'strip the trailing CrLf that we added
            If Right(strResponse, 2) = vbCrLf Then
                strResponse = Left(strResponse, Len(strResponse) - 2)
            End If

            'throw the error
            Throw New Exception("Non-positive server response from command " & pstrCommand & " (" & strResponse & ").")
        End If
    End Sub

    Private Function _FileExists(ByRef FTP As Mabry.Net.Clients.Ftp.Ftp, _
        ByVal pstrFileName As String, _
        ByVal pintTimeout As Integer) As Boolean
        'Purpose:   Determine whether a file is present on the server.
        'Input:     FTP = instance of the FTP component.
        '           pstrFileName = string path name of the file to test.
        '           pintTimeout = integer indicating the number of seconds 
        '           to wait for the method to complete before throwing an error. 
        '           If a timeout error occurs, SocketException with an ErrorCode 
        '           value of 10061 will be thrown.
        'Return:    Boolean if the file exists on the server; otherwise, False

        Dim a As Array = FTP.GetFileList(Trim(pstrFileName), pintTimeout)
        Dim e As IEnumerator = a.GetEnumerator
        While e.MoveNext
            If CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Type = Mabry.Net.Clients.Ftp.Ftp.FileTypes.File AndAlso _
                LCase(CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Name) = LCase(Right(pstrFileName, Len(CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Name))) Then
                Return True
            End If
        End While

        Return False

    End Function
#End Region

    Public Sub New()
        _License = New Mabry.Net.Clients.Ftp.Licenser
        _License.Key = "XB1C-Q86AKQH2EBYP"
        _FTP = New Mabry.Net.Clients.Ftp.Ftp
    End Sub

#Region "Discrete Methods"
    ''' <summary>
    '''     Wrapper for the ChangeDirectory method; Change to a specified folder on the server.
    ''' </summary>
    ''' <param name="pstrDirectoryName">Name of the folder to change to.</param>
    ''' <param name="pintTimeout">Optional integer indicating the number of seconds 
    '''         to wait for the method to complete before throwing an error. 
    '''         If a timeout error occurs, SocketException with an ErrorCode 
    '''         value of 10061 will be thrown. If no value is passed, the method will 
    '''         use the default value assigned when the connection was established.
    ''' </param>
    ''' <remarks>
    '''     Assumes a connect has already been established. If not, at exception is thrown.
    ''' </remarks>
    Public Sub ChangeDirectory(ByVal pstrDirectoryName As String, _
        Optional ByVal pintTimeout As Integer = 0)

        On Error GoTo Local_Error

        'make sure we are connected
        If _FTP.Connected = False Then
            Throw New Exception("Not connected to FTP server during ChangeDirectory command.")
        End If

        'use default or value set during Connect()
        If pintTimeout < 1 Then
            pintTimeout = _Timeout
        End If

        
        'note - an exception will occur if the file does not exist
        _FTP.ChangeDirectory(pstrDirectoryName, pintTimeout)
        Exit Sub

Local_Error:
        Throw New Exception("FTP ChangeDirectory: " & Err.Description)
    End Sub
    ''' <summary>
    '''     Establish a connection to the FTP server.
    ''' </summary>
    ''' <param name="pstrServer">Host name or IP address of the FTP server.</param>
    ''' <param name="pstrUsername">Username to use when opening the FTP connection.</param>
    ''' <param name="pstrPassword">Password to use when opening the FTP connection.</param>
    ''' <param name="pintTimeout">Optional integer indicating the number of seconds 
    '''         to wait for the method to complete before throwing an error. 
    '''         If a timeout error occurs, SocketException with an ErrorCode 
    '''         value of 10061 will be thrown. 
    ''' </param>
    ''' <param name="pintPort">Optional TCP port number that the FTP server listens for incoming connections on.</param>
    ''' <remarks>
    '''     Client should call the Disconnect method when all method calls are complete.
    ''' </remarks>
    Public Function Connect(ByVal pstrServer As String, _
        ByVal pstrUsername As String, _
        ByVal pstrPassword As String, _
        Optional ByVal pintTimeout As Integer = 20, _
        Optional ByVal pintPort As Integer = 21, _
        Optional ByVal PassiveMode As Boolean = False) As Boolean

        'set a default timeout
        _Timeout = pintTimeout
        '_PassiveMode = PassiveMode

        Try
            _FTP.PassiveMode = PassiveMode
            _FTP.Connect(pstrServer, pintPort, pstrUsername, pstrPassword, _Timeout)


        Catch ex As Exception
            're-initialize
            _Initialize()
            Throw New Exception("FTP Connect: " & ex.Message)
        End Try
        Return _FTP.Connected
    End Function
    ''' <summary>
    '''     Wrapper for the Connected property; True if the component is connected to an FTP server and
    '''     ready to accept commands. Otherwise, False.
    ''' </summary>
    Public ReadOnly Property Connected() As Boolean
        Get
            Return _FTP.Connected
        End Get
    End Property
    ''' <summary>
    '''     Wrapper for the DeleteFile method; Deletes a file on the FTP server.
    ''' </summary>
    ''' <param name="pstrFileName">Name of the file to delete. Wildcards not allowed.</param>
    ''' <param name="pintTimeout">Optional integer indicating the number of seconds 
    '''         to wait for the method to complete before throwing an error. 
    '''         If a timeout error occurs, SocketException with an ErrorCode 
    '''         value of 10061 will be thrown. If no value is passed, the method will 
    '''         use the default value assigned when the connection was established.
    ''' </param>
    ''' <remarks>
    '''     Assumes a connect has already been established. If not, at exception is thrown.
    ''' </remarks>
    Public Sub DeleteFile(ByVal pstrFileName As String, _
        Optional ByVal pintTimeout As Integer = 0)

        On Error GoTo Local_Error

        'does not support batch deletes
        If InStr(pstrFileName, "*") > 0 Or InStr(pstrFileName, "?") > 0 Then
            Throw New Exception("FileName parameter cannot contain wildcard characters.")
            Exit Sub
        End If

        'make sure we are connected
        If _FTP.Connected = False Then
            Throw New Exception("Not connected to FTP server during DeleteFile command.")
        End If

        'use default or value set during Connect()
        If pintTimeout < 1 Then
            pintTimeout = _Timeout
        End If

        'note - an exception will occur if the file does not exist
        _FTP.DeleteFile(pstrFileName, pintTimeout)
        Exit Sub

Local_Error:
        Throw New Exception("FTP DeleteFile: " & Err.Description)
    End Sub
    ''' <summary>
    '''     Terminate a connection with the FTP server.
    ''' </summary>
    Public Sub Disconnect()
        Try
            Select Case _FTP.State.Code
                Case Mabry.Net.Clients.Ftp.States.NotConnected
                    'do nothing
                Case Mabry.Net.Clients.Ftp.States.Connected
                    'disconnect - this should be the case 99.997% of the time
                    _FTP.Disconnect()
                Case Else
                    'generally the caller shouldn't try to disconnect mid-command
                    'but handle it
                    _FTP.Abort(_Timeout)
                    _FTP.Disconnect()
            End Select
        Catch ex As Exception
            're-initialize
            _Initialize()
            Throw New Exception("FTP Disconnect: " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    '''     Determines if a file exists on the server. Useful because other functions may fail
    '''     based on the presence (or lack thereof) of a server file.
    ''' </summary>
    ''' <param name="pstrFileName">Name of the file to check.</param>
    ''' <param name="pintTimeout">Optional integer indicating the number of seconds 
    '''         to wait for the method to complete before throwing an error. 
    '''         If a timeout error occurs, SocketException with an ErrorCode 
    '''         value of 10061 will be thrown. If no value is passed, the method will 
    '''         use the default value assigned when the connection was established.
    ''' </param>
    ''' <remarks>
    '''     Assumes a connect has already been established. If not, at exception is thrown.
    '''     True if the file exists on the server; otherwise, False.
    ''' </remarks>
    Public Function FileExists(ByVal pstrFileName As String, _
        Optional ByVal pintTimeout As Integer = 0) As Boolean

        On Error GoTo Local_Error

        'make sure we are connected
        If _FTP.Connected = False Then
            Throw New Exception("Not connected to FTP server during FileList command.")
        End If

        'use default or value set during Connect()
        If pintTimeout < 1 Then
            pintTimeout = _Timeout
        End If

        Dim a As Array = _FTP.GetFileList(Trim(pstrFileName), pintTimeout)
        Dim e As IEnumerator = a.GetEnumerator
        While e.MoveNext
            If CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Type = Mabry.Net.Clients.Ftp.Ftp.FileTypes.File AndAlso _
                LCase(CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Name) = LCase(Right(pstrFileName, Len(CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Name))) Then
                Return True
            End If
        End While

        Return False

Local_Error:
        Throw New Exception("FTP FileExists: " & Err.Description)
    End Function

    ''' <summary>
    '''     Return a list of server filenames.
    ''' </summary>
    ''' <param name="pstrFileSpec">Optional search string, including wildcard search pattern, for 
    '''         files being sought.</param>
    ''' <param name="pintTimeout">Optional integer indicating the number of seconds 
    '''         to wait for the method to complete before throwing an error. 
    '''         If a timeout error occurs, SocketException with an ErrorCode 
    '''         value of 10061 will be thrown. If no value is passed, the method will 
    '''         use the default value assigned when the connection was established.
    ''' </param>
    ''' <remarks>
    '''     Assumes a connect has already been established. If not, at exception is thrown.
    '''     Returns an ArrayList of filenames found.
    ''' </remarks>
    Public Function FileList(Optional ByVal pstrFileSpec As String = "", _
        Optional ByVal pintTimeout As Integer = 0) As ArrayList

        On Error GoTo Local_Error

        Dim ReturnValue As New ArrayList

        'make sure we are connected
        If _FTP.Connected = False Then
            Throw New Exception("Not connected to FTP server during FileList command.")
        End If

        'use default or value set during Connect()
        If pintTimeout < 1 Then
            pintTimeout = _Timeout
        End If

        'fetch list
        Dim a As Array = _FTP.GetFileList(pstrFileSpec, pintTimeout)
        Dim e As IEnumerator = a.GetEnumerator
        While e.MoveNext
            If CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Type = Mabry.Net.Clients.Ftp.Ftp.FileTypes.File Then
                'add to return value
                ReturnValue.Add(CType(e.Current, Mabry.Net.Clients.Ftp.FtpFileInfo).Name)
            End If
        End While

        Return ReturnValue

Local_Error:
        Throw New Exception("FTP FileList: " & Err.Description)
    End Function
    ''' <summary>
    '''     Wrapper for the GetFile method; Copies a server file to a local disk file.
    ''' </summary>
    ''' <param name="pstrSourceFile">String name for the server file being copied.</param>
    ''' <param name="pstrDestinationFile">String name for the local file that will be created/overwritten.</param>
    ''' <param name="pblnAsciiTransferMode">Indicates whether to transfer the file as ASCII or binary.</param>
    ''' <param name="pintTimeout">Optional integer indicating the number of seconds 
    '''         to wait for the method to complete before throwing an error. 
    '''         If a timeout error occurs, SocketException with an ErrorCode 
    '''         value of 10061 will be thrown. If no value is passed, the method will 
    '''         use the default value assigned when the connection was established.
    ''' </param>
    ''' <remarks>
    '''     Assumes a connect has already been established. If not, at exception is thrown.
    ''' </remarks>
    Public Sub GetFile(ByVal pstrSourceFile As String, _
        ByVal pstrDestinationFile As String, _
        Optional ByVal pblnAsciiTransferMode As Boolean = True, _
        Optional ByVal pintTimeout As Integer = 0)

        On Error GoTo Local_Error

        'make sure we are connected
        If _FTP.Connected = False Then
            Throw New Exception("Not connected to FTP server during GetFile command.")
        End If

        'use default or value set during Connect()
        If pintTimeout < 1 Then
            pintTimeout = _Timeout
        End If

        'set transfer mode
        If pblnAsciiTransferMode = True Then
            _FTP.TransferType = TransferTypes.Ascii
        Else
            _FTP.TransferType = TransferTypes.Binary
        End If


        'get file
        _FTP.GetFile(pstrSourceFile, pstrDestinationFile, pintTimeout)
        Exit Sub

Local_Error:
        Throw New Exception("FTP GetFile: " & Err.Description)
    End Sub
    ''' <summary>
    '''     Wrapper for the PutFile method; Copies a server file to a local disk file.
    ''' </summary>
    ''' <param name="pstrSourceFile">String name of the local file that will be copied to the server.</param>
    ''' <param name="pstrDestinationFile">String name for server file that will be created/overwritten.</param>
    ''' <param name="pblnAsciiTransferMode">Indicates whether to transfer the file as ASCII or binary.</param>
    ''' <param name="pintTimeout">Optional integer indicating the number of seconds 
    '''         to wait for the method to complete before throwing an error. 
    '''         If a timeout error occurs, SocketException with an ErrorCode 
    '''         value of 10061 will be thrown. If no value is passed, the method will 
    '''         use the default value assigned when the connection was established.
    ''' </param>
    ''' <remarks>
    '''     Assumes a connect has already been established. If not, at exception is thrown.
    ''' </remarks>
    Public Sub PutFile(ByVal pstrSourceFile As String, _
        ByVal pstrDestinationFile As String, _
        Optional ByVal pblnAsciiTransferMode As Boolean = True, _
        Optional ByVal pintTimeout As Integer = 0)

        On Error GoTo Local_Error

        'make sure we are connected
        If _FTP.Connected = False Then
            Throw New Exception("Not connected to FTP server during PutFile command.")
        End If

        'use default or value set during Connect()
        If pintTimeout < 1 Then
            pintTimeout = _Timeout
        End If

        'set transfer mode
        If pblnAsciiTransferMode = True Then
            _FTP.TransferType = TransferTypes.Ascii
        Else
            _FTP.TransferType = TransferTypes.Binary
        End If

        'put file
        _FTP.PutFile(pstrSourceFile, pstrDestinationFile, pintTimeout)
        Exit Sub

Local_Error:
        Throw New Exception("FTP PutFile: " & Err.Description)
    End Sub
    ''' <summary>
    '''     Wrapper for the Rename method; renames a server file.
    ''' </summary>
    ''' <param name="pstrOriginalName">String name of the server file being renamed.</param>
    ''' <param name="pstrNewName">New name of the server file.</param>
    ''' <param name="pintTimeout">Optional integer indicating the number of seconds 
    '''         to wait for the method to complete before throwing an error. 
    '''         If a timeout error occurs, SocketException with an ErrorCode 
    '''         value of 10061 will be thrown. If no value is passed, the method will 
    '''         use the default value assigned when the connection was established.
    ''' </param>
    ''' <remarks>
    '''     Assumes a connect has already been established. If not, at exception is thrown.
    '''     If the file does not exist under the original name, an exception will be thrown.
    '''     If another file exists which matches the new name, an exception will be thrown.
    ''' </remarks>
    Public Sub RenameFile(ByVal pstrOriginalName As String, _
        ByVal pstrNewName As String, _
        Optional ByVal pintTimeout As Integer = 0)

        On Error GoTo Local_Error

        'make sure we are connected
        If _FTP.Connected = False Then
            Throw New Exception("Not connected to FTP server during RenameFile command.")
        End If

        'use default or value set during Connect()
        If pintTimeout < 1 Then
            pintTimeout = _Timeout
        End If

        'rename file
        _FTP.Rename(pstrOriginalName, pstrNewName, pintTimeout)

        Exit Sub

Local_Error:
        Throw New Exception("FTP Rename: " & Err.Description)
    End Sub
    ''' <summary>
    '''     Private method used to re-initialize the component in the event of some exception.
    ''' </summary>
    Private Sub _Initialize()
        'do over
        On Error Resume Next
        If _FTP.Connected = True Then
            _FTP.Disconnect()
        End If
        _FTP.Dispose()
        _FTP = Nothing
        _FTP = New Mabry.Net.Clients.Ftp.Ftp
    End Sub
#End Region
End Class


