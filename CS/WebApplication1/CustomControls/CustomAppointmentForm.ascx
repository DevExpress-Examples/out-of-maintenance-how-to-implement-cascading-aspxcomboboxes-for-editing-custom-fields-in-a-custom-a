<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomAppointmentForm.ascx.cs" Inherits="WebApplication1.CustomControls.CustomAppointmentForm" %>
<%@ Register Assembly="DevExpress.Web.v15.2, Version=15.2.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v15.2, Version=15.2.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler.Controls" TagPrefix="dxsc" %>

<script id="dxss_ASPxSchedulerAppoinmentForm" type="text/javascript">
    var lastCompany = null;
    function OnCompanyChanged(s, e) {
        if (cmbContacts.InCallback())
            lastCompany = s.GetValue().toString();
        else
            cmbContacts.PerformCallback(s.GetValue().toString());
    }

    function OnContactEndCallback(s, e) {
        if (lastCompany) {
            cmbContacts.PerformCallback(lastCompany);
            lastCompany = null;
        }
    }

    function OnEdtMultiResourceSelectedIndexChanged(s, e) {
        var resourceNames = new Array();
        var items = s.GetSelectedItems();
        var count = items.length;
        if (count > 0) {
            for (var i = 0; i < count; i++)
                _aspxArrayPush(resourceNames, items[i].text);
        }
        else
            _aspxArrayPush(resourceNames, ddResource.cp_Caption_ResourceNone);
        ddResource.SetValue(resourceNames.join(', '));
    }
    function OnChkReminderCheckedChanged(s, e) {
        var isReminderEnabled = s.GetValue();
        if (isReminderEnabled)
            _dxAppointmentForm_cbReminder.SetSelectedIndex(3);
        else
            _dxAppointmentForm_cbReminder.SetSelectedIndex(-1);

        _dxAppointmentForm_cbReminder.SetEnabled(isReminderEnabled);

    }
</script>

