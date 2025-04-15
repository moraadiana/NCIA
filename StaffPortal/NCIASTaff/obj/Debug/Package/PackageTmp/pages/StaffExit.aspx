<%@ Page Title="StaffExit" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="StaffExit.aspx.cs" Inherits="NCIASTaff.pages.StaffExit1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="content-wrapper">
    <section class="content-header">
        <h1>Staff Clearance</h1>
        <ol class="breadcrumb">
            <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Staff Clearance</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="vwHeader" runat="server">
                        <div class="box box-info box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">Staff Clearance</h3>
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

                    </asp:View>

                    <asp:View ID="vwLines" runat="server">
                    <div class="row">
                        <!-- Header Section -->
                        <div class="col-md-12">
                            <h3 class="text-center">Clearance Lines</h3>
                        </div>

                        <!-- Grid Section -->
                        <div class="col-md-12">
                            <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="Cleared By" class="table table-striped table-bordered table-hover table-responsive" runat="server"
                                AllowSorting="True" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No">
                                        <HeaderStyle Width="30px" />
                                        <ItemTemplate>
                                            <%# string.Format("{0}",Container.DataItemIndex + 1 + ".") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                   
                                    <asp:BoundField DataField="Cleared By" HeaderText="Cleared By" SortExpression="Cleared By" />

                                    <asp:BoundField DataField="Clearer Name" HeaderText="Clearer Name" SortExpression="Clearer Name" />

                                    <asp:BoundField DataField="Department Code" HeaderText="Department Code" SortExpression="Department Codet" />

                                    <asp:BoundField DataField="Designation" HeaderText="Designation" SortExpression="Designation" />

                                    <asp:BoundField DataField="Cleared" HeaderText="Cleared" SortExpression="Cleared" />

                                     <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" />
                                     
                                     <asp:BoundField DataField="Date Cleared" HeaderText="Date Cleared" SortExpression="Date Cleared" />
                    
                                   
                                </Columns>

                                <FooterStyle HorizontalAlign="Center" />

               
                                <EmptyDataTemplate>
                                    <span style="color: red; font-weight: bold;">No Records Available</span>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>

        
                        <div class="col-md-12 text-center mt-3">
                            <a href="StaffExitListing.aspx" class="btn btn-warning pull-left"><i class="fa fa-backward"></i>&nbsp;Back</a>
                           
                            <%--<asp:Button ID="btnSendForApproval" runat="server" Text="Send for Approval" CssClass="btn btn-primary pull-right" OnClick="btnSendForApproval_Click" />--%>
                        </div>
                    </div>
                </asp:View>

                </asp:MultiView>
            </div>
        </div>
    </section>
</div>
</asp:Content>
