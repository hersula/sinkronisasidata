Public Class frmUtama
    Dim Jam, Menit, Detik, Mundur As Integer
    Dim xyLastUpdate
    Dim xynow

    Private Sub frmUtama_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        'Timer1.Enabled = True
    End Sub

    Private Sub frmUtama_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        NotifyIcon1.Dispose()
    End Sub

    Private Sub frmUtama_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        xynow = Format(Now, "yyyy-MM-dd 23:59:59")
        MyStore()
        CheckLastUpdate()
        Me.Size = New Size(612, 408)
        checkServer()
        Mundur = 0
        Timer1.Enabled = True
        btnProcess.Enabled = True
        btnProcess.PerformClick()
        NotifyIcon1.Visible = False
    End Sub

    Sub CheckLastUpdate()
        SQL = ""
        SQL = "select * from m_update_data;"
        Tabel = Proses.ExecuteQueryLoc(SQL)
        If Tabel.Rows.Count = 0 Then Exit Sub
        xyLastUpdate = Format(Tabel.Rows(0).Item("last_update"), "yyyy-MM-dd 00:00:00")
    End Sub

    Sub checkServer()

        '-- Cek Internet --
        Try
            If My.Computer.Network.Ping("8.8.8.8") Then
                lblInternetStatus.Text = "Status Internet : online"
                lblInternetStatus.ForeColor = Color.Blue
            Else
                lblInternetStatus.Text = "Status Internet : offline"
                lblInternetStatus.ForeColor = Color.Red
            End If

        Catch ex As Exception
            lblInternetStatus.Text = "Status Internet : offline"
            lblInternetStatus.ForeColor = Color.Red
            MsgBox("Ada masalah koneksi Internet, silahkan coba lagi beberapa menit kemudian" & vbNewLine & "ada kesalahan :" & ex.ToString(), vbExclamation, "Nabawi Monitor")
        End Try

        '-- Cek Database Server HO --
        Try
            If My.Computer.Network.Ping("51.79.150.45") Then
                lblServerStatus.Text = "Status Server : online"
                lblServerStatus.ForeColor = Color.Purple
            Else
                lblServerStatus.Text = "Status Server : offline"
                lblServerStatus.ForeColor = Color.Red
            End If


        Catch ex As Exception
            lblServerStatus.Text = "Status Server : offline"
            lblServerStatus.ForeColor = Color.Red
            MsgBox("Ada masalah dengan koneksi ke server HO, silahkan coba lagi beberapa menit kemudian" & vbNewLine & "ada kesalahan :" & ex.ToString(), vbExclamation, "Nabawi Monitor")
        Finally
            lblservercheck.Text = "Last Check:" & Now().ToString("dd-MM-yyyy hh:mm")
        End Try
    End Sub

    Private Sub btnSetting_Click(sender As System.Object, e As System.EventArgs) Handles btnSetting.Click
        FrmKoneksi.ShowDialog()
    End Sub

    Private Sub SyncSales()
        SqlGlobal = ""
        SQL = "select no_doc from t_sales_hdr where status_kirim = 0;"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub
        Dim xyNoDoc As String
        Dim TabelData As New DataTable

        PB.Visible = True
        PB.Value = 1
        PB.Maximum = TabelLoc.Rows.Count - 1

        For i As Integer = 0 To TabelLoc.Rows.Count - 1
            SqlGlobal = ""
            xyNoDoc = TabelLoc.Rows(i).Item("no_doc")
            SQL = "select * from t_sales_hdr where no_doc = '" & xyNoDoc & "' order by no_doc;"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If

            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_sales_hdr Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1

                    svexpdate = TabelData.Rows(n).Item("tgl_jatuh_tempo")
                    If IsDBNull(svexpdate) Then
                        svexpdate2 = "1900-01-01 00:00:00"
                    Else
                        svexpdate2 = CDate(TabelData.Rows(n).Item("tgl_jatuh_tempo")).ToString("yyyy-MM-dd")
                    End If

                    svcanceldate = TabelData.Rows(n).Item("cancelation_date")
                    If IsDBNull(svcanceldate) Then
                        svcanceldate2 = "1900-01-01 00:00:00"
                    Else
                        svcanceldate2 = CDate(TabelData.Rows(n).Item("cancelation_date")).ToString("yyyy-MM-dd HH:mm:ss")
                    End If


                    svtglbayar = TabelData.Rows(n).Item("tgl_bayar")
                    If IsDBNull(svtglbayar) Then
                        svtglbayar2 = "1900-01-01 00:00:00"
                    Else
                        svtglbayar2 = CDate(TabelData.Rows(n).Item("tgl_bayar")).ToString("yyyy-MM-dd HH:mm:ss")
                    End If


                    SQL = "Insert Into t_sales_hdr(store_id,no_doc,tgl_trans,kasir,type_payment,cust_id,cust_price_sub_level,employee_id,total_qty,total_gross,total_price_point,total_potongan,ppn,ppn_pcn,"
                    SQL = SQL & " ppn_value,ongkir,grand_total,cash,credit,debit,transfer,digital_wallet,kembali,credit_no,debit_no,"
                    SQL = SQL & " digital_name,status_payment,tgl_jatuh_tempo,tgl_bayar,coa_credit,coa_debit,"
                    SQL = SQL & " created_by,creation_date,modified_by,modification_date,canceled_by,cancelation_date) "
                    SQL = SQL & " Values('" & TabelData.Rows(n).Item("store_id") & "','" & TabelData.Rows(n).Item("no_doc") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("tgl_trans"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("kasir") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("type_payment") & "','" & TabelData.Rows(n).Item("cust_id") & "'," & TabelData.Rows(n).Item("cust_price_sub_level") & ",'" & TabelData.Rows(n).Item("employee_id") & "',"
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("total_qty")) & "," & CDbl(TabelData.Rows(n).Item("total_gross")) & "," & CDbl(TabelData.Rows(n).Item("total_price_point")) & "," & CDbl(TabelData.Rows(n).Item("total_potongan")) & ","
                    SQL = SQL & " '" & TabelData.Rows(n).Item("ppn") & "'," & CDbl(TabelData.Rows(n).Item("ppn_pcn")) & "," & CDbl(TabelData.Rows(n).Item("ppn_value")) & ","
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("ongkir")) & "," & CDbl(TabelData.Rows(n).Item("grand_total")) & "," & CDbl(TabelData.Rows(n).Item("cash")) & "," & CDbl(TabelData.Rows(n).Item("credit")) & ","
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("debit")) & "," & CDbl(TabelData.Rows(n).Item("transfer")) & "," & CDbl(TabelData.Rows(n).Item("digital_wallet")) & "," & CDbl(TabelData.Rows(n).Item("kembali")) & ",'" & TabelData.Rows(n).Item("credit_no").ToString & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("debit_no").ToString & "','" & TabelData.Rows(n).Item("digital_name") & "'," & TabelData.Rows(n).Item("status_payment") & ",'" & svexpdate2 & "','" & svtglbayar2 & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("coa_credit") & "','" & TabelData.Rows(n).Item("coa_debit") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("created_by") & "','" & Format(TabelData.Rows(n).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("modified_by") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("canceled_by") & "','" & svcanceldate2 & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If


            SQL = "Select * From t_sales_dtl Where no_doc = '" & xyNoDoc & "';"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If
            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_sales_dtl Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_sales_dtl (no_doc,item_id,expired_date,qty,harga,potongan,jumlah,row_no)"
                    SQL = SQL & " Values('" & xyNoDoc & "','" & TabelData.Rows(n).Item("item_id") & "','" & Format(TabelData.Rows(n).Item("expired_date"), "yyyy-MM-dd HH:mm:ss") & "'," & CDbl(TabelData.Rows(n).Item("qty")) & ","
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("harga")) & "," & CDbl(TabelData.Rows(n).Item("potongan")) & "," & CDbl(TabelData.Rows(n).Item("jumlah")) & ",'" & TabelData.Rows(n).Item("row_no").ToString & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If

            Proses.ExecuteNonQuery(SqlGlobal)

            If Success = True Then
                'Update Lokal
                SQL = "update t_sales_hdr set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                SQL = "update t_sales_dtl set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                Proses.ExecuteNonQueryLoc(SqlGlobalLoc)
            End If
            PB.Value = i
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Sales  " & xyNoDoc)
        Next
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT SALES SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT SALES GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")
    End Sub

    Private Sub SyncSR()
        SqlGlobal = ""
        SQL = "select no_doc from t_sr_hdr where status='APPROVED' and status_kirim = 0;"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub
        Dim xyNoDoc As String
        Dim TabelData As New DataTable

        PB.Visible = True
        PB.Value = 1
        PB.Maximum = TabelLoc.Rows.Count - 1

        For i As Integer = 0 To TabelLoc.Rows.Count - 1
            SqlGlobal = ""
            xyNoDoc = TabelLoc.Rows(i).Item("no_doc")
            SQL = "select * from t_sr_hdr where no_doc = '" & xyNoDoc & "' order by no_doc;"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If

            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_sr_hdr Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_sr_hdr(store_id,no_doc,tgl_trans,tot_qty,keterangan,status,created_by,creation_date,modified_by,modification_date) "
                    SQL = SQL & " Values('" & TabelData.Rows(n).Item("store_id") & "','" & TabelData.Rows(n).Item("no_doc") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("tgl_trans"), "yyyy-MM-dd HH:mm:ss") & "'," & CDbl(TabelData.Rows(n).Item("tot_qty")) & ","
                    SQL = SQL & " '" & TabelData.Rows(n).Item("keterangan") & "','" & TabelData.Rows(n).Item("status") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("created_by") & "','" & Format(TabelData.Rows(n).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("modified_by") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If


            SQL = "Select * From t_sr_dtl Where no_doc = '" & xyNoDoc & "';"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If
            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_sr_dtl Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_sr_dtl (no_doc,item_id,uom_id,qty)"
                    SQL = SQL & " Values('" & xyNoDoc & "','" & TabelData.Rows(n).Item("item_id") & "','" & TabelData.Rows(n).Item("uom_id") & "'," & CDbl(TabelData.Rows(n).Item("qty")) & ");"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If

            Proses.ExecuteNonQuery(SqlGlobal)

            If Success = True Then
                'Update Lokal
                SQL = "update t_sr_hdr set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                SQL = "update t_sr_dtl set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                Proses.ExecuteNonQueryLoc(SqlGlobalLoc)
            End If
            PB.Value = i
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Store Request  " & xyNoDoc)
        Next
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT STORE REQUEST SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT STORE REQUEST GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")
    End Sub

    Private Sub SyncReceive()
        SqlGlobal = ""
        SQL = "select no_doc from t_receive_hdr where status='APPROVED' and status_kirim = 0;"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub
        Dim xyNoDoc As String
        Dim TabelData As New DataTable

        PB.Visible = True
        PB.Value = 1
        PB.Maximum = TabelLoc.Rows.Count - 1

        For i As Integer = 0 To TabelLoc.Rows.Count - 1
            SqlGlobal = ""
            xyNoDoc = TabelLoc.Rows(i).Item("no_doc")
            SQL = "select * from t_receive_hdr where no_doc = '" & xyNoDoc & "' order by no_doc;"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If

            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_receive_hdr Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_receive_hdr(store_id,no_doc,no_doc_ref,tgl_trans,tot_qty_kirim,tot_qty_terima,status,keterangan,created_by,creation_date,modified_by,modification_date) "
                    SQL = SQL & " Values('" & TabelData.Rows(n).Item("store_id") & "','" & TabelData.Rows(n).Item("no_doc") & "','" & TabelData.Rows(n).Item("no_doc_ref") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("tgl_trans"), "yyyy-MM-dd HH:mm:ss") & "'," & CDbl(TabelData.Rows(n).Item("tot_qty_kirim")) & "," & CDbl(TabelData.Rows(n).Item("tot_qty_terima")) & ","
                    SQL = SQL & " '" & TabelData.Rows(n).Item("status") & "','" & TabelData.Rows(n).Item("keterangan") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("created_by") & "','" & Format(TabelData.Rows(n).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("modified_by") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If


            SQL = "Select * From t_receive_dtl Where no_doc = '" & xyNoDoc & "';"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If
            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_receive_dtl Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_receive_dtl (no_doc,item_id,uom_id,qty_kirim,qty_terima,expired_date)"
                    SQL = SQL & " Values('" & xyNoDoc & "','" & TabelData.Rows(n).Item("item_id") & "','" & TabelData.Rows(n).Item("uom_id") & "'," & CDbl(TabelData.Rows(n).Item("qty_kirim")) & ","
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("qty_terima")) & ",'" & Format(TabelData.Rows(n).Item("expired_date"), "yyyy-MM-dd") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If

            Proses.ExecuteNonQuery(SqlGlobal)

            If Success = True Then
                'Update Lokal
                SQL = "update t_receive_hdr set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                SQL = "update t_receive_dtl set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                Proses.ExecuteNonQueryLoc(SqlGlobalLoc)
            End If
            PB.Value = i
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Good Receive  " & xyNoDoc)
        Next
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT GOOD RECEIVE SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT GOOD RECEIVE GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")
    End Sub

    Private Sub SyncTransferOut()
        SqlGlobal = ""
        SQL = "select no_doc from t_tatout_hdr where status='APPROVED' and status_kirim = 0;"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub
        Dim xyNoDoc As String
        Dim TabelData As New DataTable

        PB.Visible = True
        PB.Value = 1
        PB.Maximum = TabelLoc.Rows.Count - 1

        For i As Integer = 0 To TabelLoc.Rows.Count - 1
            SqlGlobal = ""
            xyNoDoc = TabelLoc.Rows(i).Item("no_doc")
            SQL = "select * from t_tatout_hdr where no_doc = '" & xyNoDoc & "' order by no_doc;"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If

            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_tatout_hdr Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_tatout_hdr(no_doc,tgl_trans,store_id,store_to,tot_qty,keterangan,status_sj,status,created_by,creation_date,modified_by,modification_date) "
                    SQL = SQL & " Values('" & TabelData.Rows(n).Item("no_doc") & "','" & Format(TabelData.Rows(n).Item("tgl_trans"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("store_id") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("store_to") & "'," & CDbl(TabelData.Rows(n).Item("tot_qty")) & ",'" & TabelData.Rows(n).Item("keterangan") & "',"
                    SQL = SQL & " " & TabelData.Rows(n).Item("status_sj") & ",'" & TabelData.Rows(n).Item("status") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("created_by") & "','" & Format(TabelData.Rows(n).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("modified_by") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If


            SQL = "Select * From t_tatout_dtl Where no_doc = '" & xyNoDoc & "';"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If
            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_tatout_dtl Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_tatout_dtl (no_doc,item_id,uom_id,qty,expired_date)"
                    SQL = SQL & " Values('" & xyNoDoc & "','" & TabelData.Rows(n).Item("item_id") & "','" & TabelData.Rows(n).Item("uom_id") & "'," & CDbl(TabelData.Rows(n).Item("qty")) & ","
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("expired_date"), "yyyy-MM-dd") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If

            Proses.ExecuteNonQuery(SqlGlobal)

            If Success = True Then
                'Update Lokal
                SQL = "update t_tatout_hdr set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                SQL = "update t_tatout_dtl set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                Proses.ExecuteNonQueryLoc(SqlGlobalLoc)
            End If
            PB.Value = i
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Transfer Out  " & xyNoDoc)
        Next
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT TRANSFER OUT SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT TRANSFER OUT GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")
    End Sub

    Private Sub SyncTransferIn()
        SqlGlobal = ""
        SQL = "select no_doc from t_tatin_hdr where status='APPROVED' and status_kirim = 0;"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub
        Dim xyNoDoc As String
        Dim TabelData As New DataTable

        PB.Visible = True
        PB.Value = 1
        PB.Maximum = TabelLoc.Rows.Count - 1

        For i As Integer = 0 To TabelLoc.Rows.Count - 1
            SqlGlobal = ""
            xyNoDoc = TabelLoc.Rows(i).Item("no_doc")
            SQL = "select * from t_tatin_hdr where no_doc = '" & xyNoDoc & "' order by no_doc;"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If

            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_tatin_hdr Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_tatin_hdr(no_doc,no_doc_ref,tgl_trans,store_id,store_from,tot_qty_kirim,tot_qty_terima,status,keterangan,created_by,creation_date,modified_by,modification_date) "
                    SQL = SQL & " Values('" & TabelData.Rows(n).Item("no_doc") & "','" & TabelData.Rows(n).Item("no_doc_ref") & "','" & Format(TabelData.Rows(n).Item("tgl_trans"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("store_id") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("store_from") & "'," & CDbl(TabelData.Rows(n).Item("tot_qty_kirim")) & "," & CDbl(TabelData.Rows(n).Item("tot_qty_terima")) & ","
                    SQL = SQL & " '" & TabelData.Rows(n).Item("status") & "','" & TabelData.Rows(n).Item("keterangan") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("created_by") & "','" & Format(TabelData.Rows(n).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("modified_by") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If


            SQL = "Select * From t_tatin_dtl Where no_doc = '" & xyNoDoc & "';"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If
            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_tatin_dtl Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_tatin_dtl (no_doc,item_id,uom_id,qty_kirim,qty_terima,,expired_date)"
                    SQL = SQL & " Values('" & xyNoDoc & "','" & TabelData.Rows(n).Item("item_id") & "','" & TabelData.Rows(n).Item("uom_id") & "'," & CDbl(TabelData.Rows(n).Item("qty_kirim")) & ","
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("qty_terima")) & ",'" & Format(TabelData.Rows(n).Item("expired_date"), "yyyy-MM-dd") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If

            Proses.ExecuteNonQuery(SqlGlobal)

            If Success = True Then
                'Update Lokal
                SQL = "update t_tatin_hdr set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                SQL = "update t_tatin_dtl set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                Proses.ExecuteNonQueryLoc(SqlGlobalLoc)
            End If
            PB.Value = i
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Transfer In  " & xyNoDoc)
        Next
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT TRANSFER IN SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT TRANSFER IN GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")
    End Sub

    Private Sub SyncSO()
        SqlGlobal = ""
        SQL = "select no_doc from t_so_hdr where status=1 and status_kirim = 0;"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub
        Dim xyNoDoc As String
        Dim TabelData As New DataTable

        'PB.Visible = True
        'PB.Value = 1
        'PB.Maximum = TabelLoc.Rows.Count - 1

        For i As Integer = 0 To TabelLoc.Rows.Count - 1
            SqlGlobal = ""
            xyNoDoc = TabelLoc.Rows(i).Item("no_doc")
            SQL = "select * from t_so_hdr where no_doc = '" & xyNoDoc & "' order by no_doc;"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If

            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_so_hdr Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_so_hdr(store_id,no_doc,tgl_trans,tot_qty_buku,tot_qty_fisik,keterangan,status,created_by,creation_date,modified_by,modification_date) "
                    SQL = SQL & " Values('" & TabelData.Rows(n).Item("store_id") & "','" & TabelData.Rows(n).Item("no_doc") & "','" & Format(TabelData.Rows(n).Item("tgl_trans"), "yyyy-MM-dd HH:mm:ss") & "',"
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("tot_qty_buku")) & "," & CDbl(TabelData.Rows(n).Item("tot_qty_fisik")) & ","
                    SQL = SQL & " '" & TabelData.Rows(n).Item("keterangan") & "'," & TabelData.Rows(n).Item("status") & ","
                    SQL = SQL & " '" & TabelData.Rows(n).Item("created_by") & "','" & Format(TabelData.Rows(n).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("modified_by") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If


            SQL = "Select * From t_so_dtl Where no_doc = '" & xyNoDoc & "';"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If
            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_so_dtl Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_so_dtl (no_doc,item_id,uom_id,qty_buku,qty_fisik,expired_date)"
                    SQL = SQL & " Values('" & xyNoDoc & "','" & TabelData.Rows(n).Item("item_id") & "','" & TabelData.Rows(n).Item("uom_id") & "'," & CDbl(TabelData.Rows(n).Item("qty_buku")) & ","
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("qty_fisik")) & ",'" & Format(TabelData.Rows(n).Item("expired_date"), "yyyy-MM-dd") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If

            Proses.ExecuteNonQuery(SqlGlobal)

            If Success = True Then
                'Update Lokal
                SQL = "update t_so_hdr set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                SQL = "update t_so_dtl set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                Proses.ExecuteNonQueryLoc(SqlGlobalLoc)
            End If
            PB.Value = i
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Stock Opname  " & xyNoDoc)
        Next
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT DATA STOCK OPNAME.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT DATA STOCK OPNAME GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")
    End Sub

    Private Sub SyncReturSales()
        SqlGlobal = ""
        SQL = "select no_doc from t_retursales_hdr where status_kirim = 0 and status='APPROVED';"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub
        Dim xyNoDoc As String
        Dim TabelData As New DataTable

        'PB.Visible = True
        'PB.Value = 1
        'PB.Maximum = TabelLoc.Rows.Count - 1

        For i As Integer = 0 To TabelLoc.Rows.Count - 1
            SqlGlobal = ""
            xyNoDoc = TabelLoc.Rows(i).Item("no_doc")
            SQL = "select * from t_retursales_hdr where no_doc = '" & xyNoDoc & "' order by no_doc;"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If

            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_retursales_hdr Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_retursales_hdr(store_id,no_doc,tgl_trans,no_doc_reff,kasir,nama_kasir,cust_id,tot_items,sub_total,ppn,"
                    SQL = SQL & " total,keterangan,status,created_by,creation_date) "
                    SQL = SQL & " Values('" & TabelData.Rows(n).Item("store_id") & "','" & TabelData.Rows(n).Item("no_doc") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("tgl_trans"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("no_doc_reff") & "','" & TabelData.Rows(n).Item("kasir") & "','" & TabelData.Rows(n).Item("nama_kasir") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("cust_id") & "'," & CDbl(TabelData.Rows(n).Item("tot_items")) & "," & CDbl(TabelData.Rows(n).Item("sub_total")) & "," & CDbl(TabelData.Rows(n).Item("ppn")) & ","
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("total")) & ",'" & TabelData.Rows(n).Item("keterangan") & "','" & TabelData.Rows(n).Item("status") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("created_by") & "','" & Format(TabelData.Rows(n).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If


            SQL = "Select * From t_retursales_dtl Where no_doc = '" & xyNoDoc & "';"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If
            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_retursales_dtl Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_retursales_dtl (no_doc,item_id,hjual,qty,sub_total,expired_date)"
                    SQL = SQL & " Values('" & xyNoDoc & "','" & TabelData.Rows(n).Item("item_id") & "'," & CDbl(TabelData.Rows(n).Item("hjual")) & ","
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("qty")) & "," & CDbl(TabelData.Rows(n).Item("sub_total")) & ",'" & Format(TabelData.Rows(n).Item("expired_date"), "yyyy-MM-dd HH:mm:ss") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If

            Proses.ExecuteNonQuery(SqlGlobal)

            If Success = True Then
                'Update Lokal
                SQL = "update t_retursales_hdr set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                SQL = "update t_retursales_dtl set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                Proses.ExecuteNonQueryLoc(SqlGlobalLoc)
            End If
            PB.Value = i
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Retur Sales  " & xyNoDoc)
        Next
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT RETUR SALES SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT RETUR SALES GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")
    End Sub

    Private Sub SyncSJ()
        SqlGlobal = ""
        SQL = "select no_doc from t_sj_hdr where status='APPROVED' and status_kirim = 0;"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub
        Dim xyNoDoc As String
        Dim TabelData As New DataTable

        PB.Visible = True
        PB.Value = 1
        PB.Maximum = TabelLoc.Rows.Count - 1

        For i As Integer = 0 To TabelLoc.Rows.Count - 1
            SqlGlobal = ""
            xyNoDoc = TabelLoc.Rows(i).Item("no_doc")
            SQL = "select * from t_sj_hdr where no_doc = '" & xyNoDoc & "' order by no_doc;"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If

            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_sj_hdr Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_sj_hdr(store_id,no_doc,tgl_trans,store_to,type_kendaraan,no_kendaraan,nama_driver,tot_qty,keterangan,status,created_by,creation_date,modified_by,modification_date) "
                    SQL = SQL & " Values('" & TabelData.Rows(n).Item("store_id") & "','" & TabelData.Rows(n).Item("no_doc") & "','" & Format(TabelData.Rows(n).Item("tgl_trans"), "yyyy-MM-dd HH:mm:ss") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("store_to") & "','" & TabelData.Rows(n).Item("type_kendaraan") & "','" & TabelData.Rows(n).Item("no_kendaraan") & "','" & TabelData.Rows(n).Item("nama_driver") & "',"
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("tot_qty")) & ",'" & TabelData.Rows(n).Item("keterangan") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("status") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("created_by") & "','" & Format(TabelData.Rows(n).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("modified_by") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If


            SQL = "Select * From t_sj_dtl Where no_doc = '" & xyNoDoc & "';"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If
            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_sj_dtl Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_sj_dtl (no_doc,no_trans,tgl_trans,tot_qty,ket_detail)"
                    SQL = SQL & " Values('" & xyNoDoc & "','" & TabelData.Rows(n).Item("no_trans") & "','" & Format(TabelData.Rows(n).Item("tgl_trans"), "yyyy-MM-dd") & "',"
                    SQL = SQL & " " & CDbl(TabelData.Rows(n).Item("tot_qty")) & ",'" & TabelData.Rows(n).Item("ket_detail") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If

            Proses.ExecuteNonQuery(SqlGlobal)

            If Success = True Then
                'Update Lokal
                SQL = "update t_sj_hdr set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                SQL = "update t_sj_dtl set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                Proses.ExecuteNonQueryLoc(SqlGlobalLoc)
            End If
            PB.Value = i
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Surat Jalan  " & xyNoDoc)
        Next
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT SURAT JALAN SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT SURAT JALAN GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")
    End Sub

    Private Sub SyncReturDC()
        SqlGlobal = ""
        SQL = "select no_doc from t_returdc_hdr where status='APPROVED' and status_kirim = 0;"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub
        Dim xyNoDoc As String
        Dim TabelData As New DataTable

        PB.Visible = True
        PB.Value = 1
        PB.Maximum = TabelLoc.Rows.Count - 1

        For i As Integer = 0 To TabelLoc.Rows.Count - 1
            SqlGlobal = ""
            xyNoDoc = TabelLoc.Rows(i).Item("no_doc")
            SQL = "select * from t_returdc_hdr where no_doc = '" & xyNoDoc & "' order by no_doc;"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If

            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_returdc_hdr Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_returdc_hdr(no_doc,tgl_trans,store_id,store_to,tot_qty,keterangan,status_sj,status,created_by,creation_date,modified_by,modification_date) "
                    SQL = SQL & " Values('" & TabelData.Rows(n).Item("no_doc") & "','" & Format(TabelData.Rows(n).Item("tgl_trans"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("store_id") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("store_to") & "'," & CDbl(TabelData.Rows(n).Item("tot_qty")) & ",'" & TabelData.Rows(n).Item("keterangan") & "',"
                    SQL = SQL & " " & TabelData.Rows(n).Item("status_sj") & ",'" & TabelData.Rows(n).Item("status") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("created_by") & "','" & Format(TabelData.Rows(n).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("modified_by") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If


            SQL = "Select * From t_returdc_dtl Where no_doc = '" & xyNoDoc & "';"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If
            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_returdc_dtl Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_returdc_dtl (no_doc,item_id,uom_id,qty,expired_date)"
                    SQL = SQL & " Values('" & xyNoDoc & "','" & TabelData.Rows(n).Item("item_id") & "','" & TabelData.Rows(n).Item("uom_id") & "'," & CDbl(TabelData.Rows(n).Item("qty")) & ","
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("expired_date"), "yyyy-MM-dd") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If

            Proses.ExecuteNonQuery(SqlGlobal)

            If Success = True Then
                'Update Lokal
                SQL = "update t_returdc_hdr set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                SQL = "update t_returdc_dtl set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                Proses.ExecuteNonQueryLoc(SqlGlobalLoc)
            End If
            PB.Value = i
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Retur To DC  " & xyNoDoc)
        Next
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT RETUR TO DC SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT RETUR TO DC GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")
    End Sub

    Private Sub SyncLikuidasi()
        SqlGlobal = ""
        SQL = "select no_doc from t_likuidasi_hdr where status='APPROVED' and status_kirim = 0;"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub
        Dim xyNoDoc As String
        Dim TabelData As New DataTable

        PB.Visible = True
        PB.Value = 1
        PB.Maximum = TabelLoc.Rows.Count - 1

        For i As Integer = 0 To TabelLoc.Rows.Count - 1
            SqlGlobal = ""
            xyNoDoc = TabelLoc.Rows(i).Item("no_doc")
            SQL = "select * from t_likuidasi_hdr where no_doc = '" & xyNoDoc & "' order by no_doc;"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If

            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_likuidasi_hdr Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_likuidasi_hdr (no_doc,tgl_trans,store_id,alasan,tot_qty,status,keterangan,created_by,creation_date,modified_by,modification_date) "
                    SQL = SQL & " Values('" & TabelData.Rows(n).Item("no_doc") & "','" & Format(TabelData.Rows(n).Item("tgl_trans"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("store_id") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("alasan") & "'," & CDbl(TabelData.Rows(n).Item("tot_qty")) & ",'" & TabelData.Rows(n).Item("status") & "','" & TabelData.Rows(n).Item("keterangan") & "',"
                    SQL = SQL & " '" & TabelData.Rows(n).Item("created_by") & "','" & Format(TabelData.Rows(n).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "','" & TabelData.Rows(n).Item("modified_by") & "',"
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If


            SQL = "Select * From t_likuidasi_dtl Where no_doc = '" & xyNoDoc & "';"
            TabelData = Proses.ExecuteQueryLoc(SQL)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
                On Error GoTo ErrorHandler
                Exit Sub
            End If
            If TabelData.Rows.Count > 0 Then
                SQL = "Delete From t_likuidasi_dtl Where no_doc = '" & xyNoDoc & "';"
                SqlGlobal = SqlGlobal + vbNewLine + SQL

                For n As Integer = 0 To TabelData.Rows.Count - 1
                    SQL = "Insert Into t_likuidasi_dtl (no_doc,item_id,uom_id,qty,expired_date)"
                    SQL = SQL & " Values('" & xyNoDoc & "','" & TabelData.Rows(n).Item("item_id") & "','" & TabelData.Rows(n).Item("uom_id") & "'," & CDbl(TabelData.Rows(n).Item("qty")) & ","
                    SQL = SQL & " '" & Format(TabelData.Rows(n).Item("expired_date"), "yyyy-MM-dd") & "');"
                    SqlGlobal = SqlGlobal + vbNewLine + SQL
                Next
            End If

            Proses.ExecuteNonQuery(SqlGlobal)

            If Success = True Then
                'Update Lokal
                SQL = "update t_likuidasi_hdr set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                SQL = "update t_likuidasi_dtl set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & xyNoDoc & "';"
                SqlGlobalLoc = SqlGlobalLoc + vbNewLine + SQL
                Proses.ExecuteNonQueryLoc(SqlGlobalLoc)
            End If
            PB.Value = i
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Likuidasi Barang  " & xyNoDoc)
        Next
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT LIKUIDASI BARANG SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT LIKUIDASI BARANG GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")
    End Sub

    Private Sub SyncInvTransHist()
        SqlGlobal = ""
        SQL = "select * from inv_trans_hist_pos where status_kirim = 0;"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub

        'PB.Visible = True
        'PB.Value = 1
        'PB.Maximum = TabelLoc.Rows.Count - 1

        For i As Integer = 0 To TabelLoc.Rows.Count - 1

            SQL = ""
            SQL = "Insert Into inv_trans_hist_pos (store_id,no_doc,tgl_doc,trans_code,item_id,qty,hjual,tot_hjual,expired_date,created_by,creation_date) "
            SQL = SQL & " Values('" & TabelLoc.Rows(i).Item("store_id") & "','" & TabelLoc.Rows(i).Item("no_doc") & "','" & Format(TabelLoc.Rows(i).Item("tgl_doc"), "yyyy-MM-dd HH:mm:ss") & "',"
            SQL = SQL & " '" & TabelLoc.Rows(i).Item("trans_code") & "','" & TabelLoc.Rows(i).Item("item_id") & "'," & CDbl(TabelLoc.Rows(i).Item("qty")) & "," & CDbl(TabelLoc.Rows(i).Item("hjual")) & ","
            SQL = SQL & " " & CDbl(TabelLoc.Rows(i).Item("tot_hjual")) & ",'" & Format(TabelLoc.Rows(i).Item("expired_date"), "yyyy-MM-dd HH:mm:ss") & "',"
            SQL = SQL & " '" & TabelLoc.Rows(i).Item("created_by") & "','" & Format(TabelLoc.Rows(i).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "');"
            Proses.ExecuteNonQuery(SQL)

            If Success = True Then
                'Update Lokal
                SQL = ""
                SQL = "update inv_trans_hist_pos set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where no_doc = '" & TabelLoc.Rows(i).Item("no_doc") & "' and trans_code='" & TabelLoc.Rows(i).Item("trans_code") & "' and  item_id='" & TabelLoc.Rows(i).Item("item_id") & "';"
                Proses.ExecuteNonQueryLoc(SQL)
            End If

            'PB.Value = i

        Next

        DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Inv Trans Hist")
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT DATA INV TRANS HIST SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT DATA INV TRANS HIST GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")
    End Sub

    Sub ExporttMasterCustomer()
        SQL = "select * from m_customer where status_kirim = 0;"
        TabelLoc = Proses.ExecuteQueryLoc(SQL)
        If CekLokal = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Local Disconnected..")
            Exit Sub
        End If
        If TabelLoc.Rows.Count = 0 Then Exit Sub

        For i As Integer = 0 To TabelLoc.Rows.Count - 1
            SQL = "Delete From m_customer Where cust_id = '" & TabelLoc.Rows(i).Item("cust_id") & "';"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            SQL = ""
            SQL = "Insert Into m_customer (store_id,cust_id,cust_name,tgl_lahir,tempat_lahir,alamat,country_id,prov_id,city_id,kec_id,kel_id,kode_pos,no_telp,no_hp,email,no_ktp,"
            SQL = SQL & " type_member_id,photo_ktp,photo_selfie,loc_daftar,is_sales,limit_piutang,status,created_by,creation_date,modified_by,modification_date) "
            SQL = SQL & " Values('" & TabelLoc.Rows(i).Item("store_id") & "','" & TabelLoc.Rows(i).Item("cust_id") & "','" & TabelLoc.Rows(i).Item("cust_name") & "',"
            SQL = SQL & " '" & Format(TabelLoc.Rows(i).Item("tgl_lahir"), "yyyy-MM-dd") & "','" & TabelLoc.Rows(i).Item("tempat_lahir") & "','" & TabelLoc.Rows(i).Item("alamat") & "','" & TabelLoc.Rows(i).Item("country_id") & "','" & TabelLoc.Rows(i).Item("prov_id") & "',"
            SQL = SQL & " '" & TabelLoc.Rows(i).Item("city_id") & "','" & TabelLoc.Rows(i).Item("kec_id") & "','" & TabelLoc.Rows(i).Item("kel_id") & "','" & TabelLoc.Rows(i).Item("kode_pos") & "',"
            SQL = SQL & " '" & TabelLoc.Rows(i).Item("no_telp") & "','" & TabelLoc.Rows(i).Item("no_hp") & "','" & TabelLoc.Rows(i).Item("email") & "','" & TabelLoc.Rows(i).Item("no_ktp") & "',"
            SQL = SQL & " '" & TabelLoc.Rows(i).Item("type_member_id") & "','" & TabelLoc.Rows(i).Item("photo_ktp") & "','" & TabelLoc.Rows(i).Item("photo_selfie") & "','" & TabelLoc.Rows(i).Item("loc_daftar") & "',"
            SQL = SQL & " " & TabelLoc.Rows(i).Item("is_sales") & ",'" & TabelLoc.Rows(i).Item("limit_piutang") & "'," & TabelLoc.Rows(i).Item("status") & ","
            SQL = SQL & " '" & TabelLoc.Rows(i).Item("created_by") & "','" & Format(TabelLoc.Rows(i).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "',"
            SQL = SQL & " '" & TabelLoc.Rows(i).Item("modified_by") & "','" & Format(TabelLoc.Rows(i).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            Proses.ExecuteNonQuery(SQL)

            If Success = True Then
                'Update Lokal
                SQL = ""
                SQL = "update m_customer set status_Kirim=1,tgl_kirim='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' where cust_id = '" & TabelLoc.Rows(i).Item("cust_id") & "';"
                Proses.ExecuteNonQueryLoc(SQL)
            End If

        Next

        DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Master Customer")
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT DATA MASTER CUSTOMER SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "EXPORT DATA MASTER CUSTOMER GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")

    End Sub








    '=============================================== PROSES IMPORT ====================================================
    '==================================================================================================================

    Sub ImportMasterItems()
        SqlGlobal = ""
        SQL = "select * from m_items where modification_date between '" & xyLastUpdate & "' and '" & xynow & "';"
        TabelHO = Proses.ExecuteQuery(SQL)
        If CekServer = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Server Disconnected..")
            Exit Sub
        End If
        If TabelHO.Rows.Count = 0 Then Exit Sub

        'PB.Visible = True
        'PB.Value = 1
        'PB.Maximum = TabelHO.Rows.Count - 1

        For i As Integer = 0 To TabelHO.Rows.Count - 1
            SQL = "Delete From m_items Where item_id = '" & TabelHO.Rows(i).Item("item_id") & "';"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            SQL = ""
            SQL = "Insert Into m_items (item_id,barcode,item_name,jenis_id,merk_id,type_name,uom_id,min_stock,izin_edar,keterangan,status,photo,created_by,creation_date,modified_by,modification_date) "
            SQL = SQL & " Values('" & TabelHO.Rows(i).Item("item_id") & "','" & TabelHO.Rows(i).Item("barcode") & "','" & TabelHO.Rows(i).Item("item_name") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("jenis_id") & "','" & TabelHO.Rows(i).Item("merk_id") & "','" & TabelHO.Rows(i).Item("type_name") & "','" & TabelHO.Rows(i).Item("uom_id") & "'," & CDbl(TabelHO.Rows(i).Item("min_stock")) & ","
            SQL = SQL & " '" & TabelHO.Rows(i).Item("izin_edar") & "','" & TabelHO.Rows(i).Item("keterangan") & "','" & TabelHO.Rows(i).Item("status") & "','" & TabelHO.Rows(i).Item("photo") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("created_by") & "','" & Format(TabelHO.Rows(i).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("modified_by") & "','" & Format(TabelHO.Rows(i).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            'Proses.ExecuteQueryLoc(SqlGlobal)
            'PB.Value = i

        Next
        Proses.ExecuteQueryLoc(SqlGlobal)
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Master Items")
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER ITEMS SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER ITEMS GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")

    End Sub


    '    Sub ImportMasterItemsPrice()
    '        SqlGlobal = ""
    '        SQL = "select * from m_items_price where modification_date between '" & xyLastUpdate & "' and '" & xynow & "';"
    '        TabelHO = Proses.ExecuteQuery(SQL)
    '        If CekServer = False Then
    '            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Server Disconnected..")
    '            Exit Sub
    '        End If
    '        If TabelHO.Rows.Count = 0 Then Exit Sub

    '        'PB.Visible = True
    '        'PB.Value = 1
    '        'PB.Maximum = TabelHO.Rows.Count - 1

    '        For i As Integer = 0 To TabelHO.Rows.Count - 1
    '            SQL = "Delete From m_items_price Where item_id = '" & TabelHO.Rows(i).Item("item_id") & "';"
    '            SqlGlobal = SqlGlobal + vbNewLine + SQL

    '            SQL = ""
    '            SQL = "Insert Into m_items_price (wil_id,item_id,hbeli,hjual_level1,hjual_level2,hjual_level3,hjual_level41,hjual_level42,hjual_level43,hjual_marketplace,hjual_ojol,hpp,created_by,creation_date,modified_by,modification_date) "
    '            SQL = SQL & " Values('" & TabelHO.Rows(i).Item("wil_id") & "','" & TabelHO.Rows(i).Item("item_id") & "'," & CDbl(TabelHO.Rows(i).Item("hbeli")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level1")) & ","
    '            SQL = SQL & " " & CDbl(TabelHO.Rows(i).Item("hbeli")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level1")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level2")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level3")) & ","
    '            SQL = SQL & " " & CDbl(TabelHO.Rows(i).Item("hbeli")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level1")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level2")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level3")) & ","
    '            SQL = SQL & " " & CDbl(TabelHO.Rows(i).Item("hjual_level41")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level42")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level43")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_marketplace")) & ","
    '            SQL = SQL & " " & CDbl(TabelHO.Rows(i).Item("hjual_ojol")) & "," & CDbl(TabelHO.Rows(i).Item("hpp")) & ","
    '            SQL = SQL & " '" & TabelHO.Rows(i).Item("created_by") & "','" & Format(TabelHO.Rows(i).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "',"
    '            SQL = SQL & " '" & TabelHO.Rows(i).Item("modified_by") & "','" & Format(TabelHO.Rows(i).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
    '            SqlGlobal = SqlGlobal + vbNewLine + SQL

    '            Proses.ExecuteQueryLoc(SqlGlobal)
    '            'PB.Value = i

    '        Next

    '        DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Master Items Price")
    '        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER ITEMS PRICE SUCCESS.")
    '        Exit Sub

    'ErrorHandler:
    '        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER ITEMS PRICE GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")

    '    End Sub

    Sub ImportMasterStore()
        SqlGlobal = ""
        SQL = "select * from m_store where modification_date between '" & xyLastUpdate & "' and '" & xynow & "';"
        TabelHO = Proses.ExecuteQuery(SQL)
        If CekServer = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Server Disconnected..")
            Exit Sub
        End If
        If TabelHO.Rows.Count = 0 Then Exit Sub

        'PB.Visible = True
        'PB.Value = 1
        'PB.Maximum = TabelHO.Rows.Count - 1

        For i As Integer = 0 To TabelHO.Rows.Count - 1
            SQL = "Delete From m_store Where store_id = '" & TabelHO.Rows(i).Item("store_id") & "';"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            SQL = ""
            SQL = "Insert Into m_store (store_id,store_name,wil_id,alamat,country_id,prov_id,city_id,kec_id,kel_id,kode_pos,employee_id,no_hp,no_telp,email,photo,status,created_by,creation_date,modified_by,modification_date) "
            SQL = SQL & " Values('" & TabelHO.Rows(i).Item("store_id") & "','" & TabelHO.Rows(i).Item("store_name") & "','" & TabelHO.Rows(i).Item("wil_id") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("alamat") & "','" & TabelHO.Rows(i).Item("country_id") & "','" & TabelHO.Rows(i).Item("prov_id") & "','" & TabelHO.Rows(i).Item("city_id") & "','" & TabelHO.Rows(i).Item("kec_id") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("kel_id") & "','" & TabelHO.Rows(i).Item("kode_pos") & "','" & TabelHO.Rows(i).Item("employee_id") & "','" & TabelHO.Rows(i).Item("no_hp") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("no_telp") & "','" & TabelHO.Rows(i).Item("email") & "','" & TabelHO.Rows(i).Item("photo") & "','" & TabelHO.Rows(i).Item("status") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("created_by") & "','" & Format(TabelHO.Rows(i).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("modified_by") & "','" & Format(TabelHO.Rows(i).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            'Proses.ExecuteQueryLoc(SqlGlobal)
            'PB.Value = i

        Next
        Proses.ExecuteQueryLoc(SqlGlobal)
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Master Store")
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER TOKO SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER TOKO GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")

    End Sub

    Sub ImportMasterPriceList()
        SqlGlobal = ""
        SQL = "select * from m_items_price where modification_date between '" & xyLastUpdate & "' and '" & xynow & "';"
        TabelHO = Proses.ExecuteQuery(SQL)
        If CekServer = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Server Disconnected..")
            Exit Sub
        End If
        If TabelHO.Rows.Count = 0 Then Exit Sub

        'PB.Visible = True
        'PB.Value = 1
        'PB.Maximum = TabelHO.Rows.Count - 1

        For i As Integer = 0 To TabelHO.Rows.Count - 1
            SQL = "Delete From m_items_price Where item_id = '" & TabelHO.Rows(i).Item("item_id") & "';"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            SQL = ""
            SQL = "Insert Into m_items_price (wil_id,item_id,hbeli,hjual_level1,hjual_level2,hjual_level3,hjual_level41,hjual_level42,hjual_level43,hjual_marketplace,hjual_ojol,hpp,created_by,creation_date,modified_by,modification_date) "
            SQL = SQL & " Values('" & TabelHO.Rows(i).Item("wil_id") & "','" & TabelHO.Rows(i).Item("item_id") & "',0,"
            SQL = SQL & " " & CDbl(TabelHO.Rows(i).Item("hjual_level1")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level2")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level3")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level41")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level42")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_level43")) & ","
            SQL = SQL & " " & CDbl(TabelHO.Rows(i).Item("hjual_marketplace")) & "," & CDbl(TabelHO.Rows(i).Item("hjual_ojol")) & ",0,"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("created_by") & "','" & Format(TabelHO.Rows(i).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("modified_by") & "','" & Format(TabelHO.Rows(i).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            'Proses.ExecuteQueryLoc(SqlGlobal)
            'PB.Value = i

        Next
        Proses.ExecuteQueryLoc(SqlGlobal)
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Master Price List")
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER PRICE LIST SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER PRICE LIST GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")

    End Sub

    Sub ImportMasterJenis()
        SqlGlobal = ""
        SQL = "select * from m_jenis where modification_date between '" & xyLastUpdate & "' and '" & xynow & "';"
        TabelHO = Proses.ExecuteQuery(SQL)
        If CekServer = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Server Disconnected..")
            Exit Sub
        End If
        If TabelHO.Rows.Count = 0 Then Exit Sub

        'PB.Visible = True
        'PB.Value = 1
        'PB.Maximum = TabelHO.Rows.Count - 1

        For i As Integer = 0 To TabelHO.Rows.Count - 1
            SQL = "Delete From m_jenis Where jenis_id = '" & TabelHO.Rows(i).Item("jenis_id") & "';"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            SQL = ""
            SQL = "Insert Into m_jenis (jenis_id,jenis_name,status,use_expire,created_by,creation_date,modified_by,modification_date) "
            SQL = SQL & " Values('" & TabelHO.Rows(i).Item("jenis_id") & "','" & TabelHO.Rows(i).Item("jenis_name") & "',"
            SQL = SQL & " " & TabelHO.Rows(i).Item("status") & "," & TabelHO.Rows(i).Item("use_expire") & ","
            SQL = SQL & " '" & TabelHO.Rows(i).Item("created_by") & "','" & Format(TabelHO.Rows(i).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("modified_by") & "','" & Format(TabelHO.Rows(i).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            'Proses.ExecuteQueryLoc(SqlGlobal)
            'PB.Value = i

        Next
        Proses.ExecuteQueryLoc(SqlGlobal)
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Master Jenis")
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER JENIS SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER JENIS GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")

    End Sub

    Sub ImportMasterMerk()
        SqlGlobal = ""
        SQL = "select * from m_merk where modification_date between '" & xyLastUpdate & "' and '" & xynow & "';"
        TabelHO = Proses.ExecuteQuery(SQL)
        If CekServer = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Server Disconnected..")
            Exit Sub
        End If
        If TabelHO.Rows.Count = 0 Then Exit Sub

        'PB.Visible = True
        'PB.Value = 1
        'PB.Maximum = TabelHO.Rows.Count - 1

        For i As Integer = 0 To TabelHO.Rows.Count - 1
            SQL = "Delete From m_merk Where merk_id = '" & TabelHO.Rows(i).Item("merk_id") & "';"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            SQL = ""
            SQL = "Insert Into m_merk (merk_id,merk_name,status,created_by,creation_date,modified_by,modification_date) "
            SQL = SQL & " Values('" & TabelHO.Rows(i).Item("merk_id") & "','" & TabelHO.Rows(i).Item("merk_name") & "',"
            SQL = SQL & " " & TabelHO.Rows(i).Item("status") & ","
            SQL = SQL & " '" & TabelHO.Rows(i).Item("created_by") & "','" & Format(TabelHO.Rows(i).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("modified_by") & "','" & Format(TabelHO.Rows(i).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            'Proses.ExecuteQueryLoc(SqlGlobal)
            'PB.Value = i

        Next
        Proses.ExecuteQueryLoc(SqlGlobal)
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Master Merk")
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER MERK SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER MERK GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")

    End Sub

    Sub ImportMasterCOA()
        SqlGlobal = ""
        SQL = "select * from m_coa where modification_date between '" & xyLastUpdate & "' and '" & xynow & "' limit 100;"
        TabelHO = Proses.ExecuteQuery(SQL)
        If CekServer = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Server Disconnected..")
            Exit Sub
        End If
        If TabelHO.Rows.Count = 0 Then Exit Sub

        'PB.Visible = True
        'PB.Value = 1
        'PB.Maximum = TabelHO.Rows.Count - 1

        For i As Integer = 0 To TabelHO.Rows.Count - 1
            SQL = "Delete From m_coa Where code = '" & TabelHO.Rows(i).Item("code") & "';"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            SQL = ""
            SQL = "Insert Into m_coa (group_code,parent_code,code,name,type,is_cash_bank,created_by,creation_date,modified_by,modification_date) "
            SQL = SQL & " Values('" & TabelHO.Rows(i).Item("group_code") & "','" & TabelHO.Rows(i).Item("parent_code") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("code") & "','" & TabelHO.Rows(i).Item("name") & "','" & TabelHO.Rows(i).Item("type") & "'," & TabelHO.Rows(i).Item("is_cash_bank") & ","
            SQL = SQL & " '" & TabelHO.Rows(i).Item("created_by") & "','" & Format(TabelHO.Rows(i).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("modified_by") & "','" & Format(TabelHO.Rows(i).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "');"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            'Proses.ExecuteQueryLoc(SqlGlobal)
            'PB.Value = i

        Next
        Proses.ExecuteQueryLoc(SqlGlobal)
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Master COA")
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER COA SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER COA GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")

    End Sub

    Sub ImportDO()
        SqlGlobal = ""
        SQL = "select * from t_delivery_hdr where store_id_dest='" & xyzStore & "';"
        TabelHO = Proses.ExecuteQuery(SQL)
        If CekServer = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Server Disconnected..")
            Exit Sub
        End If
        If TabelHO.Rows.Count = 0 Then Exit Sub
        Dim xyNoDoc As String
        Dim TabelData As New DataTable
        PB.Visible = True
        'PB.Value = 1
        PB.Maximum = TabelHO.Rows.Count - 1

        For i As Integer = 0 To TabelHO.Rows.Count - 1
            xyNoDoc = TabelHO.Rows(i).Item("no_doc")
            SQL2 = ""
            SQL2 = "select * from t_delivery_hdr where no_doc='" & xyNoDoc & "'"
            TabelLoc = Proses.ExecuteQueryLoc(SQL2)
            If CekLokal = False Then
                DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Lokal Disconnected..")
                Exit Sub
            End If
            If TabelLoc.Rows.Count = 0 Then

                SQL = "Insert Into t_delivery_hdr (store_id,no_doc,tgl_trans,use_ref,ref_no_doc,store_id_dest,status,total_qty,keterangan,created_by,creation_date) "
                SQL = SQL & " Values('" & TabelHO.Rows(i).Item("store_id") & "','" & xyNoDoc & "','" & Format(TabelHO.Rows(i).Item("tgl_trans"), "yyyy-MM-dd HH:mm:ss") & "'," & TabelHO.Rows(i).Item("use_ref") & ","
                SQL = SQL & " '" & TabelHO.Rows(i).Item("ref_no_doc") & "','" & TabelHO.Rows(i).Item("store_id_dest") & "'," & TabelHO.Rows(i).Item("status") & "," & CDbl(TabelHO.Rows(i).Item("total_qty")) & ",'" & TabelHO.Rows(i).Item("keterangan") & "'," '

                SQL = SQL & " '" & TabelHO.Rows(i).Item("created_by") & "','" & Format(TabelHO.Rows(i).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "');"
                SqlGlobal = SqlGlobal + vbNewLine + SQL


                SQL = "Select * From t_delivery_dtl Where no_doc = '" & xyNoDoc & "';"
                TabelData = Proses.ExecuteQuery(SQL)
                If CekServer = False Then
                    DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Server Disconnected..")
                    On Error GoTo ErrorHandler
                    Exit Sub
                End If

                If TabelData.Rows.Count > 0 Then
                    For n As Integer = 0 To TabelData.Rows.Count - 1
                        SQL = "Insert Into t_delivery_dtl (no_doc,item_id,uom_id,qty,expired_date)"
                        SQL = SQL & " Values('" & xyNoDoc & "','" & TabelData.Rows(n).Item("item_id") & "','" & TabelData.Rows(n).Item("uom_id") & "'," & CDbl(TabelData.Rows(n).Item("qty")) & ","
                        SQL = SQL & " '" & Format(TabelData.Rows(n).Item("expired_date"), "yyyy-MM-dd") & "');"
                        SqlGlobal = SqlGlobal + vbNewLine + SQL
                    Next
                End If

            End If
            'Proses.ExecuteQueryLoc(SqlGlobal)
            PB.Value = i

        Next
        Proses.ExecuteQueryLoc(SqlGlobal)
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync DO")
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA DO SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA DO GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")

    End Sub


    Sub ImportMasterCustomer()
        SQL = "select * from m_customer where modification_date between '" & xyLastUpdate & "' and '" & xynow & "';"
        TabelHO = Proses.ExecuteQuery(SQL)
        If CekServer = False Then
            DG.Rows.Add(Format(Now, "HH:mm:ss"), "Database Server Disconnected..")
            Exit Sub
        End If
        If TabelHO.Rows.Count = 0 Then Exit Sub

        'PB.Visible = True
        'PB.Value = 1
        'PB.Maximum = TabelHO.Rows.Count - 1

        For i As Integer = 0 To TabelHO.Rows.Count - 1
            SQL = "Delete From m_customer Where cust_id = '" & TabelHO.Rows(i).Item("cust_id") & "';"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            SQL = ""
            SQL = "Insert Into m_customer (store_id,cust_id,cust_name,tgl_lahir,tempat_lahir,alamat,country_id,prov_id,city_id,kec_id,kel_id,kode_pos,no_telp,no_hp,email,no_ktp,"
            SQL = SQL & " type_member_id,photo_ktp,photo_selfie,loc_daftar,is_sales,status,created_by,creation_date,modified_by,modification_date,status_kirim) "
            SQL = SQL & " Values('" & TabelHO.Rows(i).Item("store_id") & "','" & TabelHO.Rows(i).Item("cust_id") & "','" & TabelHO.Rows(i).Item("cust_name") & "',"
            SQL = SQL & " '" & Format(TabelHO.Rows(i).Item("tgl_lahir"), "yyyy-MM-dd") & "','" & TabelHO.Rows(i).Item("tempat_lahir") & "','" & TabelHO.Rows(i).Item("alamat") & "','" & TabelHO.Rows(i).Item("country_id") & "','" & TabelHO.Rows(i).Item("prov_id") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("city_id") & "','" & TabelHO.Rows(i).Item("kec_id") & "','" & TabelHO.Rows(i).Item("kel_id") & "','" & TabelHO.Rows(i).Item("kode_pos") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("no_telp") & "','" & TabelHO.Rows(i).Item("no_hp") & "','" & TabelHO.Rows(i).Item("email") & "','" & TabelHO.Rows(i).Item("no_ktp") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("type_member_id") & "','" & TabelHO.Rows(i).Item("photo_ktp") & "','" & TabelHO.Rows(i).Item("photo_selfie") & "','" & TabelHO.Rows(i).Item("loc_daftar") & "',"
            SQL = SQL & " " & TabelHO.Rows(i).Item("is_sales") & "," & TabelHO.Rows(i).Item("status") & ","
            SQL = SQL & " '" & TabelHO.Rows(i).Item("created_by") & "','" & Format(TabelHO.Rows(i).Item("creation_date"), "yyyy-MM-dd HH:mm:ss") & "',"
            SQL = SQL & " '" & TabelHO.Rows(i).Item("modified_by") & "','" & Format(TabelHO.Rows(i).Item("modification_date"), "yyyy-MM-dd HH:mm:ss") & "',1);"
            SqlGlobal = SqlGlobal + vbNewLine + SQL

            'Proses.ExecuteQueryLoc(SqlGlobal)
            'PB.Value = i

        Next
        Proses.ExecuteQueryLoc(SqlGlobal)
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "Sync Master Customer")
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER CUSTOMER SUCCESS.")
        Exit Sub

ErrorHandler:
        DG.Rows.Add(Format(Now, "HH:mm:ss"), "IMPORT DATA MASTER CUSTOMER GAGAL.. SILAHKAN LAKUKAN EXPORT KEMBALI!!")

    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        End
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        If Mundur = 0 Then
            DG.Rows.Clear()
            Call ExporttMasterCustomer()
            Call SyncSales()
            Call SyncSR()
            Call SyncReceive()
            Call SyncTransferOut()
            Call SyncTransferIn()
            Call SyncSO()
            Call SyncReturSales()
            Call SyncSJ()
            Call SyncReturDC()
            Call SyncLikuidasi()
            Call SyncInvTransHist()

            Call ImportMasterItems()
            Call ImportMasterPriceList()
            Call ImportMasterStore()
            Call ImportMasterJenis()
            Call ImportMasterMerk()
            Call ImportMasterCOA()
            Call ImportDO()
            Call ImportMasterCustomer()
            Call UpadteTabelData()


            Mundur = 60 * CDbl(txtDurasi.Text)
            'bw1.RunWorkerAsync()
        Else
            Mundur = Mundur - 1
            btnProcess.Text = "Process Send " & Mundur
        End If
    End Sub

    Sub UpadteTabelData()
        SQL = ""
        SQL = "update m_update_data set last_update='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "'"
        Proses.ExecuteQueryLoc(SQL)
    End Sub

    Private Sub btnProcess_Click(sender As System.Object, e As System.EventArgs) Handles btnProcess.Click
        Mundur = 0
    End Sub

    Sub MyStore()
        SQL = "select * from m_company;"
        Tabel = Proses.ExecuteQueryLoc(SQL)
        If Tabel.Rows.Count > 0 Then
            xyzStore = Tabel.Rows(0).Item("store_id")
        End If
    End Sub

    Private Sub NotifyIcon1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            ContextMenuStrip1.Show(MousePosition)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
        NotifyIcon1.Visible = True
    End Sub

    Private Sub TampilToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TampilToolStripMenuItem.Click
        Me.Show()
        NotifyIcon1.Visible = False
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub bw1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bw1.DoWork
        DG.Rows.Clear()
        Call ExporttMasterCustomer()
        Call SyncSales()
        Call SyncSR()
        Call SyncReceive()
        Call SyncTransferOut()
        Call SyncTransferIn()
        Call SyncSO()
        Call SyncReturSales()
        Call SyncSJ()
        Call SyncReturDC()
        Call SyncLikuidasi()
        Call SyncInvTransHist()

        Call ImportMasterItems()
        Call ImportMasterPriceList()
        Call ImportMasterStore()
        Call ImportMasterJenis()
        Call ImportMasterMerk()
        Call ImportMasterCOA()
        Call ImportDO()
        Call ImportMasterCustomer()
        Call UpadteTabelData()


        Mundur = 60 * CDbl(txtDurasi.Text)
        System.Threading.Thread.Sleep(1000)
    End Sub
End Class
