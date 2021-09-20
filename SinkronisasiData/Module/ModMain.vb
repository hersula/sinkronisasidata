Imports System.Data
Imports System.Data.Sql
Imports MySql.Data.MySqlClient

Module ModMain
    Public BeginCommit, BeginCommitLoc As SqlClient.SqlTransaction
    Public xytahun
    Public Konek As New ModPelengkap
    Public SQL As String
    Public SQL2 As String
    'Public Cn As MySqlConnection
    'Public Cmd As MySqlCommand
    'Public Da As MySqlDataAdapter
    'Public Ds As DataSet
    'Public Dt As DataTable

    Public Tabel As DataTable
    Public Tabel2 As DataTable
    Public Tabel3 As DataTable
    Public Proses As New ClsKoneksiDatabase
    Public VarStr, VarStr1, VarStr2, VarStr3 As String
    Public Status, Posisi_Record As Boolean
    Public Diskon As Single
    Public No, Stock, Stock_Awal, Jml_Beli, Jml_Return, Jml_Jual, Sub_Total, No_Kartu As Integer
    Public Pengguna As String
    Public Member As String
    Public StoreId As String
    Public storename As String
    Public xyno As String
    Public xynoref As String
    Public xystatus As String
    Public xyloc As String
    Public blnAdd As Boolean
    Public blnEdit As Boolean
    Public xyznodoc As String
    Public strNoDoc As String
    Public svitemid As String
    Public svuomid As String
    Public svqtykirim As Double
    Public svqtyterima As Double
    Public svexpdate
    Public svexpdate2
    Public svcanceldate
    Public svcanceldate2
    Public svtglbayar
    Public svtglbayar2
    Public svlocid As String
    Public svitemname As String
    Public svhjual As Double
    Public svqty As Single
    Public svdisc As Double
    Public svsubtotal As Double
    Public svtot As Double
    Public svnotrans As String
    Public svtgltrans
    Public svtotqty As Single
    Public svket As String

    Public xyztotalAll As Double
    Public xyzkode As String
    Public strTypePay As String
    Public stritemID As String
    Public xylocdaftar As String
    Public xycountryid As String
    Public xyprovid As String
    Public xycityid As String
    Public xykecid As String
    Public xykelid As String
    Public xytypememberid As String
    Public strAktif As String
    Public xybankid As String
    Public xycountryname As String
    Public xyprovname As String
    Public xycityname As String
    Public xykecname As String
    Public xykelname As String
    Public xybankname As String
    Public xyemployeeid As String
    Public xyemployeename As String
    Public xyitemid As String
    Public xyitemname As String
    Public xycategoryname As String
    Public xymerkname As String
    Public xyuomid As String
    Public xyuomname As String
    Public xyhbeli As Double
    Public xysuppid As String
    Public xysuppname As String
    Public xyalamat As String
    Public xytop As Integer
    Public xyppn As Integer
    Public xystoreid As String
    Public xystorename As String
    Public xydaftarid As String
    Public xydaftarname As String
    Public xyroleid As String
    Public xyrolename As String
    Public xyzrolecode As String
    Public xyzrolename As String
    Public xywilID As String
    Public xywilname As String

    Public svrole
    Public svaccessid
    Public svaccessflag
    Public svaddflag
    Public sveditflag
    Public svconfirmflag
    Public svdeleteflag
    Public svprintflag
    Public svkirimflag

    Public shaddflag
    Public sheditflag
    Public shconfirmflag
    Public shdeleteflag
    Public shprintflag

    Public xycustid As String
    Public xycustname As String
    Public xycusttype As String
    Public xycusttypeID As String
    Public xycustalamat As String
    Public xyissales As Integer
    Public xyhjual As Double
    Public intRow As Integer

    Public blntunai As Boolean

    Public byrtotal As Double
    Public byrbayar As Double
    Public byrsisa As Double
    Public byrcash As Double
    Public byrcredit As Double
    Public byrdebit As Double
    Public byrnamacredit As String
    Public byrnocredit As String
    Public byrnamadebit As String
    Public byrnodebit As String

    Public shitemid As String
    Public shitemname As String
    Public shhjual As Double
    Public xyExpDate
    Public xyminbeli As String
    Public blnclose As Boolean
    Public blnSaeles As Boolean
    Public btnRetur As Boolean
    Public xydate

    Public TxMana, Success, CekInet, CekLokal, CekServer As Boolean
    Public TabelLoc As DataTable
    Public TabelHO As DataTable
    Public SqlGlobal As String
    Public SqlGlobalLoc As String
    Public xyzStore As String
End Module