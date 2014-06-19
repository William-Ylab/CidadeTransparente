<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Ferramenta.Form.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid outer">
        <div class="main-form">
            <div class="span12">
                <asp:Panel ID="pnlPublishTop" runat="server">
                    <button class="btn btn-success btn-tooltip" data-original-title="Aprovar questionário" onclick="openModal(true); return false;"><i class="fa fa-thumbs-o-up"></i><asp:Literal ID="Literal15" runat="server" Text='<%$Resources: Label, approve_form%>'/></button>
                    <button class="btn btn-danger remove btn-tooltip" data-original-title="Reprovar questionário" onclick="openModal(false); return false;"><i class="fa fa-thumbs-o-down"></i><asp:Literal ID="Literal16" runat="server" Text='<%$Resources: Label, repprove_form%>'/></button>
                </asp:Panel>
                <div class="box">
                    <header>
                        <div class="icons">
                            <i class="fa fa-share "></i>
                        </div>
                        <h5><asp:Literal ID="Literal1" Text='<%$Resources: Label, reviews %>' runat="server" /></h5>
                        <ul class="nav pull-right">
                            <li>
                                <a href="#reviews" data-toggle="collapse" class="accordion-toggle minimize-box">
                                    <i class="fa fa-chevron-down"></i>
                                </a>
                            </li>
                        </ul>
                    </header>

                    <div class="body collapse" id="reviews">
                        <asp:Repeater runat="server" ID="rptReviews">
                            <HeaderTemplate>
                                <table class="table table-striped responsive">
                                    <thead>
                                        <tr>
                                            <th>
                                                <asp:Literal ID="litIndex" runat="server" Text='<%$Resources: Label, username %>'></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="Literal1" runat="server" Text='<%$Resources: Label, review_date %>'></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="Literal2" runat="server" Text='<%$Resources: Label, rate %>'></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="Literal3" runat="server" Text='<%$Resources: Label, observations %>'></asp:Literal>
                                            </th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="block <%# bool.Parse(Eval("Accepted").ToString()) ? "success" : "error" %>" id='<%# Eval("Id") %>'>
                                    <td>
                                        <asp:Label ID="Label3" Text='<%# Eval("User.Name") %>' runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" Text='<%# DateTime.Parse(Eval("Date").ToString()).ToString("dd/MM/yyyy HH:mm:ss") %>' runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" Text='<%# bool.Parse(Eval("Accepted").ToString()) ? "Classificação positiva" : "Classificação negativa" %>' runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" Text='<%# Eval("Observations") %>' runat="server" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>

                </div>
                <div class="box">
                    <header>
                        <div class="icons">
                            <i class="fa fa-share "></i>
                        </div>
                        <h5><asp:Literal ID="Literal4" Text='<%$Resources: Label, submissions %>' runat="server" /></h5>
                        <ul class="nav pull-right">
                            <li>
                                <a href="#submits" data-toggle="collapse" class="accordion-toggle minimize-box">
                                    <i class="fa fa-chevron-down"></i>
                                </a>
                            </li>
                        </ul>
                    </header>

                    <div class="body collapse" id="submits">
                        <asp:Repeater runat="server" ID="rptSubmits">
                            <HeaderTemplate>
                                <table class="table table-striped responsive">
                                    <thead>
                                        <tr>
                                            <th>
                                                <asp:Literal ID="Literal1" runat="server" Text='<%$Resources: Label, submission_date %>'></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="Literal2" runat="server" Text='<%$Resources: Label, status %>'></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="Literal3" runat="server" Text='<%$Resources: Label, observations %>'></asp:Literal>
                                            </th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" Text='<%# DateTime.Parse(Eval("Date").ToString()).ToString("dd/MM/yyyy HH:mm:ss") %>' runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" Text='<%# Lib.Enumerations.EnumManager.getStringFromSubmitType((Lib.Enumerations.SubmitStatus)System.Enum.Parse(typeof(Lib.Enumerations.SubmitStatus), Eval("StatusEnum").ToString(), true)) %>' runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" Text='<%# Eval("Observation") %>' runat="server" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="box">
                    <header>
                        <div class="icons">
                            <i class="fa fa-share "></i>
                        </div>
                        <h5>
                            <asp:Literal ID="litTitle" runat="server"></asp:Literal></h5>
                        <ul class="nav pull-right">
                            <li>
                                <a href="#responseform" data-toggle="collapse" class="accordion-toggle minimize-box">
                                    <i class="fa fa-chevron-down"></i>
                                </a>
                            </li>
                        </ul>
                    </header>
                    <div class="body collapse" id="responseform">
                        <asp:Repeater runat="server" ID="rptBlocks" OnItemDataBound="rptBlocks_ItemDataBound">
                            <HeaderTemplate>
                                <ol class="ol-view">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:Label ID="Label5" Text='<%# Eval("Name") %>' runat="server" CssClass="title" />
                                    <asp:Repeater runat="server" ID="rptSubBlocks" OnItemDataBound="rptSubBlocks_ItemDataBound">
                                        <ItemTemplate>
                                            <br />
                                            <asp:Label ID="Label5" Text='<%# Eval("Name") %>' runat="server" CssClass="subtitle" />
                                            <asp:Repeater runat="server" ID="rptQuestions">
                                                <HeaderTemplate>
                                                    <table class="table table-striped responsive">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <asp:Literal ID="litIndex" runat="server" Text='<%$Resources: Message, index %>'></asp:Literal>
                                                                </th>
                                                                <th>
                                                                    <asp:Literal ID="Literal1" runat="server" Text='<%$Resources: Message, question %>'></asp:Literal>
                                                                </th>
                                                                <th>
                                                                    <asp:Literal ID="Literal2" runat="server" Text='<%$Resources: Message, score %>'></asp:Literal>
                                                                </th>
                                                                <th>
                                                                    <asp:Literal ID="Literal3" runat="server" Text='<%$Resources: Message, observation %>'></asp:Literal>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr class="block" id='<%# Eval("Id") %>'>
                                                        <td>
                                                            <asp:Label ID="Label3" Text='<%# Eval("BaseQuestion.Index") %>' runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label4" Text='<%# Eval("BaseQuestion.Question") %>' runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label1" Text='<%# Eval("Score") == null ? "N/A" : Eval("Score") %>' runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label2" Text='<%# Eval("Observation") %>' runat="server" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ol>
                            </FooterTemplate>
                        </asp:Repeater>
                        <button id="btnTracking" class="btn" onclick="$('#myModal').modal(); return false;"><i class="fa fa-exclamation"></i>&nbsp;<asp:Literal ID="Literal5" Text='<%$Resources: Label, calculation_details %>' runat="server" /></button>
                    </div>
                </div>
                <asp:Panel ID="pnlPublishBottom" runat="server">
                    <button class="btn btn-success btn-tooltip" data-original-title="Aprovar questionário" onclick="openModal(true); return false;"><i class="fa fa-thumbs-o-up"></i><asp:Literal ID="Literal6" Text='<%$Resources: Label, approve_question %>' runat="server" /></button>
                    <button class="btn btn-danger remove btn-tooltip" data-original-title="Reprovar questionário" onclick="openModal(false); return false;"><i class="fa fa-thumbs-o-down"></i><asp:Literal ID="Literal7" Text='<%$Resources: Label, reprove_question %>' runat="server" /></button>
                </asp:Panel>

                <asp:Label ID="lblSubmit" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel"><asp:Literal ID="Literal9" Text='<%$Resources: Label,  calculation_details%>' runat="server" /></h4>
                </div>
                <div class="modal-body">
                    <asp:Literal ID="litTracking" runat="server"></asp:Literal>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal"><asp:Literal ID="Literal8" Text='<%$Resources: Label, cancel %>' runat="server" /></button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="myModalSubmit" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    </div>

    <asp:HiddenField ID="hdnResponseId" runat="server" Value="0" />

    <script type="text/x-jquery-tmpl" id="tmplModalSubmit">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    {{if accepted}}
                            <h4 class="modal-title" id="H2"><asp:Literal ID="Literal10" Text='<%$Resources: Label,approve_question  %>' runat="server" /></h4>
                    {{else}}
                            <h4 class="modal-title" id="H4"><asp:Literal ID="Literal11" Text='<%$Resources: Label, reprove_question %>' runat="server" /></h4>
                    {{/if}}
                </div>
                <div class="modal-body">
                    <textarea id="txtReviewObservations" data-prompt-position="bottomLeft" style="width: 500px; height: 120px; {{if accepted == false}}margin-bottom: 30px; {{/if}}" class="{{if accepted == false}} validate[required] {{/if}}"></textarea>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal"><asp:Literal ID="Literal12" Text='<%$Resources: Label, cancel %>' runat="server" /></button>
                    <button type="button" onclick="sendObservation('${action}', ${accepted}, ${Id}); return false;" class="btn {{if accepted == true}}btn-success{{else}}btn-danger remove{{/if}}">{{if accepted == true}}<asp:Literal ID="Literal13" Text='<%$Resources: Label, approve %>' runat="server" />{{else}}<asp:Literal ID="Literal14" Text='<%$Resources: Label, reprove %>' runat="server" />{{/if}}</button>
                </div>
            </div>
        </div>
    </script>

    <script type="text/javascript">
        function openModal(accepted) {
            var obj = new Object();
            obj["accepted"] = accepted;
            obj["Id"] = $('#<%= hdnResponseId.ClientID%>').val();
            obj["action"] = 'send_observation';

            $('#myModalSubmit').html('');
            $('#tmplModalSubmit').tmpl(obj).appendTo('#myModalSubmit');

            $('#form1').validationEngine();

            $('#myModalSubmit').modal();

            $('.btn-tooltip').tooltip();
        }

        function sendObservation(action, accepted, rfId) {
            if (accepted == false || accepted == 'false') {
                if ($('#form1').validationEngine('validate') == false) {
                    return false;
                }
            }

            $.ajax({
                url: "/Handlers/ResponseForm/Action.ashx",
                type: 'POST',
                cache: false,
                data: {
                    action: action,
                    accepted: accepted,
                    observations: $('#txtReviewObservations').val(),
                    rfId: rfId
                },
                success: function (data) {
                    $('#myModal').modal('hide');
                    window.location = window.location;
                }
            });
        }

        function showQuestions(id) {
            $('.block-questions:not(#block_' + id).hide();
            $('#block_' + id).toggle();
        }

        function openTrackingModal() {
            $("#div_tracking").toggle();

            $("html, body").animate({ scrollTop: $(window).scrollTop() + $(window).height() }, 1000);
        }
    </script>
</asp:Content>
