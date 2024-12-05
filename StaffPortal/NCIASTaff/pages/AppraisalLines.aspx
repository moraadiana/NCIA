<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="AppraisalLines.aspx.cs" Inherits="NCIASTaff.pages.AppraisalLines" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="content-wrapper">
    <section class="content-header">
        <h1>Appraisal Requisition</h1>
        <ol class="breadcrumb">
            <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Appraisal Requisition</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="vwHeader" runat="server">
                        <div class="box box-info box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">New Appraisal Requisition</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Staff No: </label>
                                            <asp:Label ID="lblStaffNo" runat="server" Text="" ForeColor="Blue"></asp:Label>
                                        </div>
                                    </div>
                                 
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Department: </label>
                                            <asp:Label ID="lblDepartment" runat="server" Text="" ForeColor="Blue"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Unit: </label>
                                            <asp:Label ID="lblDirectorate" runat="server" Text="" ForeColor="Blue"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Period: </label>
                                            <asp:Label ID="lblPeriod" runat="server" Text="" ForeColor="Blue"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Supervisor </label>
                                            <asp:DropDownList ID="ddlSupervisor" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                               
                                </div>
                               
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:LinkButton ID="lbtnSubmit" runat="server" CssClass="btn btn-primary pull-right" OnClick="lbtnSubmit_Click"><i class="fa fa-paper-plane"></i>&nbsp;Next</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </asp:View>

                    <asp:View ID="vwLines" runat="server">
                    <div class="row">
                        <!-- Header Section -->
                        <div class="col-md-12">
                            <h3 class="text-center">Activities</h3>
                        </div>

                        <!-- Grid Section -->
                        <div class="col-md-12">
                            <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="Sub-Activity Code" class="table table-striped table-bordered table-hover table-responsive" runat="server"
                                AllowSorting="True" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No">
                                        <HeaderStyle Width="30px" />
                                        <ItemTemplate>
                                            <%# string.Format("{0}",Container.DataItemIndex + 1 + ".") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                   
                                    <asp:BoundField DataField="Sub-Activity Code" HeaderText="Sub Code" SortExpression="Sub-Activity Code" />

                                    <asp:BoundField DataField="Sub-Activity Description" HeaderText="Sub Activity" SortExpression="Sub-Activity Description" />

                    
                                    <asp:TemplateField HeaderText="Performance Criteria">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCriteria" runat="server" Text="" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="Blue" Width="150px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Annual Target">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAnnualTarget" runat="server" Text="" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="Blue" Width="150px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                    
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRemarks" runat="server" Text="" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="Blue" Width="150px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <FooterStyle HorizontalAlign="Center" />

               
                                <EmptyDataTemplate>
                                    <span style="color: red; font-weight: bold;">No Records Available</span>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>

        
                        <div class="col-md-12 text-center mt-3">
                            <a href="AppraisalListing.aspx" class="btn btn-warning pull-left"><i class="fa fa-backward"></i>&nbsp;Back</a>
                           <%-- <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-danger btn-sm" OnClick="btnBack_Click" />--%>
                            <asp:Button ID="btnSendForApproval" runat="server" Text="Send for Approval" CssClass="btn btn-primary pull-right" OnClick="btnSendForApproval_Click" />
                        </div>
                    </div>
                </asp:View>

                </asp:MultiView>
            </div>
        </div>
    </section>
</div>
</asp:Content>
