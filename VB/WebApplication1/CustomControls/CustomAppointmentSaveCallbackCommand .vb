Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports DevExpress.Web
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports DevExpress.XtraScheduler.Native
Imports DevExpress.XtraScheduler

Namespace WebApplication1.CustomControls
    Public Class CustomAppointmentSaveCallbackCommand
        Inherits AppointmentFormSaveCallbackCommand
        Public Sub New(ByVal control As ASPxScheduler)
            MyBase.New(control)
        End Sub

        Protected Friend Shadows ReadOnly Property Controller() As CustomAppointmentFormController
            Get
                Return CType(MyBase.Controller, CustomAppointmentFormController)
            End Get
        End Property

        Protected Overrides Function FindControlByID(ByVal id As String) As System.Web.UI.Control
            Return FindTemplateControl(TemplateContainer, id)
        End Function

        Private Function FindTemplateControl(ByVal RootControl As System.Web.UI.Control, ByVal id As String) As System.Web.UI.Control
            Dim foundedControl As System.Web.UI.Control = RootControl.FindControl(id)
            If foundedControl Is Nothing Then
                For Each item As System.Web.UI.Control In RootControl.Controls
                    foundedControl = FindTemplateControl(item, id)
                    If foundedControl IsNot Nothing Then
                        Exit For
                    End If
                Next item
            End If
            Return foundedControl
        End Function

        Protected Overrides Sub AssignControllerValues()
            MyBase.AssignControllerValues()

            Dim tbSubject As ASPxTextBox = CType(FindControlByID("ASPxTextBoxSubject"), ASPxTextBox)
            Dim deStart As ASPxDateEdit = CType(FindControlByID("ASPxDateEditStart"), ASPxDateEdit)
            Dim deEnd As ASPxDateEdit = CType(FindControlByID("ASPxDateEditEnd"), ASPxDateEdit)
            Dim tbDescription As ASPxMemo = CType(FindControlByID("ASPxMemoDescription"), ASPxMemo)

            Dim cbCompany As ASPxComboBox = CType(FindControlByID("ASPxComboBoxCompany"), ASPxComboBox)
            Dim cbContact As ASPxComboBox = CType(FindControlByID("ASPxComboBoxContact"), ASPxComboBox)

            Controller.Subject = tbSubject.Text
            Controller.Start = Convert.ToDateTime(deStart.Value)
            Controller.End = Convert.ToDateTime(deEnd.Value)
            Controller.Description = tbDescription.Text
            Controller.CompanyIDField = Convert.ToInt32(cbCompany.Value)
            Controller.ContactIDField = Convert.ToInt32(cbContact.Value)

            If Controller.ResourceId Is ResourceEmpty.Id Then
                Controller.ResourceId = Control.SelectedResource.Id
            End If
        End Sub

        Protected Overrides Function CreateAppointmentFormController(ByVal apt As DevExpress.XtraScheduler.Appointment) As AppointmentFormController
            Return New CustomAppointmentFormController(Control, apt)
        End Function
    End Class
End Namespace