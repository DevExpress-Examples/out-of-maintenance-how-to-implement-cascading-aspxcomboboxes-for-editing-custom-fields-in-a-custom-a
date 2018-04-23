using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Localization;

namespace WebApplication1.CustomControls {
    public partial class CustomAppointmentForm : SchedulerFormControl {
        protected void Page_Load(object sender, EventArgs e) {

        }

        public bool CanShowReminders {
            get {
                return ((AppointmentFormTemplateContainer)Parent).Control.Storage.EnableReminders;
            }
        }
        public bool ResourceSharing {
            get {
                return ((AppointmentFormTemplateContainer)Parent).Control.Storage.ResourceSharing;
            }
        }

        public IEnumerable ResourceDataSource {
            get {
                return ((AppointmentFormTemplateContainer)Parent).ResourceDataSource;
            }
        }

        public override void DataBind() {
            base.DataBind();
            AppointmentFormTemplateContainer container = (AppointmentFormTemplateContainer)Parent;
            ASPxTextBoxSubject.Text = container.Subject;
            ASPxDateEditStart.Value = container.Start;
            ASPxDateEditEnd.Value = container.End;
            ASPxMemoDescription.Text = container.Appointment.Description;

            BindComboBoxes();

            if(container.CustomFields["AppointmentCompany"] != null) {
                ASPxComboBoxCompany.Value = container.CustomFields["AppointmentCompany"];
                FillContactComboBox(container.CustomFields["AppointmentCompany"].ToString());
            }

            if(container.CustomFields["AppointmentContact"] != null) {
                ASPxComboBoxContact.Value = container.CustomFields["AppointmentContact"];
            }

            PopulateResourceEditors(container.Appointment, container);

            AppointmentRecurrenceForm1.Visible = container.ShouldShowRecurrence;

            if(container.Appointment.HasReminder) {
                cbReminder.Value = container.Appointment.Reminder.TimeBeforeStart.ToString();
                chkReminder.Checked = true;
            }
            else {
                cbReminder.ClientEnabled = false;
            }

            ASPxButtonOk.ClientSideEvents.Click = container.SaveHandler;
            ASPxButtonCancel.ClientSideEvents.Click = container.CancelHandler;
            ASPxButtonDelete.ClientSideEvents.Click = container.DeleteHandler;
        }

        private void BindComboBoxes() {
            if(Session["CompanyDataSource"] == null) {
                Session["CompanyDataSource"] = GenerateCompanyDataSource();
            }
            ASPxComboBoxCompany.DataSource = Session["CompanyDataSource"];
            ASPxComboBoxCompany.DataBind();


            if(Session["ContactDataSource"] == null) {
                Session["ContactDataSource"] = GenerateContactDataSource();
            }
            ASPxComboBoxContact.DataSource = Session["ContactDataSource"];
            ASPxComboBoxContact.DataBind();
        }

        private List<Company> GenerateCompanyDataSource() {
            List<Company> returnedResult = new List<Company>();
            for(int i = 0; i < 10; i++) {
                returnedResult.Add(new Company() { CompanyID = i, CompanyName = "Company " + i.ToString() });
            }
            return returnedResult;
        }

        private List<CompanyContact> GenerateContactDataSource() {
            List<CompanyContact> returnedResult = new List<CompanyContact>();
            List<Company> companies = Session["CompanyDataSource"] as List<Company>;

            int uniqueContactID = 0;
            for(int i = 0; i < companies.Count; i++) {
                for(int j = 0; j < 5; j++) {
                    returnedResult.Add(new CompanyContact() { CompanyID = i, ContactName = "Contact " + j.ToString() + ", Company " + i.ToString(), ContactID = uniqueContactID });
                    uniqueContactID++;
                }
            }
            return returnedResult;
        }

        protected void ASPxComboBoxContact_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e) {
            FillContactComboBox(e.Parameter);
        }

        protected void FillContactComboBox(string country) {
            if(string.IsNullOrEmpty(country)) return;

            if(Session["ContactDataSource"] != null) {
                ASPxComboBoxContact.DataSource = (Session["ContactDataSource"] as List<CompanyContact>).Where(cont => cont.CompanyID.Equals(Convert.ToInt32(country))).ToList();
                ASPxComboBoxContact.DataBind();
            }
        }

        private void PopulateResourceEditors(Appointment apt, AppointmentFormTemplateContainer container) {
            if(ResourceSharing) {
                ASPxListBox edtMultiResource = ddResource.FindControl("edtMultiResource") as ASPxListBox;
                if(edtMultiResource == null)
                    return;
                SetListBoxSelectedValues(edtMultiResource, apt.ResourceIds);
                List<String> multiResourceString = GetListBoxSeletedItemsText(edtMultiResource);
                string stringResourceNone = SchedulerLocalizer.GetString(SchedulerStringId.Caption_ResourceNone);
                ddResource.Value = stringResourceNone;
                if(multiResourceString.Count > 0)
                    ddResource.Value = String.Join(", ", multiResourceString.ToArray());
                ddResource.JSProperties.Add("cp_Caption_ResourceNone", stringResourceNone);
            }
            else {
                if(!Object.Equals(apt.ResourceId, Resource.Empty.Id))
                    edtResource.Value = apt.ResourceId.ToString();
                else
                    edtResource.Value = SchedulerIdHelper.EmptyResourceId;
            }
        }
        List<String> GetListBoxSeletedItemsText(ASPxListBox listBox) {
            List<String> result = new List<string>();
            foreach(ListEditItem editItem in listBox.Items) {
                if(editItem.Selected)
                    result.Add(editItem.Text);
            }
            return result;
        }
        void SetListBoxSelectedValues(ASPxListBox listBox, IEnumerable values) {
            listBox.Value = null;
            foreach(object value in values) {
                ListEditItem item = listBox.Items.FindByValue(value.ToString());
                if(item != null)
                    item.Selected = true;
            }
        }
        protected override void PrepareChildControls() {
            AppointmentFormTemplateContainer container = (AppointmentFormTemplateContainer)Parent;
            ASPxScheduler control = container.Control;

            AppointmentRecurrenceForm1.EditorsInfo = new EditorsInfo(control, control.Styles.FormEditors, control.Images.FormEditors, control.Styles.Buttons);
            base.PrepareChildControls();
        }

        #region #getchildeditors
        protected override ASPxEditBase[] GetChildEditors() {
            // Required to implement client-side functionality
            ASPxEditBase[] edits = new ASPxEditBase[] {
                ASPxLabelSubject, ASPxTextBoxSubject,
                ASPxLabelStart, ASPxDateEditStart,
                ASPxLabelEnd, ASPxLabelStart,
                ASPxMemoDescription,
                ASPxLabelContact, ASPxComboBoxContact,
                ASPxLabelCompany, ASPxComboBoxCompany
            };
            return edits;
        }
        #endregion #getchildeditors
        protected override ASPxButton[] GetChildButtons() {
            ASPxButton[] buttons = new ASPxButton[] {
                ASPxButtonOk, ASPxButtonCancel, ASPxButtonDelete
            };
            return buttons;
        }

        public AppointmentFormTemplateContainer GetActualContainer(object Container) {
            return Container as AppointmentFormTemplateContainer;        
        }
    }
}