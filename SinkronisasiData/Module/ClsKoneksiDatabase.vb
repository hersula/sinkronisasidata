Imports System.Data
Imports System.Data.Sql
Imports MySql.Data.MySqlClient

Public Class ClsKoneksiDatabase
    'Protected SQL As String
    Protected CnHO As MySqlConnection
    Protected CmdHO As MySqlCommand
    Protected DaHO As MySqlDataAdapter
    Protected DsHO As DataSet
    Protected DtHO As DataTable

    Protected CnLoc As MySqlConnection
    Protected CmdLoc As MySqlCommand
    Protected DaLoc As MySqlDataAdapter
    Protected DsLoc As DataSet
    Protected DtLoc As DataTable

    Public Function OpenConnHO() As Boolean
        Dim FilePath As String = Application.StartupPath & "\setting.ini"
        Dim FileName As String = System.IO.Path.GetFileName(FilePath)
        Dim decServerName As String
        Dim decLogin As String
        Dim decPass As String
        Dim decData As String

        If System.IO.File.Exists(FileName) Then
            Try

                decServerName = Konek.readini(FilePath, "Koneksi HO", "ServerNameHO", "")
                decLogin = Konek.readini(FilePath, "Koneksi HO", "LoginHO", "")
                decPass = Konek.readini(FilePath, "Koneksi HO", "PasswordHO", "")
                decData = Konek.readini(FilePath, "Koneksi HO", "DatabaseHO", "")

                CnHO = New MySqlConnection( _
                    "server='" & Konek.DecryptText(decServerName) & "'; user id='" & Konek.DecryptText(decLogin) & "'; password='" & Konek.DecryptText(decPass) & "'; database='" & Konek.DecryptText(decData) & "'")

                CnHO.Open()
                If CnHO.State <> ConnectionState.Open Then
                    Return False
                Else
                    Return True
                End If
            Catch
                Return False
            End Try
        End If
    End Function

    Public Sub CloseConnHO()
        If Not IsNothing(CnHO) Then
            CnHO.Close()
            CnHO = Nothing
        End If
    End Sub

    Public Function ExecuteQuery(ByVal Query As String) As DataTable
        If Not OpenConnHO() Then
            frmUtama.DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Server HO Disconnected..")
            Return Nothing
            Exit Function
        End If

        Try
            CmdHO = New MySqlCommand(Query, CnHO)
            DaHO = New MySqlDataAdapter
            DaHO.SelectCommand = CmdHO

            DsHO = New Data.DataSet
            DaHO.Fill(DsHO)
            DtHO = DsHO.Tables(0)
            CekServer = True
            Return DtHO

            DtHO = Nothing
            DsHO = Nothing
            DaHO = Nothing
            CmdHO = Nothing

            CloseConnHO()

        Catch ex As Exception
            CekServer = False
            Return DtHO

            DtHO = Nothing
            DsHO = Nothing
            DaHO = Nothing
            CmdHO = Nothing
        End Try

    End Function

    Public Sub ExecuteNonQuery(ByVal Query As String)

        If Not OpenConnHO() Then
            frmUtama.DG.Rows.Add(Format(Now, "HH:mm:ss"), "Koneksi SERVER HO Gagal..")
            Exit Sub
        End If

        Dim Transaction As MySqlTransaction
        Transaction = CnHO.BeginTransaction


        CmdHO = New MySqlCommand
        CmdHO.Connection = CnHO
        CmdHO.Transaction = Transaction

        Try
            CmdHO.CommandType = CommandType.Text
            CmdHO.CommandText = Query
            CmdHO.ExecuteNonQuery()
            CmdHO = Nothing
            Transaction.Commit()
            CloseConnHO()
            Success = True

        Catch ex As Exception
            frmUtama.Size = New Size(1022, 408)
            frmUtama.TextBox1.Text = Query
            frmUtama.Timer1.Enabled = False
            MsgBox(ex.ToString)
            frmUtama.DG.Rows.Add(Format(Now, "HH:mm:ss"), Query)
            Transaction.Rollback()
            CloseConnHO()
            Success = False
        End Try

    End Sub

    Public Function OpenConnLoc() As Boolean
        Dim FilePath As String = Application.StartupPath & "\setting.ini"
        Dim FileName As String = System.IO.Path.GetFileName(FilePath)
        Dim decServerNameLoc As String
        Dim decLoginLoc As String
        Dim decPassLoc As String
        Dim decDataLoc As String

        If System.IO.File.Exists(FileName) Then
            Try

                decServerNameLoc = Konek.readini(FilePath, "Koneksi Local", "ServerNameLoc", "")

                If decServerNameLoc = "" Then
                    MsgBox(" Server Belum Disetting Dengan Benar", vbInformation)
                    FrmKoneksi.ShowDialog()
                End If

                decLoginLoc = Konek.readini(FilePath, "Koneksi Local", "LoginLoc", "")
                decPassLoc = Konek.readini(FilePath, "Koneksi Local", "PasswordLoc", "")
                decDataLoc = Konek.readini(FilePath, "Koneksi Local", "DatabaseLoc", "")


                CnLoc = New MySqlConnection( _
                   "server='" & Konek.DecryptText(decServerNameLoc) & "'; user id='" & Konek.DecryptText(decLoginLoc) & "'; password='" & Konek.DecryptText(decPassLoc) & "'; database='" & Konek.DecryptText(decDataLoc) & "'")

                CnLoc.Open()

                If CnLoc.State <> ConnectionState.Open Then
                    Return False
                Else
                    Return True
                End If
            Catch
                Return False
                'FrmKoneksi.Show()
            End Try
        End If
    End Function

    Public Sub CloseConnLoc()
        If Not IsNothing(CnLoc) Then
            CnLoc.Close()
            CnLoc = Nothing
        End If
    End Sub

    Public Function ExecuteQueryLoc(ByVal Query As String) As DataTable

        If Not OpenConnLoc() Then
            frmUtama.DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Return Nothing
            Exit Function
        End If

        Try
            CmdLoc = New MySqlCommand(Query, CnLoc)
            DaLoc = New MySqlDataAdapter
            DaLoc.SelectCommand = CmdLoc

            DsLoc = New Data.DataSet
            DaLoc.Fill(DsLoc)
            DtLoc = DsLoc.Tables(0)
            CekLokal = True
            Return DtLoc

            DtLoc = Nothing
            DsLoc = Nothing
            DaLoc = Nothing
            CmdLoc = Nothing

            CloseConnLoc()

        Catch ex As Exception
            CekLokal = False
            Return DtLoc

            DtLoc = Nothing
            DsLoc = Nothing
            DaLoc = Nothing
            CmdLoc = Nothing
        End Try

    End Function

    Public Sub ExecuteNonQueryLoc(ByVal Query As String)
        If Not OpenConnLoc() Then
            frmUtama.DG.Rows.Add(Format(Now, "HH:mm:ss"), "Koneksi Lokal Gagal..")
            Exit Sub
        End If

        Dim Transaction As MySqlTransaction
        Transaction = CnLoc.BeginTransaction


        CmdLoc = New MySqlCommand
        CmdLoc.Connection = CnLoc
        CmdLoc.Transaction = Transaction

        Try
            CmdLoc.CommandType = CommandType.Text
            CmdLoc.CommandText = Query
            CmdLoc.ExecuteNonQuery()
            CmdLoc = Nothing
            Transaction.Commit()
            CloseConnLoc()
            Success = True

        Catch ex As Exception
            frmUtama.Size = New Size(1022, 408)
            frmUtama.TextBox1.Text = Query
            frmUtama.Timer1.Enabled = False
            MsgBox(ex.ToString)
            frmUtama.DG.Rows.Add(Format(Now, "HH:mm:ss"), Query)
            Transaction.Rollback()
            CloseConnLoc()
            Success = False
        End Try
    End Sub
End Class
