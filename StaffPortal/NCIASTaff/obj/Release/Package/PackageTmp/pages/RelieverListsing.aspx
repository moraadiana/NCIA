<%@ Page Language="C#" Title="Leave Listing" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="RelieverListsing.aspx.cs" Inherits="NCIASTaff.pages.RelieverListsing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Leave Listing
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Leave Listing</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-server"></i> My Leave Listing</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <br/>
                            <br/>
                            <br/>
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <table id="example1" class="table no-margin">
                                        <thead>
                                            <tr>
                                                <th class="small">#</th>
                                                <th class="small">Ref No</th>
                                                <th class="small">Leave Type</th>
                                                <th class="small">Applicant</th>
                                                <th class="small">Days applied</th>
                                                <th class="small">Application Date</th>
                                                <th class="small">Start</th>
                                                <th class="small">End</th>
                                                <th class="small">Return</th>
                                                <th class="small">Reliever's Action</th>
                                                <th class="small">Status</th>
                                                <th class="small">Actions</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <%=Jobs()%>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <!-- /.table-responsive -->
                        </div>
                        <!-- /.box-body -->
                        <%--  <div class="box-footer clearfix">
                            <a href="javascript:void(0)" class="btn btn-sm btn-info btn-flat pull-left">Place New Order</a>
                            <a href="javascript:void(0)" class="btn btn-sm btn-default btn-flat pull-right">View All Orders</a>
                        </div>--%>
                        <!-- /.box-footer -->
                    </div>
                </div>
                <!-- /.col -->
            </div>
        </section>
        <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
</asp:Content>

