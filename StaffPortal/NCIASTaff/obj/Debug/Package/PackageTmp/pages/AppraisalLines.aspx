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
                            <div class="panel panel-warning">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Activities</h3>
                                </div>
                                <div class="panel-body">
                                    <div id="newLines" runat="server" visible="false">

                                        <asp:LinkButton ID="lbnClose" ToolTip="Close Lines" CssClass="pull-right text-danger" runat="server" OnClick="lbnClose_Click">
                                                    <i class="fa fa-minus-circle"></i> Close lines
                                        </asp:LinkButton>
                                        <table class="table table-hover">
                                            <thead>
                                                <tr>
                                                    <%-- <th>Sub Code</th>--%>
                                                    <th>Sub Activity</th>
                                                    <th>Performance Creteria</th>
                                                    <th>Annual Target</th>
                                                    <th>Remarks</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>

                                                    <td>
                                                        <asp:TextBox ID="txtSubActivity" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPerformanceCriteria" CssClass="form-control" runat="server"></asp:TextBox>

                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAnnualTarget" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemarks" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnLine" CssClass="btn btn-primary pull-right" runat="server" Text="Add" OnClick="btnLine_Click" />
                                                    </td>

                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <asp:LinkButton ID="lbnAddLine" ToolTip="Add Line" CssClass="pull-right text-info" runat="server" OnClick="lbnAddLine_Click">
                                                <i class="fa fa-plus-circle"></i> Add New Line
                                    </asp:LinkButton>
                                    <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="Sub Code" CssClass="table table-responsive no-padding table-bordered table-hover" runat="server"
                                        AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="15" OnPageIndexChanging="gvLines_PageIndexChanging">
                                        <Columns>
                                            <%--<asp:TemplateField HeaderText="#No">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>  --%>
                                            <asp:BoundField DataField="Sub Code" HeaderText="Sub Code" />
                                            <asp:BoundField DataField="Sub Activity" HeaderText="Sub Activity" />
                                            <asp:BoundField DataField="Performance Criteria" HeaderText="Performance Criteria" />
                                            <asp:BoundField DataField="Annual Target" HeaderText="Annual Target" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />

                                            <%-- <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnCancel" CssClass="label label-danger" runat="server" ToolTip="Click to Remove line" OnClick="cancel" CommandArgument='<%# Eval("Sub Code") %>'>
                                                                <i class="fa fa-remove"></i> Remove
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <span style="color: red">No Records</span>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <td colspan="2">

                                                    <a href="AppraisalListing.aspx" class="btn btn-warning pull-left"><i class="fa fa-backward"></i>&nbsp;Back</a>
                                                    <asp:Button ID="btnSendForApproval" runat="server" Text="Send for Approval" CssClass="btn btn-primary pull-right" OnClick="btnSendForApproval_Click" />

                                                    <asp:LinkButton ID="btnMidYearReport" CssClass="btn btn-success btn-flat btn-sm pull-right" runat="server" OnClick="lbtnMidYearReport_Click">
                                                                 <i class="fa fa-check-circle"></i> Initiate Mid Year Review
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
</asp:Content>
