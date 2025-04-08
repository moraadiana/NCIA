<%@ Page Title="Mid Year Review" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="MidYearReview.aspx.cs" Inherits="NCIASTaff.pages.MidYearReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Appraisal</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Mid Year Review</li>
            </ol>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-diamond"></i>Mid Year Review</h3>
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
                                            <h3 class="panel-title">Mid Year Review</h3>
                                        </div>

                                        <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="Sub Code" CssClass="table table-responsive no-padding table-bordered table-hover" runat="server"
                                            AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="15" OnPageIndexChanging="gvLines_PageIndexChanging">
                                            <Columns>
                                                
                                                <asp:BoundField DataField="Sub Code" HeaderText="Sub Code" />
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
                                        <div class="box box-default">
                                           <%-- <div class="box-header with-border">
                                                <h3 class="box-title"><i class="fa fa-pencil-square-o"></i>Appraisee Input</h3>
                                            </div>--%>
                                            <div class="box-body">

                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label><i class="fa fa-commenting"></i> Appraisee Comments</label>
                                                            <asp:TextBox ID="txtComments" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label><i class="fa fa-shield"></i> Mitigating Factors</label>
                                                            <asp:TextBox ID="txtMitigation" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label><i class="fa fa-tasks"></i> Additional Assignments</label>
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
                                                        <asp:LinkButton ID="btnEndYearReport" CssClass="btn btn-success btn-flat btn-sm pull-right" runat="server" OnClick="lbtnEndYearReport_Click">
                                                                 <i class="fa fa-check-circle"></i> Initiate End Year Review
                                                        </asp:LinkButton>
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
