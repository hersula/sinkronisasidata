Imports VB = Microsoft.VisualBasic
Imports System.IO
Imports System.Security.Cryptography
Imports System.Data
Imports System.Data.SQL
Imports System.Data.SqlClient

Public Class ClsGlobal
    Public SQL As String
    Public Tabel As DataTable

    Public Cmd As SqlClient.SqlCommand
    Public Da As SqlClient.SqlDataAdapter
    Public Ds As DataSet
    Public dr As SqlClient.SqlDataReader

    Public konek As New ModPelengkap

    'txt_encrypt.text = EncryptText(txt_decrypt.text)
    Public Function EncryptText(ByVal SourceText As System.String) As System.String
        Dim MyKey As String = "Powered by Sule-Soft"
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
        Dim mykey As String = "Powered by Sule-Soft"
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
End Class
