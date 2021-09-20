Imports VB = Microsoft.VisualBasic
Imports System.IO
Imports System.Security.Cryptography
Imports System.Data
'For time server
Imports System.Runtime.InteropServices

Public Class ModPelengkap

    'Function untuk membuat penomoran otomatis dengan format angka 6 digit
    Public Function Generate(ByVal Nama_Tabel As String, ByVal Nama_Field As String, ByVal nodoc As String, ByVal thnz As String) As String
        Tabel = Proses.ExecuteQuery("Select " & Nama_Field & " From " & Nama_Tabel & " where substring(no_doc,1,2)='" & nodoc & "' and substring(no_doc,-10,2)='" & thnz & "' order by " & Nama_Field & " desc")
        If Tabel.Rows.Count = 0 Then
            Generate = "00001"
        Else
            With Tabel.Rows(0)
                VarStr = Val(.Item(0)) + 1
                VarStr1 = VB.Len(VarStr)
                VarStr2 = "00000"
                VarStr3 = VarStr2.Substring(VarStr1)
                Generate = VarStr3 & Right(.Item("" & Nama_Field & ""), 5) + 1
            End With
        End If
    End Function

    Public Function GenerateMember(ByVal Nama_Tabel As String, ByVal Nama_Field As String, ByVal storeID As String, ByVal thnz4 As String) As String
        Tabel = Proses.ExecuteQuery("Select " & Nama_Field & " From " & Nama_Tabel & " where store_id='" & storeID & "' and year(creation_date)='" & thnz4 & "' order by " & Nama_Field & " desc")
        If Tabel.Rows.Count = 0 Then
            GenerateMember = "0001"
        Else
            With Tabel.Rows(0)
                VarStr = Val(.Item(0)) + 1
                VarStr1 = VB.Len(VarStr)
                VarStr2 = "0000"
                VarStr3 = VarStr2.Substring(VarStr1)
                GenerateMember = VarStr3 & Right(.Item("" & Nama_Field & ""), 4) + 1
            End With
        End If
    End Function

    'Function untuk cek keadaan kolom teks 
    Public Function Isi(ByRef Teks As Object) As Boolean
        If Teks.ToString = "" Then
            Isi = False
            MsgBox("Data tidak boleh kosong, Silahkan cek lagi!", vbExclamation, "Peringatan")
        Else
            Isi = True
        End If
    End Function

    Function TerbilangIndonesia(ByRef Indx As String) As String

        Dim satu(10) As String
        Dim dua(10) As String
        Dim tiga(10) As String
        Dim ratus As String
        Dim ribu As String
        Dim juta As String
        Dim Millyar As String
        Dim Trilliun As String

        satu(0) = "Nol" : satu(1) = "Satu" : satu(2) = "Dua" : satu(3) = "Tiga" : satu(4) = "Empat" : satu(5) = "Lima" : satu(6) = "Enam" : satu(7) = "Tujuh" : satu(8) = "Delapan" : satu(9) = "Sembilan"
        dua(0) = "Sepuluh" : dua(1) = "Sebelas" : dua(2) = "Dua belas" : dua(3) = "Tiga belas" : dua(4) = "Empat Belas" : dua(5) = "Lima Belas" : dua(6) = "Enam Belas" : dua(7) = "Tujuh belas" : dua(8) = "Delapan belas" : dua(9) = "Sembilan belas"
        tiga(2) = "Dua puluh" : tiga(3) = "Tiga puluh" : tiga(4) = "Empat puluh" : tiga(5) = "Lima puluh" : tiga(6) = "Enam Puluh" : tiga(7) = "Tujuh Puluh" : tiga(8) = "Delapan puluh" : tiga(9) = "Sembilan puluh"
        ratus = "ratus" : ribu = "ribu" : juta = "juta"
        Millyar = "millyar" : Trilliun = "trilliun"

        Dim inp, BhsNilai As String

        inp = CStr(Val(Indx))


        Select Case Len(inp)
            Case 1
                BhsNilai = satu(CInt(Indx))

            Case 2
                If Int(CDbl(VB.Right(inp, 1))) > 0 And CDbl(VB.Left(inp, 1)) > 1 Then BhsNilai = TerbilangIndonesia(CStr(Int(CDbl(VB.Right(inp, 1)))))
                If CDbl(VB.Left(inp, 1)) > 1 Then BhsNilai = tiga(CInt(VB.Left(inp, 1))) & BhsNilai
                If CDbl(VB.Left(inp, 1)) = 1 Then BhsNilai = dua(CInt(VB.Right(inp, 1)))
                BhsNilai = Replace(BhsNilai, "Satu ribu", "Seribu")

            Case 3
                BhsNilai = satu(10)
                If Int(CDbl(VB.Right(inp, 2))) > 0 Then BhsNilai = TerbilangIndonesia(CStr(Int(CDbl(VB.Right(inp, 2)))))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 1)))) & ratus & BhsNilai

            Case 4
                If Int(CDbl(VB.Right(inp, 3))) > 0 Then BhsNilai = TerbilangIndonesia(CStr(Int(CDbl(VB.Right(inp, 3)))))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 1)))) & ribu & BhsNilai
                BhsNilai = Replace(BhsNilai, "Satu ribu", "Seribu")

            Case 5
                If Int(CDbl(VB.Right(inp, 3))) > 0 Then BhsNilai = TerbilangIndonesia(CStr(Int(CDbl(VB.Right(inp, 3)))))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 2)))) & ribu & BhsNilai

            Case 6
                If CInt(VB.Right(inp, 3)) > 0 Then BhsNilai = TerbilangIndonesia(CStr(CInt(VB.Right(inp, 3))))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 3)))) & ribu & BhsNilai

            Case 7
                If CInt(VB.Right(inp, 6)) > 0 Then BhsNilai = TerbilangIndonesia(CStr(CInt(VB.Right(inp, 6))))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 1)))) & juta & BhsNilai

            Case 8
                If CInt(VB.Right(inp, 6)) > 0 Then BhsNilai = TerbilangIndonesia(CStr(CInt(VB.Right(inp, 6))))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 2)))) & juta & BhsNilai

            Case 9
                If CInt(VB.Right(inp, 6)) > 0 Then BhsNilai = TerbilangIndonesia(CStr(CInt(VB.Right(inp, 6))))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 3)))) & juta & BhsNilai

            Case 10
                If CInt(VB.Right(inp, 9)) > 0 Then BhsNilai = TerbilangIndonesia(CStr(CInt(VB.Right(inp, 9))))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 1)))) & Millyar & BhsNilai

            Case 11
                If CInt(VB.Right(inp, 9)) > 0 Then BhsNilai = TerbilangIndonesia(CStr(CInt(VB.Right(inp, 9))))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 2)))) & Millyar & BhsNilai

            Case 12
                If Val(VB.Right(inp, 9)) > 0 Then BhsNilai = TerbilangIndonesia(VB.Right(inp, 9))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 3)))) & Millyar & BhsNilai

            Case 13
                If Val(VB.Right(inp, 12)) > 0 Then BhsNilai = TerbilangIndonesia(VB.Right(inp, 12))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 1)))) & Trilliun & BhsNilai

            Case 14
                If Val(VB.Right(inp, 12)) > 0 Then BhsNilai = TerbilangIndonesia(VB.Right(inp, 12))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 2)))) & Trilliun & BhsNilai

            Case 15
                If Val(VB.Right(inp, 12)) > 0 Then BhsNilai = TerbilangIndonesia(VB.Right(inp, 12))
                BhsNilai = TerbilangIndonesia(Int(CDbl(VB.Left(inp, 3)))) & Trilliun & BhsNilai
        End Select

        BhsNilai = Replace(BhsNilai, "Satu ratus", "Seratus")
        BhsNilai = Trim(BhsNilai)

        TerbilangIndonesia = " " & BhsNilai & " "
        TerbilangIndonesia = Replace(TerbilangIndonesia, " ", " ")

    End Function

    Public Function GenerateKodeMaster(ByVal Nama_Tabel As String, ByVal nama_id As String, ByVal nama_toko As String, ByVal tahun As String) As String

        SQL = ""
        SQL = "Select max(" & nama_id & ") as kode From " & Nama_Tabel & " where store_id='" & nama_toko & "' and year(creation_date)='" & tahun & "'"

        Tabel = Proses.ExecuteQuery(SQL)

        If IsDBNull(Tabel.Rows(0).Item("kode")) Then
            GenerateKodeMaster = "0001"
        Else
            VarStr = "0000"
            VarStr = VarStr + Right(Tabel.Rows(0).Item("kode"), 4) + 1
            GenerateKodeMaster = Trim(Right("0000" & Trim(Str(VarStr)), 4))
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

    Function BuatKomaSatu(ByVal desc As String) As String
        If Left(desc, 1) = "'" And Right(desc, 1) = "'" Then
            Dim Panjang As Integer
            Panjang = Len(desc) - 2
            BuatKomaSatu = "'" & Replace(Mid(desc, 2, Panjang), "'", "''") & "'"
        Else
            BuatKomaSatu = Replace(desc, "'", "''")
        End If
    End Function

    Public Sub KosongkanIsiText(ByVal x As Form)
        For Each i As Control In x.Controls
            If TypeOf (i) Is TextBox Then
                i.Text = ""
                i.Focus()
            End If
        Next
    End Sub

    'Mengosongkan isi teks combobox pada form seluruh
    Public Sub KosongkanIsiCombo(ByVal x As Form)
        For Each i As Control In x.Controls
            If TypeOf (i) Is ComboBox Then
                i.Text = ""
            End If
        Next
    End Sub

    Public Function Bool1(ByVal Item As Object) As Boolean
        Return If(Item IsNot Nothing AndAlso Not IsDBNull(Item), CBool(Item), False)
    End Function

    Public Sub GenerateAkses(ByVal Nama_Menu As String)

        SQL = ""
        SQL = " select a.nama,a.deskripsi,b.access_flag,b.access_flag,b.add_flag,b.edit_flag,b.validasi_flag,b.delete_flag,b.print_flag,"
        SQL = SQL & " a.id, b.role_code, c.role_name, b.menu_id, a.no_urut"
        SQL = SQL & " from m_app_menu_roles a left join m_app_menu_action b on a.id=b.menu_id"
        SQL = SQL & " left join m_app_roles c on b.role_code=c.role_code"
        SQL = SQL & " where b.role_code = '" & xyroleid & "' and a.deskripsi='" & Nama_Menu & "'"
        SQL = SQL & " order by a.no_urut"

        Tabel = Proses.ExecuteQuery(SQL)

        If Tabel.Rows.Count <> 0 Then

            shaddflag = Tabel.Rows(0).Item("add_flag")
            sheditflag = Tabel.Rows(0).Item("edit_flag")
            shconfirmflag = Tabel.Rows(0).Item("validasi_flag")
            shdeleteflag = Tabel.Rows(0).Item("delete_flag")
            shprintflag = Tabel.Rows(0).Item("print_flag")

        End If

    End Sub


End Class



