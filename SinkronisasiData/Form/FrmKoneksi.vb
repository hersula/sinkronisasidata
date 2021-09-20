Public Class FrmKoneksi
    Dim encServerName As String
    Dim encLogin As String
    Dim encPass As String
    Dim encData As String
    Dim encPort As String

    Dim decServerName As String
    Dim decLogin As String
    Dim decPass As String
    Dim decData As String
    Dim decPort As String
    Dim FilePath As String = Application.StartupPath & "\setting.ini"
    Dim FileName As String = System.IO.Path.GetFileName(FilePath)

    Sub ShowLokal()

        If System.IO.File.Exists(FileName) Then
            decServerName = Konek.readini(FilePath, "Koneksi Local", "ServerNameLoc", "")
            decLogin = Konek.readini(FilePath, "Koneksi Local", "LoginLoc", "")
            decPass = Konek.readini(FilePath, "Koneksi Local", "PasswordLoc", "")
            decData = Konek.readini(FilePath, "Koneksi Local", "DatabaseLoc", "")

            txtServerNameLoc.Text = Konek.DecryptText(decServerName)
            txtLoginLoc.Text = Konek.DecryptText(decLogin)
            txtPasswordLoc.Text = Konek.DecryptText(decPass)
            txtDatabaseLoc.Text = Konek.DecryptText(decData)

        Else

            txtServerNameLoc.Text = ""
            txtLoginLoc.Text = ""
            txtPasswordLoc.Text = ""
            txtDatabaseLoc.Text = ""

        End If
    End Sub

    Sub ShowHO()

        If System.IO.File.Exists(FileName) Then
            decServerName = Konek.readini(FilePath, "Koneksi HO", "ServerNameHO", "")
            decLogin = Konek.readini(FilePath, "Koneksi HO", "LoginHO", "")
            decPass = Konek.readini(FilePath, "Koneksi HO", "PasswordHO", "")
            decData = Konek.readini(FilePath, "Koneksi HO", "DatabaseHO", "")

            txtServerNameHO.Text = Konek.DecryptText(decServerName)
            txtLoginHO.Text = Konek.DecryptText(decLogin)
            txtPasswordHO.Text = Konek.DecryptText(decPass)
            txtDatabaseHO.Text = Konek.DecryptText(decData)

        Else

            txtServerNameHO.Text = ""
            txtLoginHO.Text = ""
            txtPasswordHO.Text = ""
            txtDatabaseHO.Text = ""

        End If
    End Sub

    Private Sub btnSimpan_Click(sender As System.Object, e As System.EventArgs) Handles btnSimpan.Click
        Dim FilePath As String = Application.StartupPath & "\setting.ini"

        '=== Lokal ===
        encServerName = Konek.EncryptText(txtServerNameLoc.Text)
        encLogin = Konek.EncryptText(txtLoginLoc.Text)
        encPass = Konek.EncryptText(txtPasswordLoc.Text)
        encData = Konek.EncryptText(txtDatabaseLoc.Text)

        Konek.writeini(FilePath, "Koneksi Local", "ServerNameLoc", encServerName)
        Konek.writeini(FilePath, "Koneksi Local", "LoginLoc", encLogin)
        Konek.writeini(FilePath, "Koneksi Local", "PasswordLoc", encPass)
        Konek.writeini(FilePath, "Koneksi Local", "DatabaseLoc", encData)


        '=== HO ===

        encServerName = Konek.EncryptText(txtServerNameHO.Text)
        encLogin = Konek.EncryptText(txtLoginHO.Text)
        encPass = Konek.EncryptText(txtPasswordHO.Text)
        encData = Konek.EncryptText(txtDatabaseHO.Text)

        Konek.writeini(FilePath, "Koneksi HO", "ServerNameHO", encServerName)
        Konek.writeini(FilePath, "Koneksi HO", "LoginHO", encLogin)
        Konek.writeini(FilePath, "Koneksi HO", "PasswordHO", encPass)
        Konek.writeini(FilePath, "Koneksi HO", "DatabaseHO", encData)

        MsgBox("Konfigurasi berhasil di simpan.", MsgBoxStyle.Information, "Informasi")
        End
    End Sub

    Private Sub FrmKoneksi_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ShowLokal()
        ShowHO()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

    End Sub
End Class