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
                                                <label>Type</label>
                                                <asp:DropDownList ID="ddlType" CssClass="form-control" runat="server">
                                                    <asp:ListItem Value="0">Local</asp:ListItem>
                                                    <asp:ListItem Value="1">Foreign</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Course</label>
                                                <asp:DropDownList ID="ddlCourse" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Application Type</label>
                                                <asp:DropDownList ID="ddlApplicationType" CssClass="form-control select2" runat="server">
                                                     <asp:ListItem Value="0">Individual</asp:ListItem>
                                                     <asp:ListItem Value="1">Group</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Training Classification</label>
                                               <asp:DropDownList ID="ddlTrainingClassification" CssClass="form-control" runat="server">
                                                    <asp:ListItem Value="0">External</asp:ListItem>
                                                    <asp:ListItem Value="1">In-house</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                  <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Training Need</label>
<%--                                                <asp:DropDownList ID="ddlTrainer" CssClass="form-control select2" runat="server"></asp:DropDownList>--%>
                                                <asp:TextBox ID="txtTrainingNeed" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>    
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Training Objective</label>
<%--                                                <asp:DropDownList ID="ddlTrainer" CssClass="form-control select2" runat="server"></asp:DropDownList>--%>
                                                <asp:TextBox ID="txtTrainingObjective" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>    
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Mode of Training</label>
                                                <asp:DropDownList ID="ddlModeofTraining" CssClass="form-control" runat="server">
                                                    <asp:ListItem Value="0">Physical</asp:ListItem>
                                                    <asp:ListItem Value="1">Virtual</asp:ListItem>
                                                   
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                         <div class="col-md-3">
                                             <div class="form-group">
                                                 <label>Dsa Vote</label>
                                                 <asp:DropDownList ID="ddlDsaVote" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                             </div>
                                         </div>
                                      </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Training Vote</label>
                                                <asp:DropDownList ID="ddlTrainingVote" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                         <div class="col-md-3">
                                             <div class="form-group">
                                                 <label>Transport Vote</label>
                                                 <asp:DropDownList ID="ddlTransportVote" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                             </div>
                                         </div>
                                       
                                         <div class="col-md-3">
                                             <div class="form-group">
                                                 <label>Start Date</label>
                                                 <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" Widt="350px" TextMode="Date" OnTextChanged="txtStartDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                 <script>
                                                     $j('#Main1_txtStartDate').Zebra_DatePicker({
                                                         direction: [1, false],
                                                         onSelect: function () {
                                                             this.trigger("change")
                                                         }
                                                     });</script>
                                             </div>
                                             </div>
                                         

                                         
                                         <div class="col-md-3">
                                             <div class="form-group">
                                                 <label>End Date</label>
                                                 <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" Widt="350px" TextMode="Date" OnTextChanged="txtEndDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                 <script>
                                                     $j('#Main1_txtEndDate').Zebra_DatePicker({
                                                         direction: [1, false],
                                                         onSelect: function () {
                                                             this.trigger("change")
                                                         }
                                                     });</script>
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
                                                <label>Course Fee</label><br />
                                                <asp:TextBox ID="txtCourseFee" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>DSA Amount</label><br />
                                                <asp:TextBox ID="txtDsaAmount" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Transport Cost</label><br />
                                                <asp:TextBox ID="txtTransportCost" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:LinkButton ID="lbtnAddParticipant" CssClass="btn btn-primary" runat="server" OnClick="lbtnAddParticipant_Click"><i class="fa fa-pencil-square"></i>&nbsp;Add</asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="Document No" class="table table-responsive no-padding table-bordered table-hover" runat="server"
                                                AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="5">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No" SortExpression="">
                                                        <HeaderStyle Width="30px" />
                                                        <ItemTemplate>
                                                            <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Document No" HeaderText="Training Code" />
                                                    <asp:BoundField DataField="Staff No" HeaderText="Employee No" />
                                                    <asp:BoundField DataField="Staff Name" HeaderText="Employee Name" />
                                                    <asp:BoundField DataField="Course Fee" HeaderText="Objectives" />
                                                    <asp:BoundField DataField="DSA Amount" HeaderText="Objectives" />
                                                    <asp:BoundField DataField="Transport Cost" HeaderText="Objectives" />
                                                    <asp:TemplateField HeaderText="Action" SortExpression="" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemStyle Width="110px" HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnRemove" CssClass="label label-danger" runat="server" ToolTip="Click to Remove line" OnClick="lbtnRemove_Click" OnClientClick="return confirm('Are you sure you want to delete this line?')" CommandArgument='<%# Eval("Staff No") %>'><i class="fa fa-remove"></i> Remove</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle HorizontalAlign="Center" />
                                                <EmptyDataTemplate>
                                                    <span style="color: red">No Recods</span>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                         <div class="col-md-12 text-center mt-3">
                                             <a href="TrainingListing.aspx" class="btn btn-warning pull-left"><i class="fa fa-backward"></i>&nbsp;Back</a>
                                            <%-- <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-danger btn-sm" OnClick="btnBack_Click" />--%>
                                             <asp:Button ID="btnSendForApproval" runat="server" Text="Send for Approval" CssClass="btn btn-primary pull-right" OnClick="btnSendForApproval_Click" />
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
