<%@ Page Title="TransportRequisition" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="TransportRequisition.aspx.cs" Inherits="NCIASTaff.pages.TransportRequisition" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Transport Requisition</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
                <li class="active">Transport Requisition</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="vwHeader" runat="server">
                            <div class="box box-warning box-solid">
                                <div class="box-header with-border">
                                    <h3 class="box-title">New Transport Requisition</h3>
                                </div>
                                <div class="box-body">
                                    <div class="form-row">
                                       <%-- <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="memoNo">Memo No:</label>
                                                <asp:DropDownList ID="ddlMemoNo" CssClass="form-control select2" runat="server" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                        </div>--%>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="description">Description</label>
                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="Enter description" required="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="dateOfTravel">Date of Travel</label>
                                                <asp:TextBox ID="txtDateOfTravel" runat="server" CssClass="form-control" TextMode="Date" required="true" onchange="calculateReturnDate()" onfocus="setMinDate()"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="noOfDays">Number of Days</label>
                                                <asp:TextBox ID="txtNoOfDays" runat="server" CssClass="form-control" placeholder="Enter number of days" required="true" onchange="calculateReturnDate()"></asp:TextBox>
                                            </div>
                                        </div>
                                        </div>
                                        <div class="form-row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Panel runat="server" ID="panel2">
                                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Expected Return Date: "></asp:Label>
                                                    <asp:Label ID="lblReturnDate" runat="server" Font-Bold="True" ForeColor="#FF6600" CssClass="spLabel">&nbsp;</asp:Label>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                             <div class="col-md-4">
                                                 <div class="form-group">
                                                     <label>Unit: </label>
                                                     <asp:Label ID="lblUnitCode" runat="server" Text="" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                                 </div>
                                             </div> 
                                             <div class="col-md-4">
                                                 <div class="form-group">
                                                     <label>Department: </label>
                                                     <asp:Label ID="lblDepartment" runat="server" Text="" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                                 </div>
                                             </div>
                                            </div>
                                       
                                    <div class="form-row">
                                        
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="from">From:</label>
                                                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" placeholder="Commencing Place" required="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="destination">Destination</label>
                                                <asp:TextBox ID="txtDestination" runat="server" CssClass="form-control" placeholder="Enter destination" required="true"></asp:TextBox>
                                            </div>
                                        </div>
                                       
                                        </div>
                                    <div class="form-row">
                                        <div class="col-md-12">
                                      
                                        <div class="box-footer clearfix">
                                            <asp:LinkButton ID="lbtnNext" runat="server" CssClass="btn btn-sm btn-success btn-flat pull-right" OnClick="lbtnNext_Click">
                                                <i class="fa fa-send-o"></i> Next
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lbtnBack" CssClass="btn btn-sm btn-warning btn-flat pull-left" runat="server" OnClick="lbtnBack1_Click">
                                                <i class="fa fa-backward"></i> Back
                                            </asp:LinkButton>
                                        </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:View>
                        <asp:View ID="vwLines" runat="server">
                            <div class="panel panel-warning">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Passengers
                                     <asp:Label ID="lbltransportNo" runat="server" Text=""></asp:Label></h3>
                                </div>
                                <div class="panel-body">
                                    <div id="newLines" runat="server" visible="false">
                                        <asp:LinkButton ID="lbnClose" ToolTip="Close Lines" CssClass="pull-right text-danger" runat="server" OnClick="lbnClose_Click">
                                            <i class="fa fa-minus-circle"></i> Close lines
                                        </asp:LinkButton>
                                        <table class="table table-hover">
                                            <thead>
                                                <tr>
                                                    <th>Passenger No.</th>
                                                    <th>Passenger Name</th>
                                                    <th>Passenger Phone No.</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlPassengerNo" CssClass="form-control select2" runat="server" onchange="fillPassengerDetails()"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPhoneNo" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnLine" CssClass="btn btn-primary pull-right" runat="server" Text="Add" OnClick="btnLine_Click" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <asp:LinkButton ID="lbnAddLine" ToolTip="Add Passenger" CssClass="pull-right text-info" runat="server" OnClick="lbnAddLine_Click">
                                        <i class="fa fa-plus-circle"></i> Add New Passenger
                                    </asp:LinkButton>
                                    <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="PassengerNo" CssClass="table table-responsive no-padding table-bordered table-hover" runat="server"
                                        AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="15">
                                        <Columns>
                                            <asp:TemplateField HeaderText="#No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PassengerNo" HeaderText="Passenger No." />
                                            <asp:BoundField DataField="Name" HeaderText="Passenger Name" />
                                            <asp:BoundField DataField="PhoneNo" HeaderText="Passenger Phone No." />
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnCancel" CssClass="label label-danger" runat="server" ToolTip="Click to Remove line" OnClick="cancel" CommandArgument='<%# Eval("PassengerNo") %>'>
                                                        <i class="fa fa-remove"></i> Remove
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <span style="color: red">No Records</span>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:LinkButton ID="LinkButton1" CssClass="btn btn-sm btn-warning btn-flat pull-left" runat="server" OnClick="lbtnBack_Click">
                                                        <i class="fa fa-backward"></i> Back
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnSubmit" CssClass="btn btn-success btn-flat btn-sm pull-right" runat="server" OnClick="lbtnSubmit_Click">
                                                        <i class="fa fa-check-circle"></i> Submit Requisition
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </section>
    </div>

    <script>
        function calculateReturnDate() {
            var dateOfTravel = document.getElementById('<%= txtDateOfTravel.ClientID %>').value;
            var noOfDays = document.getElementById('<%= txtNoOfDays.ClientID %>').value;
            if (dateOfTravel && noOfDays) {
                var startDate = new Date(dateOfTravel);
                startDate.setDate(startDate.getDate() + parseInt(noOfDays));
                document.getElementById('<%= lblReturnDate.ClientID %>').textContent = startDate.toLocaleDateString();
            }
        }

        function setMinDate() {
            var dateOfTravel = document.getElementById('<%= txtDateOfTravel.ClientID %>');
            var today = new Date().toISOString().split('T')[0];
            dateOfTravel.setAttribute('min', today);
        }
        function fillPassengerDetails() {
            var ddl = document.getElementById('<%= ddlPassengerNo.ClientID %>');
           var selectedValue = ddl.options[ddl.selectedIndex].text;

           // Split the selected value by " - "
           var details = selectedValue.split(" - ");

           // Set txtName and txtPhoneNo based on the availability of details
           document.getElementById('<%= txtName.ClientID %>').value = details[1] ? details[1] : ''; // Set to blank if name is missing
            document.getElementById('<%= txtPhoneNo.ClientID %>').value = details[2] ? details[2] : ''; // Set to blank if phone number is missing
        }


    </script>
</asp:Content>