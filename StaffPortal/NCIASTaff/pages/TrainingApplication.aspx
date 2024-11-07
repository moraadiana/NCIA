<%@ Page Title="Training Application" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="TrainingApplication.aspx.cs" Inherits="NCIASTaff.TrainingApplication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Training Application</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Training Application</li>
            </ol>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-diamond"></i>New Training Application</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-remove"></i></button>
                            </div>
                        </div>
                        <div class="box-body">
                            <asp:MultiView ID="MultiView1" runat="server">
                                <asp:View ID="vwHeader" runat="server">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Staff No: </label>
                                                <asp:Label ID="lblStaffNo" runat="server" Text="" ForeColor="Blue"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Staff Name: </label>
                                                <asp:Label ID="lblStaffName" runat="server" Text="" ForeColor="Blue"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Department: </label>
                                                <asp:Label ID="lblDirectorate" runat="server" Text="" ForeColor="Blue"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Unit: </label>
                                                <asp:Label ID="lblDepartment" runat="server" Text="" ForeColor="Blue"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Location</label>
                                                <asp:DropDownList ID="ddlLocation" CssClass="form-control" runat="server">
                                                    <asp:ListItem Value="0">Local</asp:ListItem>
                                                    <asp:ListItem Value="1">International</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Individual Course</label>
                                                <asp:DropDownList ID="ddlIndividualCourse" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <asp:DropDownList ID="ddlCountry" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Supervisor</label>
                                                <asp:DropDownList ID="ddlSupervisor" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Training Category</label>
                                                <asp:DropDownList ID="ddlTrainingCategory" CssClass="form-control" runat="server">
                                                    <asp:ListItem Value="0">Individual</asp:ListItem>
                                                    <asp:ListItem Value="1">Group</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Trainer</label>
<%--                                                <asp:DropDownList ID="ddlTrainer" CssClass="form-control select2" runat="server"></asp:DropDownList>--%>
                                                <asp:TextBox ID="txtTrainer" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>                                        
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Sponsor</label>
                                                <asp:DropDownList ID="ddlSponsor" CssClass="form-control" runat="server">
                                                    <asp:ListItem Value="0">Self</asp:ListItem>
                                                    <asp:ListItem Value="1">Donor</asp:ListItem>
                                                    <asp:ListItem Value="2">Other</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>County</label>
                                                <asp:DropDownList ID="ddlCounty" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label>Purpose of Training</label>
                                                <asp:TextBox ID="txtPurpose" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:LinkButton ID="lbtnSubmit" CssClass="btn btn-success pull-right" runat="server" OnClick="lbtnSubmit_Click"><i class="fa fa-paper-plane"></i>&nbsp;Submit</asp:LinkButton>
                                        </div>
                                    </div>
                                </asp:View>
                                <asp:View ID="vwLines" runat="server">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h3>Training participants</h3>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Training Number</label><br />
                                                <asp:Label ID="lblTrainingNo" ForeColor="Blue" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Employee Code</label><br />
                                                <asp:DropDownList ID="ddlParticipants" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Objective</label><br />
                                                <asp:TextBox ID="txtObjective" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:LinkButton ID="lbtnAddParticipant" CssClass="btn btn-primary" runat="server" OnClick="lbtnAddParticipant_Click"><i class="fa fa-pencil-square"></i>&nbsp;Add</asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="Training Code" class="table table-responsive no-padding table-bordered table-hover" runat="server"
                                                AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="5">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No" SortExpression="">
                                                        <HeaderStyle Width="30px" />
                                                        <ItemTemplate>
                                                            <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Training Code" HeaderText="Training Code" />
                                                    <asp:BoundField DataField="Employee Code" HeaderText="Employee No" />
                                                    <asp:BoundField DataField="Employee name" HeaderText="Employee Name" />
                                                    <asp:BoundField DataField="Objectives" HeaderText="Objectives" />
                                                    <asp:TemplateField HeaderText="Action" SortExpression="" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemStyle Width="110px" HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnRemove" CssClass="label label-danger" runat="server" ToolTip="Click to Remove line" OnClick="lbtnRemove_Click" OnClientClick="return confirm('Are you sure you want to delete this line?')" CommandArgument='<%# Eval("Employee Code ") %>'><i class="fa fa-remove"></i> Remove</asp:LinkButton>
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
                                </asp:View>
                            </asp:MultiView>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
