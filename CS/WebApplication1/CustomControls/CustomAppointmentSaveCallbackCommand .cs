using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler.Native;

namespace WebApplication1.CustomControls {
    public class CustomAppointmentSaveCallbackCommand : AppointmentFormSaveCallbackCommand {
        public CustomAppointmentSaveCallbackCommand(ASPxScheduler control) : base(control) { }

        protected internal new CustomAppointmentFormController Controller {
            get { return (CustomAppointmentFormController)base.Controller; }
        }

        protected override System.Web.UI.Control FindControlByID(string id) {
            return FindTemplateControl(TemplateContainer, id);
        }

        System.Web.UI.Control FindTemplateControl(System.Web.UI.Control RootControl, string id) {
            System.Web.UI.Control foundedControl = RootControl.FindControl(id);
            if(foundedControl == null) {
                foreach(System.Web.UI.Control item in RootControl.Controls) {
                    foundedControl = FindTemplateControl(item, id);
                    if(foundedControl != null) break;
                }
            }
            return foundedControl;
        }

        protected override void AssignControllerValues() {
            base.AssignControllerValues();

            ASPxTextBox tbSubject = (ASPxTextBox)FindControlByID("ASPxTextBoxSubject");
            ASPxDateEdit deStart = (ASPxDateEdit)FindControlByID("ASPxDateEditStart");
            ASPxDateEdit deEnd = (ASPxDateEdit)FindControlByID("ASPxDateEditEnd");
            ASPxMemo tbDescription = (ASPxMemo)FindControlByID("ASPxMemoDescription");

            ASPxComboBox cbCompany = (ASPxComboBox)FindControlByID("ASPxComboBoxCompany");
            ASPxComboBox cbContact = (ASPxComboBox)FindControlByID("ASPxComboBoxContact");

            Controller.Subject = tbSubject.Text;
            Controller.Start = Convert.ToDateTime(deStart.Value);
            Controller.End = Convert.ToDateTime(deEnd.Value);
            Controller.Description = tbDescription.Text;
            Controller.CompanyIDField = Convert.ToInt32(cbCompany.Value);
            Controller.ContactIDField = Convert.ToInt32(cbContact.Value);

            if(Controller.ResourceId == EmptyResource.Empty) {
                Controller.ResourceId = Control.SelectedResource.Id;
            }
        }

        protected override AppointmentFormController CreateAppointmentFormController(DevExpress.XtraScheduler.Appointment apt) {
            return new CustomAppointmentFormController(Control, apt);
        }
    }
}