Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Drawing

Module Md
    Public LastVersion As Integer = 6
    Public MyProjectType As ProjectType = ProjectType.StaticIP
    Public Demo As Boolean = False

    Public cmd As New SqlCommand
    Public con As New SqlConnection
    Public s As New SqlClient.SqlConnectionStringBuilder
    Public FourceExit As Boolean = False
    Public HasLeft As Boolean = False

    Public UserName, ArName, BranchId, LevelId, Password, CompanyName, CompanyTel, Nurse As String
    Public Manager, Receptionist, IsTeller As Boolean
    Public DefaultStore, DefaultSave As Integer
    Public EnName As String = "Please, Login", Currentpage As String = ""

    Public StopProfiler As Boolean = False
    Public AllowCashierToEditPrice As Boolean = False
    Public ShowCostCenter As Boolean = False
    Public ShowBanks As Boolean = True
    Public ShowBarcode As Boolean = False
    Public ShowColorAndSize As Boolean = False
    Public ShowShifts As Boolean = False
    Public ShowShiftForEveryStore As Boolean = False
    Public ShowQtySub As Boolean = True
    Public ShowCurrency As Boolean = False
    Public ShowBankCash_G As Boolean = False
    Public ShowStoresMotionsEditing As Boolean = False
    Public CustView As Boolean = False
    Public CustEdit As Boolean = False

    Public BarcodePrinter As String = ""
    Public PonePrinter As String = ""

    Public CurrentDate As DateTime
    Public CurrentShiftId As Integer = 0
    Public CurrentShiftName As String = ""
    Public Cashier As String = "0"
    Public UdlName As String = "Connect"
    Public IsLogedIn As Boolean = False

    Enum ProjectType
        Local
        Server
        StaticIP
        PCs
    End Enum

End Module
