﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="MidYearReviewListing.aspx.cs" Inherits="NCIASTaff.pages.MidYearReviewListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Appraisal Listing</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Appraisal Listing</li>
            </ol>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-server"></i>&nbsp;My Mid Year Review</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <div class="box-body">
                            <%--<div class="col-md-12">
                                
                                <p class="text-center"><a class="btn btn-pill btn-info u-posRelative" href="AppraisalLines.aspx?query=new&status=Open">New Appraisal<span class="waves"></span> </a>
                                   

                                </p>
                            </div>--%>
                            <br />
                            <br />
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <table id="example1" class="table no-margin">
                                        <thead>
                                            <tr>
                                                 <th class="small">#</th>
                                                 <th class="small">Appraisal No</th>
                                                <%-- <th class="small">Date</th>
                                                 <th class="small">Period</th>--%>              
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
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>