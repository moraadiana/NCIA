<%@ Page Title="End Year Review" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="EndYearReview.aspx.cs" Inherits="NCIASTaff.pages.EndYearReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Appraisal</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">End Year Review</li>
            </ol>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-diamond"></i>End Year Review</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-remove"></i></button>
                            </div>
                        </div>

                        <div class="box-body">
                            <asp:MultiView ID="MultiView1" runat="server">

                                <asp:View ID="vwLines" runat="server">
                                    <div class="panel panel-warning">
                                        <div class="panel-heading">
                                            <h3 class="panel-title">End Year Review</h3>
                                        </div>

                                        <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="Sub Code" CssClass="table table-responsive no-padding table-bordered table-hover" runat="server"
                                            AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="15" OnPageIndexChanging="gvLines_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="#No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="Line No" HeaderText="Sub Code" />--%>
                                                <asp:BoundField DataField="Sub Activity" HeaderText="Sub Activity" />
                                                <asp:BoundField DataField="Performance Criteria" HeaderText="Performance Criteria" />
                                                <asp:BoundField DataField="Annual Target" HeaderText="Annual Target" />
                                                <%--<asp:BoundField DataField="Remarks" HeaderText="Remarks" />--%>
                                                <asp:TemplateField HeaderText="Self Assessment">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSelfAssessment" runat="server" CssClass="form-control" Text='<%# Eval("Self Assessment") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EmptyDataTemplate>
                                                <span style="color: red">No Records</span>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                       
                                         <div class="panel panel-warning">
    <div class="panel-heading">
        <h3 class="panel-title">Values and Competencies</h3>
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
                        <th>Description</th>
                        <th>Target Score</th>
                        <th>Final Score</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>

                        <td>
                            <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTargetScore" CssClass="form-control" runat="server"></asp:TextBox>

                        </td>
                        <td>
                            <asp:TextBox ID="txtFinalScore" CssClass="form-control" runat="server"></asp:TextBox>
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
        <asp:GridView ID="gvlines1" AutoGenerateColumns="false" DataKeyNames="Code" CssClass="table table-responsive no-padding table-bordered table-hover" runat="server"
            AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="15" OnPageIndexChanging="gvLines_PageIndexChanging">
            <Columns>
               
                <asp:BoundField DataField="Code" HeaderText="Code" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:BoundField DataField="Target Score" HeaderText="Target Score" />
                <asp:BoundField DataField="Final Score" HeaderText="Final Score" />
              
            </Columns>
            <EmptyDataTemplate>
                <span style="color: red">No Records</span>
            </EmptyDataTemplate>
        </asp:GridView>
         <div class="box box-default">
     <%-- <div class="box-header with-border">
         <h3 class="box-title"><i class="fa fa-pencil-square-o"></i>Appraisee Input</h3>
     </div>--%>
     <div class="box-body">

         <div class="row">
             <div class="col-md-4">
                 <div class="form-group">
                     <label><i class="fa fa-commenting"></i>Appraisee Comments</label>
                     <asp:TextBox ID="txtComments" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                 </div>
             </div>
             <div class="col-md-4">
                 <div class="form-group">
                     <label><i class="fa fa-shield"></i>Mitigating Factors</label>
                     <asp:TextBox ID="txtMitigation" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                 </div>
             </div>
             <div class="col-md-4">
                 <div class="form-group">
                     <label><i class="fa fa-tasks"></i>Additional Assignments</label>
                     <asp:TextBox ID="txtAssignments" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                 </div>
             </div>
         </div>
     </div>
 </div>
                                        <table class="table table-hover">
                                            <thead>

                                                <tr>
                                                    <td colspan="2">
                                                        <asp:LinkButton ID="btnback" CssClass="btn btn-sm btn-warning btn-flat pull-left" runat="server" OnClick="lbtnBack_Click">
                                                                <i class="fa fa-backward"></i> Back
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnSubmit" CssClass="btn btn-success btn-flat btn-sm pull-right" runat="server" OnClick="lbtnSubmit_Click">
                                                                <i class="fa fa-check-circle"></i> Submit
                                                        </asp:LinkButton>
                                                        <%--<asp:LinkButton ID="btnEndYearReport" CssClass="btn btn-success btn-flat btn-sm pull-right" runat="server" OnClick="lbtnEndYearReport_Click">
                                                                 <i class="fa fa-check-circle"></i> Initiate End Year Review
                                                        </asp:LinkButton>--%>
                                                    </td>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>

                                </asp:View>
                            </asp:MultiView>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
