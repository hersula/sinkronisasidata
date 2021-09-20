Imports VB = Microsoft.VisualBasic
Imports System.IO
Imports System.Security.Cryptography
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.OleDb
'For time server
Imports System.Runtime.InteropServices

Public Class ModFuction

    Public Function pecahdata(ByVal pecah As String, ByVal urutan As Integer) As String
        Dim pisahdata() As String
        Dim hasil As String
        pisahdata = Split(pecah, "-", , vbTextCompare)
        If UBound(pisahdata) > 0 Then
            hasil = pisahdata(urutan)
        Else
            hasil = pecah
        End If
        Return hasil
    End Function

    Public Function pecahdatadesimal(ByVal pecah As String, ByVal urutan As Integer) As String
        Dim pisahdata() As String
        Dim hasil As String
        pisahdata = Split(pecah, ",", , vbTextCompare)
        If UBound(pisahdata) > 0 Then
            hasil = pisahdata(urutan)
        Else
            hasil = pecah
        End If
        Return hasil
    End Function

   
    Function NullToStr(ByVal inputVar As Object) As String
        If Trim(inputVar & "") = "" Then
            NullToStr = ""
        Else
            NullToStr = "" & inputVar & ""
        End If
    End Function


    'txt_encrypt.text = EncryptText(txt_decrypt.text)
    Public Function EncryptText(ByVal SourceText As System.String) As System.String
        Dim MyKey As String = "Powered by F1KAR"
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim strResult As System.String = ""

        Try
            Dim bykey() As Byte = System.Text.Encoding.UTF8.GetBytes(Strings.Left(MyKey, 8))
            Dim InputByteArray() As Byte = System.Text.Encoding.UTF8.GetBytes(SourceText)
            Dim des As New DESCryptoServiceProvider
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(bykey, IV), CryptoStreamMode.Write)
            cs.Write(InputByteArray, 0, InputByteArray.Length)
            cs.FlushFinalBlock()
            strResult = Convert.ToBase64String(ms.ToArray())
        Catch ex As Exception
            'Throw New Exception
            Return strResult
        End Try
        Return strResult
    End Function
    'txt_decrypt.text = DecryptText(txt_encrypt.text)
    Public Function DecryptText(ByVal Chippedtexta As System.String) As System.String
        Dim mykey As String = "Powered by F1KAR"
        Dim iv() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim inputbytearray(Chippedtexta.Length) As Byte
        Dim strresult As System.String
        Try
            Dim bykey() As Byte = System.Text.Encoding.UTF8.GetBytes(Strings.Left(mykey, 8))
            Dim des As New DESCryptoServiceProvider
            inputbytearray = Convert.FromBase64String(Chippedtexta)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(bykey, iv), CryptoStreamMode.Write)
            cs.Write(inputbytearray, 0, inputbytearray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            strresult = encoding.GetString(ms.ToArray())
            Return strresult
        Catch ex As Exception
            'Throw New Exception
            'MsgBox("File Config was Broken..!!")
            Return vbFalse
        End Try
    End Function

    'fungsi untuk write file .ini
    Private Declare Unicode Function WritePrivateProfileString Lib "kernel32" _
    Alias "WritePrivateProfileStringW" (ByVal lpSection As String, ByVal lpParamName As String, _
    ByVal lpParamVal As String, ByVal lpFileName As String) As Int32

    'procedure untuk write .ini
    Public Sub writeini(ByVal iniFilename As String, ByVal section As String, ByVal ParamName As String, ByVal ParamVal As String)
        'menanggil fungsi WritePrivateProfilString untuk write file .ini
        Dim result As Integer = WritePrivateProfileString(section, ParamName, ParamVal, iniFilename)
    End Sub

    'function untuk read file .ini
    Private Declare Unicode Function GetPrivateProfileString Lib "kernel32" _
    Alias "GetPrivateProfileStringW" (ByVal lpSection As String, ByVal lpParamName As String, _
    ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, _
    ByVal lpFilename As String) As Int32

    'function untuk read file .ini
    Public Function readini(ByVal iniFileName As String, ByVal Section As String, ByVal ParamName As String, ByVal ParamDefault As String) As String
        Dim ParamVal As String = Space$(1024)
        Dim LenParamVal As Long = GetPrivateProfileString(Section, ParamName, ParamDefault, ParamVal, Len(ParamVal), iniFileName)
        'mengembalikan nilai yang sudah didapatkan
        readini = Strings.Left(ParamVal, LenParamVal)
    End Function

    'Public Sub IUDQuery(ByRef Query As String)
    '    Try
    '        With cmd
    '            .CommandText = Query
    '            .CommandType = CommandType.Text
    '            .Connection = Cn
    '            .ExecuteNonQuery()
    '        End With
    '    Catch ex As Exception
    '        MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
    '        MsgBox("Tidak dapat terhubung dengan database, aplikasi akan ditutup", MsgBoxStyle.Exclamation, "Tidak terhubung dengan database")
    '        'End
    '    End Try
    'End Sub

    Public Function GetData(ByVal FileName As String) As List(Of String)
        Dim valueList As New List(Of String)
        Using cn As New OleDbConnection With
            {
                .ConnectionString = ConnectionString(FileName)
            }
            Using cmd As OleDbCommand = New OleDbCommand("SELECT bNumber FROM [Table_1$]", cn)
                cn.Open()
                Dim reader As OleDbDataReader = cmd.ExecuteReader
                While reader.Read
                    valueList.Add(reader.GetString(0))
                End While
            End Using
        End Using
        Return valueList
    End Function
    Public Function ConnectionString(ByVal FileName As String) As String
        Dim Builder As New OleDbConnectionStringBuilder
        If IO.Path.GetExtension(FileName).ToUpper = ".XLS" Then
            Builder.Provider = "Microsoft.Jet.OLEDB.4.0"
            Builder.Add("Extended Properties", "Excel 8.0;IMEX=1;HDR=Yes;")
        Else
            Builder.Provider = "Microsoft.ACE.OLEDB.12.0"
            Builder.Add("Extended Properties", "Excel 12.0;IMEX=1;HDR=Yes;")
        End If

        Builder.DataSource = FileName

        Return Builder.ConnectionString

    End Function

    Public Function TestNull(value As String, defaultValue As String) As String
        If String.IsNullOrEmpty(value) Then
            Return defaultValue
        Else
            Return value
        End If
    End Function

    Public Function Bool1(ByVal Item As Object) As Boolean
        Return If(Item IsNot Nothing AndAlso Not IsDBNull(Item), CBool(Item), False)
    End Function
End Class

