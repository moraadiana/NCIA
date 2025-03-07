<%@ Page Title="StaffExit" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="StaffExit1.aspx.cs" Inherits="NCIASTaff.pages.StaffExit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">

    <div class="content-wrapper">
        <section class="content-header">
            <h1>Staff Exit Application</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Staff Exit Application</li>
            </ol>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-diamond"></i>Staff Exit Application</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-remove"></i></button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="row">
                               <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Staff Number: </label>
                                        <asp:Label ID="lblEmpNo" runat="server" Text="" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                    </div>
                                </div>
                               <div class="col-md-3">
                                     <div class="form-group">
                                         <label>Employee Name: </label>
                                         <asp:Label ID="lblEmpName" runat="server" Text="" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                     </div>
                                 </div>
                                <div class="col-md-3">
                                     <div class="form-group">
                                         <label>Department: </label>
                                         <asp:Label ID="lblDepartment" runat="server" Text="" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                     </div>
                                 </div>
                               
                                <div class="col-md-3">
                                     <div class="form-group">
                                         <label>Designation: </label>
                                         <asp:Label ID="lblDesignation" runat="server" Text="" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                     </div>
                                 </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label> Date of Leaving</label>
                                        <asp:TextBox ID="txtleavingDate" CssClass="form-control" runat="server" Widt="350px" TextMode="Date"  AutoPostBack="true"></asp:TextBox>
                                        
                                    </div>
                                </div>
                                
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Nature of living</label>
                                        
                                        <asp:DropDownList ID="ddlNatureofLeaving" CssClass="form-control" runat="server">
                                            <asp:ListItem Value="0">Retired</asp:ListItem>
                                            <asp:ListItem Value="1">Resigned</asp:ListItem>
                                            <asp:ListItem Value="3">Dismissed</asp:ListItem>
                                            <asp:ListItem Value="4">Discharged</asp:ListItem>
                                            <asp:ListItem Value="5">Deceased</asp:ListItem>
                                            <asp:ListItem Value="6">Contract Expiry</asp:ListItem>
                                            <asp:ListItem Value="7">Study Leave</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label>Reason</label>
                                        <asp:TextBox ID="txtReason" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <a href="StaffExitListing.aspx" class="btn btn-warning pull-left"><i class="fa fa-backward"></i>&nbsp;Back</a>
                                    <asp:LinkButton ID="lbtnSubmit" CssClass="btn btn-success pull-right" runat="server" OnClick="lbtnSubmit_Click"><i class="fa fa-paper-plane"></i>&nbsp;Submit</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
