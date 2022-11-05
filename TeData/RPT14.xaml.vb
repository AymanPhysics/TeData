Imports System.Data

Public Class RPT14

    Dim MainTableName As String = "Branches"

    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@BranchId", "@IncomeTypeId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(CboMain.SelectedValue.ToString), Val(IncomeTypeId.SelectedValue.ToString), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}

        Select Case Flag
            Case 1
                rpt.Rpt = "ItemMotion.rpt"
            Case 2
                rpt.Rpt = "ItemMotionAll.rpt"
        End Select
        rpt.ShowDialog()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return

        bm.FillCombo(MainTableName, CboMain, "")
        
        bm.FillCombo("select 0 Id,'-' Name union select cast(Id as varchar(100)) Id,Name from IncomeTypes where HasBalance=1", IncomeTypeId)

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

        If Flag = 2 Then
            lblDayDate.Visibility = Windows.Visibility.Hidden
            FromDate.Visibility = Windows.Visibility.Hidden
            
            lblType.Visibility = Windows.Visibility.Hidden
            IncomeTypeId.Visibility = Windows.Visibility.Hidden
        End If

    End Sub


End Class
