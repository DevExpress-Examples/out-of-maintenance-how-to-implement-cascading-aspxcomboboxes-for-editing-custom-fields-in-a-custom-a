Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports DevExpress.XtraScheduler

Namespace WebApplication1.CustomControls
	Public Class CustomAppointmentFormController
		Inherits AppointmentFormController
		Public Sub New(ByVal control As ASPxScheduler, ByVal apt As Appointment)
			MyBase.New(control, apt)

		End Sub

		Public Property CompanyIDField() As Integer
			Get
				Return Convert.ToInt32(EditedAppointmentCopy.CustomFields("AppointmentCompany"))
			End Get
			Set(ByVal value As Integer)
				EditedAppointmentCopy.CustomFields("AppointmentCompany") = value
			End Set
		End Property

		Public Property ContactIDField() As Integer
			Get
				Return Convert.ToInt32(EditedAppointmentCopy.CustomFields("AppointmentContact"))
			End Get
			Set(ByVal value As Integer)
				EditedAppointmentCopy.CustomFields("AppointmentContact") = value
			End Set
		End Property
	End Class
End Namespace