<!-- default file list -->
*Files to look at*:

* [CustomAppointmentForm.ascx](./CS/WebApplication1/CustomControls/CustomAppointmentForm.ascx) (VB: [CustomAppointmentForm.ascx](./VB/WebApplication1/CustomControls/CustomAppointmentForm.ascx))
* [CustomAppointmentForm.ascx.cs](./CS/WebApplication1/CustomControls/CustomAppointmentForm.ascx.cs) (VB: [CustomAppointmentForm.ascx](./VB/WebApplication1/CustomControls/CustomAppointmentForm.ascx))
* [CustomAppointmentFormController.cs](./CS/WebApplication1/CustomControls/CustomAppointmentFormController.cs) (VB: [CustomAppointmentFormController.vb](./VB/WebApplication1/CustomControls/CustomAppointmentFormController.vb))
* [CustomAppointmentSaveCallbackCommand .cs](./CS/WebApplication1/CustomControls/CustomAppointmentSaveCallbackCommand .cs) (VB: [CustomAppointmentSaveCallbackCommand .vb](./VB/WebApplication1/CustomControls/CustomAppointmentSaveCallbackCommand .vb))
* [CustomDataSource.cs](./CS/WebApplication1/CustomDataSource.cs) (VB: [CustomDataSource.vb](./VB/WebApplication1/CustomDataSource.vb))
* [CustomObjects.cs](./CS/WebApplication1/CustomObjects.cs) (VB: [CustomObjects.vb](./VB/WebApplication1/CustomObjects.vb))
* [Default.aspx](./CS/WebApplication1/Default.aspx) (VB: [Default.aspx.vb](./VB/WebApplication1/Default.aspx.vb))
* [Default.aspx.cs](./CS/WebApplication1/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebApplication1/Default.aspx.vb))
<!-- default file list end -->
# How to implement cascading ASPxComboBoxes for editing custom fields in a custom Appointment Edit form


<p>This example illustrates a server-side technique of editing hierarchical custom appointment fields using cascading ASPxComboBoxes.<br />The main idea of implementing cascading ASPxComboBoxes was demonstrated in the following example:<br /><a href="https://www.devexpress.com/Support/Center/p/E2355">A general technique of using cascading ASPxComboBoxes</a><br />In this example, we added two custom fields for appointments (<strong>CompanyID</strong> and <strong>ContactID</strong>)<strong> </strong>and two corresponding ASPxComboBoxes onto a custom Appointment Edit form. <br />An approach for customizing the Appointment Edit Form for working with custom fields was described here:<br /><a href="https://documentation.devexpress.com/#AspNet/CustomDocument5464">How to: Modify the Appointment Editing Form for Working with Custom Fields</a><br />Changing a value of the "<strong>CompanyID</strong>" combobox results in filtering data in the "<strong>ContactID</strong>" combobox.</p>

<br/>


