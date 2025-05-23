﻿<%@ Page Title="PettyCash Listing (Reimbursement) " Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="PettyCashListing(Reimbursement).aspx.cs" Inherits="NCIASTaff.pages.PettyCashListing_Reimbursement_" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>PettyCash Listing (Reimbursement) </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">PettyCash Listing (Reimbursement)</li>
            </ol>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-server"></i>&nbsp;PettyCash Listing (Reimbursement) </h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="col-md-12">
                                
                                <p class="text-center"><a class="btn btn-pill btn-info u-posRelative" href="PettyCashLines(Reimbursement).aspx?query=new&status=Open">New PettyCash(Reimbursement)<span class="waves"></span> </a>
                                    <%--<a class="btn btn-pill btn-warning u-posRelative pull-right" href="LeaveStatement.aspx"><i class="fa fa-file-pdf-o"></i>My Leave Statement<span class="waves"></span> </a>--%>

                                </p>
                            </div>
                            <br />
                            <br />
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <table id="example1" class="table no-margin">
                                        <thead>
                                            <tr>
                                                 <th class="small">#</th>
                                                 <th class="small">No</th>
                                                 <th class="small">Date</th>
                                                 <th class="small">Account Name</th>              
                                                 <th class="small">Status</th>
                                                 <th class="small">Actions</th>
                                            </tr>
                                        </thead>
                                       <tbody>
                                            <%= Jobs() %>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