<table class="dxscAppointmentForm" cellpadding="0" cellspacing="0" style="width: 100%; height: 230px;">
    <tr>
        <td class="dxscDoubleCell" colspan="2">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell">
                        <dx:ASPxLabel ID="ASPxLabelSubject" runat="server" Text="Subject:" />
                    </td>
                    <td class="dxscControlCell">
                        <dx:ASPxTextBox ID="ASPxTextBoxSubject" ClientInstanceName="TextBoxSubject" Width="100%" runat="server">
                        </dx:ASPxTextBox>

                    </td>
                </tr>
            </table>
        </td>
    </tr>

    <tr>
        <td class="dxscSingleCell">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell">
                        <dx:ASPxLabel ID="ASPxLabelStart" runat="server" Text="Start:" />
                    </td>
                    <td class="dxscControlCell">
                        <dx:ASPxDateEdit ID="ASPxDateEditStart" EditFormat="DateTime" ClientInstanceName="DateEditStart" Width="100%" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                </tr>
            </table>
        </td>
        <td class="dxscSingleCell">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell" style="padding-left: 25px;">
                        <dx:ASPxLabel ID="ASPxLabelEnd" runat="server" Text="End:" />
                    </td>
                    <td class="dxscControlCell">
                        <dx:ASPxDateEdit ID="ASPxDateEditEnd" EditFormat="DateTime" ClientInstanceName="DateEditStart" Width="100%" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                </tr>
            </table>
        </td>
    </tr>

    <tr>
        <td class="dxscDoubleCell" colspan="2" style="height: 90px;">
            <dx:ASPxMemo ID="ASPxMemoDescription" ClientInstanceName="TextBoxDescription" Width="100%" Height="100px" runat="server">
            </dx:ASPxMemo>
        </td>
    </tr>
    <tr>
        <% if(CanShowReminders) { %>
        <td class="dxscSingleCell" style="height: 68px">
            <% }
           else { %>
        <td class="dxscDoubleCell" colspan="2" style="height: 68px">
            <% } %>
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell">
                        <dx:ASPxLabel ID="lblResource" runat="server" AssociatedControlID="edtResource" Text="Resource:">
                        </dx:ASPxLabel>
                    </td>
                    <td class="dxscControlCell">
                        <% if(ResourceSharing) { %>
                        <dx:ASPxDropDownEdit ID="ddResource" runat="server" Width="100%" ClientInstanceName="ddResource" Enabled='<%# GetActualContainer(Container).CanEditResource %>' AllowUserInput="false">
                            <DropDownWindowTemplate>
                                <dx:ASPxListBox ID="edtMultiResource" runat="server" Width="100%" SelectionMode="CheckColumn" DataSource='<%# ResourceDataSource %>' Border-BorderWidth="0">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnEdtMultiResourceSelectedIndexChanged(s, e); }"></ClientSideEvents>
                                </dx:ASPxListBox>
                            </DropDownWindowTemplate>
                        </dx:ASPxDropDownEdit>
                        <% }
                           else { %>
                        <dx:ASPxComboBox ClientInstanceName="_dx" ID="edtResource" runat="server" Width="100%" DataSource='<%# ResourceDataSource %>' Enabled='<%# GetActualContainer(Container).CanEditResource %>' ValueType="System.String">
                        </dx:ASPxComboBox>
                        <% } %>             
                    </td>

                </tr>
            </table>
        </td>
        <% if(CanShowReminders) { %>
        <td class="dxscSingleCell" style="height: 68px">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell" style="padding-left: 22px;">
                        <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 20px; height: 20px;">
                                    <dx:ASPxCheckBox ClientInstanceName="_dx" ID="chkReminder" runat="server">
                                        <ClientSideEvents CheckedChanged="function(s, e) { OnChkReminderCheckedChanged(s, e); }" />
                                    </dx:ASPxCheckBox>
                                </td>
                                <td style="padding-left: 2px;">
                                    <dx:ASPxLabel ID="lblReminder" runat="server" Text="Reminder" AssociatedControlID="chkReminder" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="dxscControlCell" style="padding-left: 3px">
                        <dx:ASPxComboBox ID="cbReminder" ClientInstanceName="_dxAppointmentForm_cbReminder" runat="server" Width="100%" DataSource='<%# GetActualContainer(Container).ReminderDataSource %>' />
                    </td>
                </tr>
            </table>
        </td>
        <% } %>
    </tr>

    <tr>
        <td class="dxscSingleCell">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell">
                        <dx:ASPxLabel ID="ASPxLabelCompany" runat="server" Text="Company:" />
                    </td>
                    <td class="dxscControlCell">
                        <dx:ASPxComboBox runat="server" ID="ASPxComboBoxCompany" ClientInstanceName="cmbCompanies" DropDownStyle="DropDownList" IncrementalFilteringMode="StartsWith"
                            TextField="CompanyName" ValueField="CompanyID" EnableSynchronization="False" ValueType="System.Int32" Width="100%">
                            <ClientSideEvents SelectedIndexChanged="OnCompanyChanged" />
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
        <td class="dxscSingleCell">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell" style="padding-left: 25px;">
                        <dx:ASPxLabel ID="ASPxLabelContact" runat="server" Text="Contact:" />
                    </td>
                    <td class="dxscControlCell">
                        <dx:ASPxComboBox runat="server" ID="ASPxComboBoxContact" ClientInstanceName="cmbContacts" DropDownStyle="DropDownList" IncrementalFilteringMode="StartsWith"
                            TextField="ContactName" ValueField="ContactID" EnableSynchronization="False" OnCallback="ASPxComboBoxContact_Callback" ValueType="System.Int32" Width="100%">
                            <ClientSideEvents EndCallback="OnContactEndCallback" />
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<dxsc:AppointmentRecurrenceForm ID="AppointmentRecurrenceForm1" runat="server"/>
<table>
    <tr>
        <td>
            <dx:ASPxButton ID="ASPxButtonOk" ClientInstanceName="ButtonOk" runat="server" Text="Ok" Width="100" AutoPostBack="false">
            </dx:ASPxButton>

        </td>
        <td>
            <dx:ASPxButton ID="ASPxButtonCancel" ClientInstanceName="ButtonCancel" runat="server" Text="Cancel" Width="100" AutoPostBack="false">
            </dx:ASPxButton>

        </td>
        <td>
            <dx:ASPxButton ID="ASPxButtonDelete" ClientInstanceName="ButtonDelete" runat="server" Text="Delete" Width="100" AutoPostBack="false">
            </dx:ASPxButton>
        </td>
    </tr>
</table>
