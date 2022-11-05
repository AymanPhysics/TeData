Imports System.Data

Imports System
Imports System.Collections
Imports System.ComponentModel

Imports System.Drawing
Imports System.Text
Imports System.Drawing.Imaging
 
Imports System.IO
 
Imports TCPCamActivex

Public Class PrintResetOne

    Dim bm As New BasicMethods

    Dim lop As Boolean = False

    Private Sub PrintResetOne_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If TypeOf Parent Is Window Then
            Return
        End If
        Dim m As New Window With {.Content = New PrintResetOne}
        m.WindowState = WindowState.Maximized
        m.WindowStyle = WindowStyle.None
        m.Show()
        m.BringIntoView()
    End Sub

    Private Sub Button2_Click(sender As Object, e As RoutedEventArgs) Handles Button2.Click
        Dim s As Integer = Val(bm.ExecuteScalar("exec GenerateTellerNo " & Md.DefaultStore))
        Dim m As New PrintReset
        m.PrintReset_Loaded(Nothing, Nothing)
        m.PrintReset(s)
    End Sub

End Class
