<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ListResponseForm.aspx.cs" Inherits="Ferramenta.Form.ListResponseForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid outer">
        <div class="main-form">
            <div class="span12">
                <div class="box">
                    <div>
                        <div style="margin-bottom: 15px;">
                            <header>
                                <div class="icons">
                                    <i class="fa fa-filter"></i>
                                </div>
                                <h5>Filtro</h5>
                                <ul class="nav pull-right">
                                    <li>
                                        <a href="#filterBlock" data-toggle="collapse" class="accordion-toggle minimize-box">
                                            <i class="fa fa-chevron-up"></i>
                                        </a>
                                    </li>
                                </ul>
                            </header>
                            <div id="filterBlock" class="body in collapse form-horizontal">
                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal Text='<%$Resources: Label, submision_status %>' runat="server" /></label>
                                    <div class="controls">
                                        <label>
                                            <input type="checkbox" class="uniform" id="chbApproved" checked="checked" /><asp:Literal ID="Literal1" Text='<%$Resources: Label, approved %>' runat="server" />
                                        </label>
                                        <label>
                                            <input type="checkbox" class="uniform" id="chbReproved" checked="checked" /><asp:Literal ID="Literal2" Text='<%$Resources: Label, reproved %>' runat="server" />
                                        </label>
                                        <label>
                                            <input type="checkbox" class="uniform" id="chbSubmitted" checked="checked" /><asp:Literal ID="Literal3" Text='<%$Resources: Label, submitted %>' runat="server" />
                                        </label>
                                    </div>
                                </div>
                                <div class="form-actions">
                                    <button id="btnFilter" class="btn btn-primary" onclick="filter(); return false;">
                                        <asp:Literal ID="Literal4" Text='<%$Resources: Label, filter %>' runat="server" /></button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="span12">
                <div class="box">
                    <header>
                        <div class="icons">
                            <i class="fa fa-filter"></i>
                        </div>
                        <h5>
                            <asp:Literal ID="Literal5" Text='<%$Resources: Label, answered_questions %>' runat="server" /></h5>
                        <ul class="nav pull-right">
                            <li>
                                <a href="#responseFormBlock" data-toggle="collapse" class="accordion-toggle minimize-box">
                                    <i class="fa fa-chevron-up"></i>
                                </a>
                            </li>
                        </ul>
                    </header>
                    <div id="responseFormBlock" class="body in collapse form-horizontal">
                        <div id="actionTable" class="body collapse in" style="height: 400px;">
                            <table class="table table-bordered responsive">
                                <thead>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                            <button id="btnPublishAll" class="btn btn-success" style="float: right;" onclick="publish(); return false;"><i class="fa fa-exclamation"></i>&nbsp;<asp:Literal ID="Literal6" Text='<%$Resources: Label, publish_selected_questions %>' runat="server" /></button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    </div>
    <script type="text/ecmascript">
        var dataNotFiltered;
        var dataFiltered;
        var type = '<%= this.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Master%>';

        $(document).ready(function () {
            loadCurrentForm();


            if ($('#actionTable tbody').find('.btn-success').length == 0) {
                $('#btnPublishAll').hide();
            } else {
                if (type == 'False') {
                    $('#btnPublishAll').hide();
                } else {
                    $('#btnPublishAll').show();
                }
            }
        });

        function loadCurrentForm() {
            $.ajax({
                url: "/Handlers/ResponseForm/GetAll.ashx",
                data: {
                    periodId: getValueByQueryString('periodId')
                },
                type: 'POST',
                cache: false,
                success: function (data) {
                    dataNotFiltered = data;
                    dataFiltered = data;

                    filter();

                    $('.btn-tooltip').tooltip();

                    if ($('#actionTable tbody').find('.btn-success').length == 0) {
                        $('#btnPublishAll').hide();
                    } else {
                        if (type == 'False') {
                            $('#btnPublishAll').hide();
                        } else {
                            $('#btnPublishAll').show();
                        }
                    }
                }
            });
        }

        function openModal(accepted, id, action) {
            var obj = new Object();
            obj["accepted"] = accepted;
            obj["Id"] = id;
            obj["action"] = action;

            $('#myModal').html('');
            $('#tmplmodal').tmpl(obj).appendTo('#myModal');

            $('#form1').validationEngine();

            $('#myModal').modal();
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
                    loadCurrentForm();
                }
            });
        }

        function filter() {
            var app = $('#chbApproved').attr('checked');
            var rep = $('#chbReproved').attr('checked');
            var sub = $('#chbSubmitted').attr('checked');

            dataFiltered = [];
            $(dataNotFiltered).each(function (i, v) {
                if (v.SubmitStatusEnum == "NotApproved") {
                    if (rep) {
                        dataFiltered.push(v);
                    }
                }

                if (v.SubmitStatusEnum == "Approved") {
                    if (app) {
                        dataFiltered.push(v);
                    }
                }

                if (v.SubmitStatusEnum == "Submitted") {
                    if (sub) {
                        dataFiltered.push(v);
                    }
                }
            });

            $('#actionTable tbody').html('');
            $('#actionTable thead').html('');

            $('#tmplhead').tmpl(dataFiltered[0]).appendTo('#actionTable thead');
            $('#tmplresponseform').tmpl(dataFiltered).appendTo('#actionTable tbody');

            $('#form1').validationEngine();
            $('.btn-tooltip').tooltip();
        }

        function redirect(id) {
            window.location = "../Form/View.aspx?rfid=" + id;

            return false;
        }

        var rfsIdsToPublish = [];
        function addRfTopublish(rfid, btn) {
            var accepted = true;

            if (btn.className.indexOf('btn-success') > -1) {
                accepted = false;
            }

            if (accepted) {
                $('#btnpublish' + rfid).find('i').removeClass('fa-square-o');
                $('#btnpublish' + rfid).find('i').addClass('fa-check-square-o');

                $(btn).addClass('btn-success');

                //se ainda não consta no array, adiciona
                if ($.inArray(rfid, rfsIdsToPublish) == -1) {
                    rfsIdsToPublish.push(rfid);
                }

            } else {
                $('#btnpublish' + rfid).find('i').removeClass('fa-check-square-o');
                $('#btnpublish' + rfid).find('i').addClass('fa-square-o');

                $(btn).removeClass('btn-success');

                //procura a posição e exclui do vetor
                var index = $.inArray(rfid, rfsIdsToPublish);
                if (index > -1) {
                    rfsIdsToPublish.splice(index, 1);
                }
            }

            if ($('#actionTable tbody').find('.btn-success').length == 0) {
                $('#btnPublishAll').hide();
            } else {
                $('#btnPublishAll').show();
            }
        }

        function publish() {
            if (rfsIdsToPublish.length > 0) {
                sendObservation('publish_all', true, rfsIdsToPublish.join());
            }
        }
    </script>
    <script type="text/x-jquery-tmpl" id="tmplhead">
        <tr>
            <th>
                <asp:Literal ID="Literal7" Text='<%$Resources: Label, username %>' runat="server" /></th>
            <th>
                <asp:Literal ID="Literal8" Text='<%$Resources: Label, usertype %>' runat="server" /></th>
            <th>
                <asp:Literal ID="Literal9" Text='<%$Resources: Label, reviews %>' runat="server" /></th>
            <th>
                <asp:Literal ID="Literal10" Text='<%$Resources: Label, rate %>' runat="server" /></th>
            <th>
                <asp:Literal ID="Literal24" Text='<%$Resources: Label, status %>' runat="server" /></th>
            <th>
                <asp:Literal ID="Literal11" Text='<%$Resources: Label, show %>' runat="server" /></th>
            {{if UserIsMaster == true}}
            <th>
                <asp:Literal ID="Literal12" Text='<%$Resources: Label, publish %>' runat="server" /></th>
            {{/if}}
        </tr>
    </script>
    <script type="text/x-jquery-tmpl" id="tmplresponseform">
        <tr>
            <td><a href="/User/NewUser.aspx?id=${UserId}">${UserName}</a></td>
            <td>${UserType}</td>
            <td>{{if Reviews.length > 0}}
                    <span class="btn-tooltip" data-html="true" data-placement="bottom" data-original-title="{{each PositiveReviews}}${UserName} <br/>{{/each}}" style="cursor: pointer; font-size: 25px; color: green">${PositiveReviews.length} <i class="fa fa-thumbs-o-up"></i></span>
                <span class="btn-tooltip" data-html="true" data-placement="bottom" data-original-title="{{each NegativeReviews}}${UserName} <br/>{{/each}}" style="cursor: pointer; font-size: 25px; float: right; color: red">${NegativeReviews.length} <i class="fa fa-thumbs-o-down"></i></span>
                {{else}}
                    <asp:Literal ID="Literal13" Text='<%$Resources: Label, no_reviews %>' runat="server" />
                {{/if}}
            </td>
            <td>{{if UserAlreadyReview}}
                <asp:Literal ID="Literal14" Text='<%$Resources: Label, already_rated %>' runat="server" />
                {{else}}
                    {{if SubmitStatusEnum == "Approved"}}
                            <asp:Literal ID="Literal23" Text='<%$Resources: Label, selected_to_published %>' runat="server" />
                {{else}}
                        <button class="btn btn-success btn-tooltip" data-original-title="Aprovar" onclick="openModal(true, ${Id}, 'send_observation'); return false;"><i class="fa fa-thumbs-o-up"></i></button>
                <button class="btn btn-danger remove btn-tooltip" data-original-title="Reprovar" onclick="openModal(false, ${Id}, 'send_observation'); return false;"><i class="fa fa-thumbs-o-down"></i></button>
                {{/if}}
                {{/if}}
            </td>
            <td>
                <span>${SubmitStatus}</span>
            </td>
            <td>
                <button class="btn" onclick="return redirect(${Id});"><i class="fa fa-search"></i></button>
            </td>
            {{if UserIsMaster == false}}
                <td style="display: none">
            {{else}}
                <td>{{/if}}
            {{if SubmitStatusEnum == "Approved"}}
                <asp:Literal ID="Literal15" Text='<%$Resources: Label, selected_to_published %>' runat="server" />
                    <%--<button id="btnpublish${Id}" class="btn btn-success" onclick="addRfTopublish(${Id}, this); return false;"><i class="fa fa-check-square-o"></i></button>--%>
            {{else}}
                <button id="btnpublish${Id}" class="btn" onclick="addRfTopublish(${Id}, this);  return false;"><i class="fa fa-square-o"></i></button>
                    {{/if}}
                </td>
        </tr>
    </script>
    <script type="text/x-jquery-tmpl" id="tmplmodal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>

                    {{if accepted}}
                        {{if action == "send_observation"}}
                            <h4 class="modal-title" id="H1">
                                <asp:Literal ID="Literal16" Text='<%$Resources: Label, approve_question %>' runat="server" /></h4>
                    {{else}}
                            <h4 class="modal-title" id="H2">
                                <asp:Literal ID="Literal17" Text='<%$Resources: Label, publish_question %>' runat="server" /></h4>
                    {{/if}}
                    
                    {{else}}
                            <h4 class="modal-title" id="H3">
                                <asp:Literal ID="Literal18" Text='<%$Resources: Label, reprove_question %>' runat="server" /></h4>
                    {{/if}}
                </div>
                <div class="modal-body">
                    <textarea id="txtReviewObservations" placeholder="Adicione aqui o que deve ser revisado" data-prompt-position="bottomLeft" style="width: 500px; height: 120px; {{if accepted == false}}margin-bottom: 30px; {{/if}}" class="{{if accepted == false}} validate[required] {{/if}}"></textarea>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        <asp:Literal ID="Literal19" Text='<%$Resources: Label, cancel %>' runat="server" /></button>
                    <button type="button" onclick="sendObservation('${action}', ${accepted}, ${Id}); return false;" class="btn {{if accepted == true}}btn-success{{else}}btn-danger remove{{/if}}">{{if accepted == true}}{{if action == "send_observation"}}<asp:Literal ID="Literal20" Text='<%$Resources: Label, approve %>' runat="server" />{{else}}<asp:Literal ID="Literal21" Text='<%$Resources: Label, publish %>' runat="server" />{{/if}}{{else}}<asp:Literal ID="Literal22" Text='<%$Resources: Label, reprove %>' runat="server" />{{/if}}</button>
                </div>
            </div>
        </div>
    </script>
</asp:Content>
