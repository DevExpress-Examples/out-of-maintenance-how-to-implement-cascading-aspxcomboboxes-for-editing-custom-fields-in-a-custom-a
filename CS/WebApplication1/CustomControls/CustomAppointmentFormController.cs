using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler;

namespace WebApplication1.CustomControls {
    public class CustomAppointmentFormController : AppointmentFormController {
        public CustomAppointmentFormController(ASPxScheduler control, Appointment apt) : base(control, apt) {
        
        }

        public int CompanyIDField {
            get { return Convert.ToInt32(EditedAppointmentCopy.CustomFields["AppointmentCompany"]); }
            set { EditedAppointmentCopy.CustomFields["AppointmentCompany"] = value; }
        }

        public int ContactIDField {
            get { return Convert.ToInt32(EditedAppointmentCopy.CustomFields["AppointmentContact"]); }
            set { EditedAppointmentCopy.CustomFields["AppointmentContact"] = value; }
        }
    }
}