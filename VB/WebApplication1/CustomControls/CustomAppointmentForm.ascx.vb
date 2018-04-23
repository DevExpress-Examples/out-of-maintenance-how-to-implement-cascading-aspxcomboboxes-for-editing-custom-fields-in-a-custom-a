Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Localization

Namespace WebApplication1.CustomControls
	Partial Public Class CustomAppointmentForm
		Inherits SchedulerFormControl
		Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

		End Sub

		Public ReadOnly Property CanShowReminders() As Boolean
			Get
				Return (CType(Parent, AppointmentFormTemplateContainer)).Control.Storage.EnableReminders
			End Get
		End Property
		Public ReadOnly Property ResourceSharing() As Boolean
			Get
				Return (CType(Parent, AppointmentFormTemplateContainer)).Control.Storage.ResourceSharing
			End Get
		End Property

		Public ReadOnly Property ResourceDataSource() As IEnumerable
			Get
				Return (CType(Parent, AppointmentFormTemplateContainer)).ResourceDataSource
			End Get
		End Property

		Public Overrides Sub DataBind()
			MyBase.DataBind()
            Dim container As AppointmentFormTemplateContainer = CType(Parent, AppointmentFormTemplateContainer)
			ASPxTextBoxSubject.Text = container.Subject
			ASPxDateEditStart.Value = container.Start
			ASPxDateEditEnd.Value = container.End
            ASPxMemoDescription.Text = container.Appointment.Description

			BindComboBoxes()

			If container.CustomFields("AppointmentCompany") IsNot Nothing Then
				ASPxComboBoxCompany.Value = container.CustomFields("AppointmentCompany")
				FillContactComboBox(container.CustomFields("AppointmentCompany").ToString())
			End If

			If container.CustomFields("AppointmentContact") IsNot Nothing Then
				ASPxComboBoxContact.Value = container.CustomFields("AppointmentContact")
			End If

			PopulateResourceEditors(container.Appointment, container)

			AppointmentRecurrenceForm1.Visible = container.ShouldShowRecurrence

			If container.Appointment.HasReminder Then
				cbReminder.Value = container.Appointment.Reminder.TimeBeforeStart.ToString()
				chkReminder.Checked = True
			Else
				cbReminder.ClientEnabled = False
			End If

			ASPxButtonOk.ClientSideEvents.Click = container.SaveHandler
			ASPxButtonCancel.ClientSideEvents.Click = container.CancelHandler
			ASPxButtonDelete.ClientSideEvents.Click = container.DeleteHandler
		End Sub

		Private Sub BindComboBoxes()
			If Session("CompanyDataSource") Is Nothing Then
				Session("CompanyDataSource") = GenerateCompanyDataSource()
			End If
			ASPxComboBoxCompany.DataSource = Session("CompanyDataSource")
			ASPxComboBoxCompany.DataBind()


			If Session("ContactDataSource") Is Nothing Then
				Session("ContactDataSource") = GenerateContactDataSource()
			End If
			ASPxComboBoxContact.DataSource = Session("ContactDataSource")
			ASPxComboBoxContact.DataBind()
		End Sub

		Private Function GenerateCompanyDataSource() As List(Of Company)
			Dim returnedResult As New List(Of Company)()
			For i As Integer = 0 To 9
				returnedResult.Add(New Company() With {.CompanyID = i, .CompanyName = "Company " & i.ToString()})
			Next i
			Return returnedResult
		End Function

		Private Function GenerateContactDataSource() As List(Of CompanyContact)
			Dim returnedResult As New List(Of CompanyContact)()
			Dim companies As List(Of Company) = TryCast(Session("CompanyDataSource"), List(Of Company))

			Dim uniqueContactID As Integer = 0
			For i As Integer = 0 To companies.Count - 1
				For j As Integer = 0 To 4
					returnedResult.Add(New CompanyContact() With {.CompanyID = i, .ContactName = "Contact " & j.ToString() & ", Company " & i.ToString(), .ContactID = uniqueContactID})
					uniqueContactID += 1
				Next j
			Next i
			Return returnedResult
		End Function

		Protected Sub ASPxComboBoxContact_Callback(ByVal sender As Object, ByVal e As DevExpress.Web.CallbackEventArgsBase)
			FillContactComboBox(e.Parameter)
		End Sub

		Protected Sub FillContactComboBox(ByVal country As String)
			If String.IsNullOrEmpty(country) Then
				Return
			End If

			If Session("ContactDataSource") IsNot Nothing Then
				ASPxComboBoxContact.DataSource = (TryCast(Session("ContactDataSource"), List(Of CompanyContact))).Where(Function(cont) cont.CompanyID.Equals(Convert.ToInt32(country))).ToList()
				ASPxComboBoxContact.DataBind()
			End If
		End Sub

		Private Sub PopulateResourceEditors(ByVal apt As Appointment, ByVal container As AppointmentFormTemplateContainer)
			If ResourceSharing Then
				Dim edtMultiResource As ASPxListBox = TryCast(ddResource.FindControl("edtMultiResource"), ASPxListBox)
				If edtMultiResource Is Nothing Then
					Return
				End If
				SetListBoxSelectedValues(edtMultiResource, apt.ResourceIds)
				Dim multiResourceString As List(Of String) = GetListBoxSeletedItemsText(edtMultiResource)
				Dim stringResourceNone As String = SchedulerLocalizer.GetString(SchedulerStringId.Caption_ResourceNone)
				ddResource.Value = stringResourceNone
				If multiResourceString.Count > 0 Then
					ddResource.Value = String.Join(", ", multiResourceString.ToArray())
				End If
				ddResource.JSProperties.Add("cp_Caption_ResourceNone", stringResourceNone)
			Else
                If (Not Object.Equals(apt.ResourceId, ResourceEmpty.Id)) Then
                    edtResource.Value = apt.ResourceId.ToString()
                Else
                    edtResource.Value = SchedulerIdHelper.EmptyResourceId
                End If
			End If
		End Sub
		Private Function GetListBoxSeletedItemsText(ByVal listBox As ASPxListBox) As List(Of String)
			Dim result As New List(Of String)()
			For Each editItem As ListEditItem In listBox.Items
				If editItem.Selected Then
					result.Add(editItem.Text)
				End If
			Next editItem
			Return result
		End Function
		Private Sub SetListBoxSelectedValues(ByVal listBox As ASPxListBox, ByVal values As IEnumerable)
			listBox.Value = Nothing
			For Each value As Object In values
				Dim item As ListEditItem = listBox.Items.FindByValue(value.ToString())
				If item IsNot Nothing Then
					item.Selected = True
				End If
			Next value
		End Sub
		Protected Overrides Sub PrepareChildControls()
            Dim container As AppointmentFormTemplateContainer = CType(Parent, AppointmentFormTemplateContainer)
			Dim control As ASPxScheduler = container.Control

			AppointmentRecurrenceForm1.EditorsInfo = New EditorsInfo(control, control.Styles.FormEditors, control.Images.FormEditors, control.Styles.Buttons)
			MyBase.PrepareChildControls()
		End Sub

		#Region "#getchildeditors"
		Protected Overrides Function GetChildEditors() As ASPxEditBase()
			' Required to implement client-side functionality
			Dim edits() As ASPxEditBase = { ASPxLabelSubject, ASPxTextBoxSubject, ASPxLabelStart, ASPxDateEditStart, ASPxLabelEnd, ASPxLabelStart, ASPxMemoDescription, ASPxLabelContact, ASPxComboBoxContact, ASPxLabelCompany, ASPxComboBoxCompany }
			Return edits
		End Function
		#End Region ' #getchildeditors
		Protected Overrides Function GetChildButtons() As ASPxButton()
			Dim buttons() As ASPxButton = { ASPxButtonOk, ASPxButtonCancel, ASPxButtonDelete }
			Return buttons
        End Function

		Public Function GetActualContainer(ByVal Container As Object) As AppointmentFormTemplateContainer
            Return TryCast(Container, AppointmentFormTemplateContainer)
        End Function
    End Class
End Namespace