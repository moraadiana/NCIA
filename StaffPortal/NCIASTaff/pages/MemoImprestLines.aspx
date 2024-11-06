<%@ Page Title="Memo Imprest Lines" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="MemoImprestLines.aspx.cs" Inherits="NCIASTaff.pages.MemoImprestLines1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Memo Imprest Requisition</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Memo Imprest Requisition</li>
            </ol>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title">Memo Imprest Requisition</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-4">
                                    <label>Approved Memos</label>
                                    <asp:DropDownList ID="ddlApprovedMemos" CssClass="form-control select2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlApprovedMemos_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label>Responsibility Center</label>
                                    <asp:DropDownList ID="ddlResponsibilityCenter" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                </div>
                            </div>

                            <div id="imprestDetails" runat="server" visible="false">
                                <br />
                                <div style="width: 100%; height: 3px; background-color: #cd8969; margin: 6px 0 10px 0"></div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <label style="font-size: 1.6rem;">General and Payee Information</label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Date: </label>
                                            <asp:Label ID="lblDate" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Department: </label>
                                            <asp:Label ID="lblDirectorate" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Unit: </label>
                                            <asp:Label ID="lblDepartment" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Region: </label>
                                            <asp:Label ID="lblRegion" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Total Net Amount: </label>
                                            <asp:Label ID="lblTotalNetAmount" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Account No: </label>
                                            <asp:Label ID="lblAccountNo" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Payee: </label>
                                            <asp:Label ID="lblPayee" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Mobile No: </label>
                                            <asp:Label ID="lblMobileNo" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Job Title: </label>
                                            <asp:Label ID="lblJobTitle" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Status: </label>
                                            <asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>Purpose</label>
                                            <asp:TextBox ID="txtPurpose" CssClass="form-control" Enabled="false" TextMode="MultiLine" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div style="width: 100%; height: 3px; background-color: #cd8969; margin: 6px 0 10px 0"></div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <label style="font-size: 1.6rem;">Bank and Payment Information</label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Payee's Bank Code: </label>
                                            <asp:Label ID="lblPayeesBankCode" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Payee's Bank Name: </label>
                                            <asp:Label ID="lblPayeesBankName" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Payee's Branch Code: </label>
                                            <asp:Label ID="lblPayeesBranchCode" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Payee's Branch Name: </label>
                                            <asp:Label ID="lblPayeeBranchName" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Payee's Bank Account: </label>
                                            <asp:Label ID="lblPayeesBankAccount" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Payement Release Date: </label>
                                            <asp:Label ID="lblPaymentReleaseDate" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Payment Mode: </label>
                                            <asp:Label ID="lblPayMode" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Cheque No: </label>
                                            <asp:Label ID="lblChequeNo" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div id="attachments" runat="server">
                                    <div style="width: 100%; height: 3px; background-color: #cd8969; margin: 6px 0 10px 0"></div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>Upload supporting documents</label>
                                                <asp:FileUpload ID="fuImprestDocs" CssClass="form-control" ToolTip="Upload files with .pdf, .jpg, .png and .jpeg files only" runat="server" />
                                                <br />
                                                <asp:LinkButton ID="lbtnImprestAttach" runat="server" OnClick="lbtnImprestAttach_Click" CssClass="btn btn-primary"><i class="fa fa-upload"></i>&nbsp;Upload</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="width: 100%; height: 3px; background-color: #cd8969; margin: 6px 0 10px 0"></div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <label style="font-size: 1.6rem;">Memo Imprest Lines</label>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:GridView ID="gvLines" DataKeyNames="Advance Type" AutoGenerateColumns="false" class="table table-responsive no-padding table-bordered table-hover" runat="server"
                                            AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="100">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No" SortExpression="">
                                                    <HeaderStyle Width="30px" />
                                                    <ItemTemplate>
                                                        <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="No" HeaderText="Document No" />
                                                <asp:BoundField DataField="Advance Type" HeaderText="Advance Type" />
                                                <asp:BoundField DataField="Account No:" HeaderText="Account No" />
                                                <asp:BoundField DataField="Account Name" HeaderText="Account Name" />
                                                <asp:BoundField DataField="varAmount" HeaderText="Amount" />
                                            </Columns>
                                            <FooterStyle HorizontalAlign="Center" />
                                            <EmptyDataTemplate>
                                                <span style="color: red">No Recods</span>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div style="width: 100%; height: 3px; background-color: #cd8969; margin: 6px 0 10px 0"></div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <label style="font-size: 1.6rem;">Document Attachment</label>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:GridView ID="gvAttachments" AutoGenerateColumns="false" DataKeyNames="Document No" class="table table-responsive no-padding table-bordered table-hover" runat="server"
                                            AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="5">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No" SortExpression="">
                                                    <HeaderStyle Width="30px" />
                                                    <ItemTemplate>
                                                        <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Document No" HeaderText="Document No" />
                                                <asp:BoundField DataField="Description" HeaderText="Description" />
                                                <asp:BoundField DataField="$systemCreatedAt" HeaderText="Date Uploaded" />
                                                <asp:TemplateField HeaderText="Action" SortExpression="" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnRemoveAttach" CssClass="label label-danger" runat="server" ToolTip="Click to Remove line" OnClick="lbtnRemoveAttach_Click" OnClientClick="return confirm('Are you sure you want to delete this line?')" CommandArgument='<%# Eval("$systemId") %>'><i class="fa fa-remove"></i> Remove</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle HorizontalAlign="Center" />
                                            <EmptyDataTemplate>
                                                <span style="color: red">No Recods</span>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <a href="MemoImprestListing.aspx" class="btn btn-warning pull-left"><i class="fa fa-backward"></i>&nbsp;Back</a>
                                        <asp:LinkButton ID="lbtnApprovalRequest" runat="server" CssClass="btn btn-success pull-right" OnClick="lbtnApprovalRequest_Click">Send Approval</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
