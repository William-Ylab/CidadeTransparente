<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ManageBaseForm.aspx.cs" Inherits="Ferramenta.Form.ManageBaseForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .handle-sort, .question-handle-sort
        {
            cursor: move;
            background-color: #ccc;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="<%= @System.Web.Optimization.BundleTable.Bundles.ResolveBundleUrl("~/Scripts/TEditor/editor") %>" type="text/javascript"></script>
    <link href="<%= @System.Web.Optimization.BundleTable.Bundles.ResolveBundleUrl("~/Design/Styles/TEditor/editor") %>" rel="Stylesheet" type="text/css" />
    <div class="container-fluid outer">
        <div class="span12">
            <div class="box">
                <div class="control-group">
                    <table class="table table-bordered responsive">
                        <tbody>
                            <tr>
                                <td class="span2">
                                    <asp:Literal ID="Literal1" Text='<%$Resources: Label, period%>' runat="server" />
                                </td>
                                <td class="span10" colspan="2">
                                    <asp:DropDownList ID="ddlPeriods" runat="server" CssClass="span6" DataValueField="IdCrypt" DataTextField="Name">
                                    </asp:DropDownList>
                                    <a id="newPeriodLink" href="../Period/NewPeriod.aspx"><asp:Literal ID="Literal7" runat="server" Text="<%$Resources: Label, register_a_new_period%>" /></a>
                                    <div id="openPeriodMessage" class="alert-block alert-info" style="background-color: #fff; display: none">
                                        <div style="margin-left: 4px">
                                            <i class="fa fa-warning fa-lg"></i>
                                            <span><asp:Literal runat="server" Text='<%$Resources: Label, period_is_open_so_cannot_be_edited%>'></asp:Literal> </span>
                                        </div>
                                    </div>
                                    <div id="noFormInPeriod" class="alert-block alert-info" style="background-color: #fff; display: none">
                                        <div style="margin-left: 4px">
                                            <i class="fa fa-warning fa-lg"></i>
                                            <span></span>
                                        </div>
                                        <div>
                                            <button type="button" class="btn btn-primary" onclick="showNewFormName();return false"><asp:Literal ID="Literal4" runat="server" Text='<%$Resources: Label, desire_create_a_new_form%>'></asp:Literal></button>
                                            <button runat="server" id="btnDesireCopyForm" type="button" class="btn btn-primary" onclick="showCopyForm();return false"><asp:Literal ID="Literal5" runat="server" Text='<%$Resources: Label, desire_create_from_another_form%>'></asp:Literal></button>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trCopyFormBlock" style="display: none">
                                <td class="span2">
                                    <asp:Literal ID="Literal2" Text='<%$Resources: Label, copy_form_from  %>' runat="server" /></td>
                                <td class="span8">
                                    <asp:DropDownList ID="ddlPreviousForms" runat="server" DataTextField="Name" DataValueField="IdCrypt">
                                    </asp:DropDownList>
                                </td>
                                <td class="span3">
                                    <button type="button" id="btnCopyForm" class="btn btn-primary" onclick="copyForm();return false;" title='<asp:Literal runat="server" Text="<%$Resources: Label, copy_form%>" />'><i class="fa fa-copy"></i>&nbsp;<asp:Literal ID="Literal6" runat="server" Text='<%$Resources: Label, copy_form%>'></asp:Literal></button>
                                </td>
                            </tr>
                            <tr id="trNewFormName">
                                <td class="span2">
                                    <asp:Literal ID="Literal3" Text='<%$Resources: Label, question_name  %>' runat="server" /></td>
                                <td class="span9"><span id="formName"></span></td>
                                <td class="span2">
                                    <button type="button" id="btnEditFormName" class="btn edit" onclick="editFormName(this);return false;" title='<asp:Literal runat="server" Text="<%$Resources: Label, edit_form_name%>" />'><i class="fa fa-edit"></i></button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="main-form" id="blocks">
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="blockTemplate">
        <div class="span12">
            <div id="${BlockId}" class="box">
                <header>
                    <div class="icons">
                        <i class="fa fa-circle-o"></i>
                    </div>
                    <h5 style="font-size: 24px">${BlockName}</h5>
                    <div class="toolbar">
                        <a href="#table_block${BlockId}" class="accordion-toggle minimize-box" onclick="toggleAction('#table_block${BlockId}')"><i class="fa fa-chevron-up"></i></a>
                    </div>
                </header>
                <div id="table_block${BlockId}" class="body in collapse">
                    <table class="table table-condensed responsive">
                        <tbody>
                            <tr>
                                <td class="span10" colspan="2">
                                    <input type="text" id="txtBlockName${BlockId}" class="span10" placeholder='<asp:Literal runat="server" Text="<%$Resources: Label, add_new_sub_block%>" />' /></td>
                                <td class="span2">
                                    <input type="text" id="txtWeight${BlockId}" class="span1" placeholder='<asp:Literal runat="server" Text="<%$Resources: Label, weight%>" />' /></td>
                                <td class="span1">
                                    <div id="subblock${BlockId}_0">
                                        <button type="button" class="btn btn-success" onclick="saveNewSubBlock(${BlockId});return false;" title='<asp:Literal runat="server" Text="<%$Resources: Label, add_sub_block%>" />'><i class="fa fa-plus"></i></button>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="subblock-sort">
                        {{each(t, sb) SubBlocks}}
                            <table id="${sb.SubblockId}" class="table table-bordered responsive subblock" style="background-color: #fff;">
                                <thead>
                                    <tr>
                                        <th class="span1 handle-sort"><i class="fa fa-ellipsis-v fa-2x"></i><span id="spanSubBlockIndex" style="margin: 5px; font-size: 2em">${sb.SubBlockIndex}</span><input id="hdnSubBlockId" type="hidden" value="${sb.SubblockId}" /></th>
                                        <th class="span7" colspan="2" style="font-size: 18px;">${sb.SubBlockName}</th>
                                        <th class="span2">${sb.SubBlockWeight}</th>
                                        <th class="span2">
                                            <button type="button" class="btn edit" onclick="editSubBlock(this, ${$data.BlockId}, ${sb.SubblockId});return false;" title='<asp:Literal runat="server" Text="<%$Resources: Label, edit_sub_block%>" />'><i class="fa fa-edit"></i></button>
                                            <button type="button" class="btn btn-danger remove" onclick="deleteSubBlock(this, ${sb.SubblockId});return false;" title='<asp:Literal runat="server" Text="<%$Resources: Label, delete_sub_block%>" />'><i class="fa fa-trash-o"></i></button>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {{each(t, q) Questions}}
                                <tr id="${q.Id}" style="background-color: #fff" class="question${sb.SubblockId}_${q.Id}">
                                    <td colspan="2" class="question-handle-sort"><i id="question_handle_sort" class="fa fa-ellipsis-v fa-lg" style="cursor: move; width: 20px;"></i></td>
                                    <%--                                    <td>
                                        <span id="spanQuestionIndex">${q.Index}</span>
                                        <input id="hdnQuestionId" type="hidden" value="${q.Id}" />
                                    </td>--%>
                                    <td>{{html htmlDecode(q.Question)}}</td>
                                    <td>${q.Value}</td>
                                    <td class="text-center">
                                        <button type="button" class="btn edit" onclick="editQuestion(this, ${q.Id}, ${sb.SubblockId});return false;" title='<asp:Literal runat="server" Text="<%$Resources: Label, edit_question%>" />'><i class="fa fa-edit"></i></button>
                                        <button type="button" class="btn btn-danger remove" onclick="deleteQuestion(this, ${q.Id}, ${sb.SubblockId});return false;" title='<asp:Literal runat="server" Text="<%$Resources: Label, delete_question%>" />'><i class="fa fa-trash-o"></i></button>
                                    </td>
                                </tr>
                                    {{/each}}
                                <tr class="new-question">
                                    <td colspan="2"></td>
                                    <td>
                                        <%--<input type="text" id='txtNewQuestion${sb.SubblockId}' class="teditor txtNewQuestion${sb.SubblockId} span8" placeholder="Adicionar uma nova pergunta" /></td>--%>
                                        <textarea rows="8" id='txtNewQuestion${sb.SubblockId}' class="teditor txtNewQuestion${sb.SubblockId} span8" placeholder='<asp:Literal runat="server" Text="<%$Resources: Label, add_a_new_question%>" />' /></td>
                                    <td>
                                        <input type="text" id='txtNewValue${sb.SubblockId}' class="txtNewQuestion${sb.SubblockId} span1" placeholder='<asp:Literal runat="server" Text="<%$Resources: Label, weight%>" />' /></td>
                                    <td class="text-center">
                                        <div id="question${sb.SubblockId}_0" style="position: absolute">
                                            <button type="button" id="btnAddQuestion${sb.SubblockId}" class="btn btn-success" onclick="saveNewQuestion(${sb.SubblockId});return false;" title='<asp:Literal runat="server" Text="<%$Resources: Label, add_question%>" />'><i class="fa fa-plus"></i></button>
                                        </div>
                                    </td>
                                </tr>
                                </tbody>
                            </table>
                        {{/each}}
                    </div>
                </div>
            </div>
        </div>
    </script>
    <script type="text/x-jquery-tmpl" id="blockTemplateReadOnly">
        <div class="span12">
            <div class="box">
                <header>
                    <div class="icons">
                        <i class="fa fa-circle-o"></i>
                    </div>
                    <h5 style="font-size: 24px">${BlockName}</h5>
                    <div class="toolbar">
                        <a href="#table_block${BlockId}" data-toggle="collapse" class="accordion-toggle minimize-box"><i class="fa fa-chevron-up"></i></a>
                    </div>
                </header>
                <div class="body in collapse">
                    <div class="subblock-sort">
                        {{each(t, sb) SubBlocks}}
                            <table class="table table-bordered responsive subblock" style="background-color: #fff;">
                                <thead>
                                    <tr>
                                        <th class="span1 handle-sort"><i class="fa fa-ellipsis-v fa-2x"></i><span style="margin: 5px; font-size: 2em">${sb.SubBlockIndex}</span></th>
                                        <th class="span7" colspan="2" style="font-size: 18px;">${sb.SubBlockName}</th>
                                        <th class="span2">${sb.SubBlockWeight}</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {{each(t, q) Questions}}
                                        <tr style="background-color: #fff" class="question${sb.SubblockId}_${q.Id}">
                                            <td colspan="2" class="question-handle-sort"><i class="fa fa-ellipsis-v fa-lg" style="cursor: move; width: 20px;"></i></td>
                                            <td>{{html htmlDecode(q.Question)}}</td>
                                            <td>${q.Value}</td>
                                        </tr>
                                    {{/each}}
                                </tbody>
                            </table>
                        {{/each}}
                    </div>
                </div>
            </div>
        </div>
    </script>

    <script type="text/javascript">
        var blockStates = [];
        var editorSettings = {
            b: true, i: true, u: true, br: true, indent: false, source: false, center: false, color: false, fsize: false, format: false, link: false, left: false, ul: false, unlink: false,
            ol: false, outdent: false, p: false, remove: false, right: false, rule: false, sub: false, strike: false, sup: false
        };

        var _currentForm = null;

        $(document).ready(function () {
            loadCurrentForm();

            $("#<%=ddlPeriods.ClientID %>").change(function () {
                loadCurrentForm();
            });
        });

        function toggleAction(id) {
            if ($(id).css('display') == 'none') {
                $(id).css('display', "");
                blockStates.splice(id, 1);
            }else{
                $(id).css('display', "none");
                blockStates.push(id);
            }
        }

        function showNewFormName() {
            $('#trNewFormName').fadeIn();
            $('#formName').html('');
            $('#trCopyFormBlock').hide();
        }

        function showCopyForm() {
            $('#trCopyFormBlock').fadeIn();
            $('#trNewFormName').hide();
            $('#formName').html('');
        }

        function copyForm(obj) {
            var formId = $('#<%=ddlPreviousForms.ClientID%>').val();
            var periodId = $("#<%=ddlPeriods.ClientID %>").val();
            if (confirm('<asp:Literal runat="server" Text="<%$Resources: Message, will_be_create_a_new_form%>" />')) {
                $.ajax({
                    url: "/Handlers/BaseForm/Action.ashx",
                    type: 'POST',
                    data: { t: 'co', p: periodId, f: formId },
                    cache: false,
                    success: function (data) {

                        if (data == 'ok') {
                            showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, form_copied_with_success%>" />', 'success');
                            loadCurrentForm();
                        } else {
                            showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, problem_when_copied_form%>" />', 'error');
                        }
                    }
                });
            }
        }

        function loadCurrentForm() {
            var periodId = $("#<%=ddlPeriods.ClientID %>").val();

            if (periodId == "") {
                $('#newPeriodLink').show();
                $('#trNewFormName').hide();
            } else {
                $('#newPeriodLink').hide();
                $('#trNewFormName').show();
                $('#trCopyFormBlock').hide();
                $.ajax({
                    url: "/Handlers/BaseForm/Action.ashx",
                    type: 'POST',
                    data: { t: 'cf', p: periodId },
                    cache: false,
                    success: function (data) {
                        _currentForm = data;
                        bindForm();
                        $("input[id*='txtNewValue']").setMask("decimal5");
                        $("input[id*='txtWeight']").setMask("decimal5");
                        $('input[title!=""]').hint();
                    }
                });
            }

        }

        function bindForm() {
            $('#blocks').html('');
            $("#formName").html('');
            if (_currentForm != null) {
                if (_currentForm.BaseBlocks != null) {
                    $('#noFormInPeriod').hide();
                    $("#formName").html(_currentForm.FormName);

                    for (var i in _currentForm.BaseBlocks) {
                        var currentBlock = _currentForm.BaseBlocks[i];

                        if (_currentForm.PeriodStatus === false) {
                            $('#btnEditFormName').show();
                            $('#blockTemplate').tmpl(currentBlock).appendTo('#blocks');
                        } else {
                            $('#btnEditFormName').hide();
                            $('#blockTemplateReadOnly').tmpl(currentBlock).appendTo('#blocks');
                        }
                    }

                    if (_currentForm.PeriodStatus === false) {
                        $("#openPeriodMessage").hide();
                    } else {
                        $("#openPeriodMessage").fadeIn();
                    }

                } else {
                    $('#noFormInPeriod').fadeIn();
                    $('#btnEditFormName').show();
                    $("#openPeriodMessage").hide();
                    $('#trNewFormName').hide();
                    $('#trCopyFormBlock').hide();


                }
            } else {
                $("#openPeriodMessage").hide();
                $('#noFormInPeriod').hide();
            }

            $(".subblock-sort").sortable({
                tolerance: 'pointer',
                items: '.table',
                forcePlaceholderSize: true,
                handle: '.handle-sort',
                stop: function (event, ui) {
                    reOrderSubBlocks()
                },
                helper: function (e, ui) {
                    ui.children().each(function () {
                        $(this).width($(this).width());
                    });
                    return ui;
                }
            });


            $(".subblock tbody").sortable({
                placeholder: ".placeholder",
                items: 'tr:not(.new-question)',
                forcePlaceholderSize: true,
                tolerance: 'pointer',
                handle: '.question-handle-sort',
                stop: function (event, ui) {
                    reOrderQuestions($(ui.item).closest("table"))
                },
                helper: function (e, ui) {
                    ui.children().each(function () {
                        $(this).width($(this).width());
                    });
                    return ui;
                }
            });

            $('.minimize-box').on('click', function (e) {
                e.preventDefault();
                var $icon = $(this).children('i');
                if ($icon.hasClass('fa-chevron-down')) {
                    $icon.removeClass('fa-chevron-down').addClass('fa-chevron-up');
                } else if ($icon.hasClass('fa-chevron-up')) {
                    $icon.removeClass('fa-chevron-up').addClass('fa-chevron-down');
                }
            });

            $('.minimize-box').on('click', function (e) {
                e.preventDefault();
                var $icon = $(this).children('i');
                if ($icon.hasClass('fa-minus')) {
                    $icon.removeClass('fa-minus').addClass('fa-plus');
                } else if ($icon.hasClass('fa-plus')) {
                    $icon.removeClass('fa-plus').addClass('fa-minus');
                }
            });


            //Carrega os estados dos blocos
            for (var i in blockStates) {
                $(blockStates[i]).css('display', 'none');
            }
        }

        function reOrderSubBlocks() {
            var inx = 1;
            var blocks = [];
            var subBlocksIds = [];
            $('#blocks').find('.box').each(function (i, v) {
                inx = 1;
                subBlocksIds = [];

                var block = new Object();
                block.Id = $(v).attr('id');

                $(v).find('.subblock').each(function (i2, subblock) {
                    $(subblock).find('#spanSubBlockIndex').html(inx);
                    subBlocksIds.push($(subblock).attr('id'));
                    inx++;
                });

                block.SubBlocks = subBlocksIds;

                blocks.push(block);
            });

            $.ajax({
                url: "/Handlers/BaseForm/Action.ashx",
                type: 'POST',
                data: { t: 'ob', oi: $.toJSON(blocks) },
                cache: false,
                success: function (data) {
                    loadCurrentForm();
                    return false;
                }
            });
        }

        function reOrderQuestions(table) {
            var inx = 1;
            var blockId = $(table).attr('id');
            var questionsIds = [];

            $(table).find('tr:not(.new-question)').each(function (i, v) {
                questionsIds.push($(v).attr('id'));
            });

            $.ajax({
                url: "/Handlers/BaseForm/Action.ashx",
                type: 'POST',
                data: { t: 'oq', bi: blockId, oi: $.toJSON(questionsIds) },
                cache: false,
                success: function (data) {
                    loadCurrentForm();
                    return false;
                }
            });
        }

        function deleteQuestion(obj, id, blockId) {
            $.ajax({
                url: "/Handlers/BaseForm/Action.ashx",
                type: 'POST',
                data: { t: 'dq', id: id },
                cache: false,
                success: function (data) {
                    if (data == "ok") {
                        $(obj).closest('tr').remove();
                        reOrderQuestions($('#block_' + blockId));
                        showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, question_deleted_with_success%>" />', 'success');
                    } else {
                        showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, problem_when_remove_a_question%>" />', 'error');
                    }
                },
                error: function (error) {
                    showAlert(error.responseText, 'error');
                }
            });

        }

        function deleteSubBlock(obj, id) {
            $.ajax({
                url: "/Handlers/BaseForm/Action.ashx",
                type: 'POST',
                data: { t: 'db', id: id },
                cache: false,
                success: function (data) {
                    if (data == "ok") {
                        $(obj).closest('table').remove();
                        reOrderSubBlocks();
                        showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, sub_block_deleted_with_success%>" />', 'success');
                    } else {
                        showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, problem_when_remove_a_sub_block%>" />', 'error');
                    }
                },
                error: function (error) {
                    showAlert(error.responseText, 'error');
                }
            });

        }

        function editSubBlock(obj, blockId, id) {
            var tr = $(obj).parent().parent(); //tr

            var ctrlSubBlockNameId = "txtSubblockName" + id;
            var ctrlSubBlockWeightId = "txtSubblockWeight" + id;

            var tdSubblockName = tr.children("th:nth-child(2)");
            var tdSubblockWeight = tr.children("th:nth-child(3)");
            var tdButtons = tr.children("th:nth-child(4)");

            tdSubblockName.html("<input class='span8' type='text' id='" + ctrlSubBlockNameId + "' value='" + tdSubblockName.html() + "'/>" +
                                "<input type='hidden' id='cur_" + ctrlSubBlockNameId + "' value='" + tdSubblockName.html() + "'/>");
            tdSubblockWeight.html("<input type='text' style='width:40px' id='" + ctrlSubBlockWeightId + "' value='" + tdSubblockWeight.html() + "'/>" +
                                  "<input type='hidden' id='cur_" + ctrlSubBlockWeightId + "' value='" + tdSubblockWeight.html() + "'/>");

            $("#" + ctrlSubBlockWeightId).setMask("decimal5");

            var btnSave = "<button type='button' class='btn btn-success' onclick='saveCurrentSubBlock(" + blockId + "," + id + ");return false;' title='"+ '<asp:Literal runat="server" Text="<%$Resources: Label, save%>" />'+ "'><i class='fa fa-check'></i></button>";
            var btnCancel = "<button type='button' class='btn btn-danger' onclick='cancelEditSubBlock(this," + blockId + "," + id + ");return false;' title='" + '<asp:Literal runat="server" Text="<%$Resources: Label, cancel%>" />' + "'><i class='fa fa-times'></i></button>";

            tdButtons.html("<div id='subblock" + blockId + "_" + id + "'>" + btnSave + "&nbsp;" + btnCancel + "</div>");

            return false;
        }

        function cancelEditSubBlock(obj, blockId, id) {
            var ctrlSubBlockNameId = "txtSubblockName" + id;
            var ctrlSubBlockWeightId = "txtSubblockWeight" + id;

            var tr = $(obj).parent().parent().parent(); //tr
            var tdSubblockName = tr.children("th:nth-child(2)");
            var tdSubblockWeight = tr.children("th:nth-child(3)");
            var tdButtons = tr.children("th:nth-child(4)");

            tdSubblockName.html($("#cur_" + ctrlSubBlockNameId).val());
            tdSubblockWeight.html($("#cur_" + ctrlSubBlockWeightId).val());


            var buttonEdit = "<button type='button' class='btn edit' onclick='editSubBlock(this, " + blockId + "," + id + ");return false;' title='" + '<asp:Literal runat="server" Text="<%$Resources: Label, edit_sub_block%>" />' + "'><i class='fa fa-edit'></i></button>";
            var buttonRemove = "<button type='button' class='btn btn-danger remove' onclick='deleteSubBlock(this, " + id + ");return false;' title='" + '<asp:Literal runat="server" Text="<%$Resources: Label, remove_sub_block%>" />' + "'><i class='fa fa-trash-o'></i></button>";

            tdButtons.html(buttonEdit + "&nbsp;" + buttonRemove);

            return false;
        }

        function editQuestion(obj, questionId, subblockId) {
            var regex = /<br\s*[\/]?>/gi;

            var tr = $(obj).parent().parent(); //tr

            var ctrlQuestionTextId = "txtQuestionText" + questionId;
            var ctrlQuestionValueId = "txtQuestionValue" + questionId;

            var tdQuestionText = tr.children("td:nth-child(2)");
            var tdQuestionValue = tr.children("td:nth-child(3)");
            var tdButtons = tr.children("td:nth-child(4)");

            tdQuestionText.html("<div><textarea rows='8' class='span8 jqte-test' id='" + ctrlQuestionTextId + "'>" + tdQuestionText.html().replace(regex, '\n') + "</textarea>" +
                                "<input type='hidden' id='cur_" + ctrlQuestionTextId + "' value='" + tdQuestionText.html() + "'/></div>");
            tdQuestionValue.html("<input type='text' style='width:40px' id='" + ctrlQuestionValueId + "' value='" + tdQuestionValue.html() + "'/><input type='hidden' id='cur_" + ctrlQuestionValueId + "' value='" + tdQuestionValue.html() + "'/>");

            $("#" + ctrlQuestionValueId).setMask("decimal5");

            //$("#" + ctrlQuestionTextId).jqte(editorSettings);

            currentHtml = tdButtons.html();

            var btnSave = "<button type='button' class='btn btn-success' onclick='saveCurrentQuestion(" + subblockId + "," + questionId + ");return false;' title='" + '<asp:Literal runat="server" Text="<%$Resources: Label, save%>" />' + "'><i class='fa fa-check'></i></button>";
            var btnCancel = "<button type='button' class='btn btn-danger remove' onclick='return cancelEditQuestion(this," + questionId + "," + subblockId + ");return false;' title='" + '<asp:Literal runat="server" Text="<%$Resources: Label, cancel%>" />' + "'><i class='fa fa-times'></i></button>";

            tdButtons.html("<div id='question" + subblockId + "_" + questionId + "'style='position:absolute'>" + btnSave + "&nbsp;" + btnCancel + "</div>");

            return false;
        }

        function cancelEditQuestion(obj, questionId, subblockId) {
            var ctrlQuestionTextId = "txtQuestionText" + questionId;
            var ctrlQuestionValueId = "txtQuestionValue" + questionId;

            var tr = $(obj).parent().parent().parent(); //tr
            var tdQuestionText = tr.children("td:nth-child(2)");
            var tdQuestionValue = tr.children("td:nth-child(3)");
            var tdButtons = tr.children("td:nth-child(4)");

            tdQuestionText.html($("#cur_" + ctrlQuestionTextId).val());
            tdQuestionValue.html($("#cur_" + ctrlQuestionValueId).val());

            var buttonEdit = "<button type='button' class='btn btn-edit' onclick='editQuestion(this, " + questionId + ", " + subblockId + ");return false;' title='" + '<asp:Literal runat="server" Text="<%$Resources: Label, edit_question%>" />' + "'><i class='fa fa-edit'></i></button>";
            var buttonRemove = "<button type='button' class='btn btn-danger remove' onclick='deleteQuestion(this, " + questionId + ", " + subblockId + ");return false;' title='" + '<asp:Literal runat="server" Text="<%$Resources: Label, remove_question%>" />' + "'><i class='fa fa-trash-o'></i></button>";


            tdButtons.html(buttonEdit + "&nbsp;" + buttonRemove);

            return false;
        }

        function saveNewSubBlock(blockId) {
            var ctrlSubBlockNameId = "txtBlockName" + blockId;
            var ctrlSubBlockWeightId = "txtWeight" + blockId;

            saveSubBlock(blockId, 0, $("#" + ctrlSubBlockNameId).val(), $("#" + ctrlSubBlockWeightId).val());
        }

        function saveCurrentSubBlock(blockId, subblockId) {
            var ctrlSubBlockNameId = "txtSubblockName" + subblockId;
            var ctrlSubBlockWeightId = "txtSubblockWeight" + subblockId;

            saveSubBlock(blockId, subblockId, $("#" + ctrlSubBlockNameId).val(), $("#" + ctrlSubBlockWeightId).val());
        }

        function saveSubBlock(blockId, subblockId, subBlockName, subBlockWeight) {

            if (subBlockName == '') {
                showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, insert_a_name_for_sub_block%>" />', 'warning');
                return false;
            }

            if (isNaN(convertStringToFloat(subBlockWeight)) || subBlockWeight == '') {
                showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, insert_a_weight_for_sub_block%>" />', 'warning');
                return false;
            }

            if (subBlockName != "" && subBlockName != null && subBlockWeight != "" && subBlockWeight != null) {

                setLoadingToDiv(("#subblock" + blockId + "_" + subblockId));
                $.ajax({
                    url: "/Handlers/BaseForm/Action.ashx",
                    type: 'POST',
                    data: { t: 'nb', bid: blockId, sbi: subblockId, n: subBlockName, w: subBlockWeight },
                    cache: false,
                    success: function (data) {
                        if (data == 'ok') {
                            reOrderSubBlocks();

                            $('#txtBlockName' + blockId).val('');
                            $('#txtWeight' + blockId).val('');
                            if (subblockId == 0)
                                showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, sub_block_added_with_success%>" />', 'success');
                            else
                                showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, sub_block_updated_with_success%>" />', 'success');
                        } else {
                            showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, problem_to_add_a_sub_block%>" />', 'error');
                        }
                        return false;
                    },
                    error: function (error) {
                        showAlert(error.responseText, 'error');
                    },
                    complete: function (data) {
                        removeLoadingToDiv(("#subblock" + blockId + "_" + subblockId));
                    }
                });
            }
        }

        function saveNewQuestion(subblockId) {
            var ctrlNewQuestionId = "txtNewQuestion" + subblockId;
            var ctrlNewValueId = "txtNewValue" + subblockId;

            saveQuestion(subblockId, 0, $("#" + ctrlNewQuestionId).val(), $("#" + ctrlNewValueId).val());
        }

        function saveCurrentQuestion(subblockId, questionId) {
            var ctrlQuestionTextId = "txtQuestionText" + questionId;
            var ctrlQuestionValueId = "txtQuestionValue" + questionId;

            saveQuestion(subblockId, questionId, $("#" + ctrlQuestionTextId).val(), $("#" + ctrlQuestionValueId).val());
        }

        function saveQuestion(subBlockId, questionId, currentQuestion, currentValue) {

            if (currentQuestion == 'Pergunta') {
                showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, insert_a_question%>" />', 'warning');
                return false;
            }

            if (isNaN(convertStringToFloat(currentValue)) || currentValue == '') {
                currentValue = 1;
                //showAlert('O peso deve ser um número.', 'warning');
                //return false;
            }

            if (currentQuestion != "" && currentQuestion != null && currentValue != "" && currentValue != null) {
                setLoadingToDiv($("#question" + subBlockId + "_" + questionId));
                $.ajax({
                    url: "/Handlers/BaseForm/Action.ashx",
                    type: 'POST',
                    data: { t: 'nq', q: htmlEncode(currentQuestion), v: currentValue, sbi: subBlockId, qid: questionId },
                    cache: false,
                    success: function (data) {
                        if (data == 'ok') {
                            loadCurrentForm();
                            if (questionId == 0)
                                showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, question_added_with_success%>" />', 'success');
                            else
                                showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, question_updated_with_success%>" />', 'success');
                        } else {
                            showAlert('<asp:Literal runat="server" Text="<%$Resources: Message, problem_to_add_a_question%>" />', 'error');
                        }
                        return false;
                    },
                    error: function (error) {
                        showAlert(error.responseText, 'error');
                    },
                    complete: function (data) {
                        removeLoadingToDiv($("#question" + subBlockId + "_" + questionId));
                    }
                });
            }
        }

        function editFormName(obj) {
            var tr = $(obj).parent().parent(); //tr

            var ctrlFormTextId = "txtCurrentFormName";

            var tdFormText = tr.children("td:nth-child(2)");
            var tdButtons = tr.children("td:nth-child(3)");

            tdFormText.html("<input type='text' maxlength='90' class='span7' id='" + ctrlFormTextId + "' value='" + $("#formName").html() + "'/><input type='hidden' id='cur_" + ctrlFormTextId + "' value='" + $("#formName").html() + "'/>");

            var btnSave = "<button type='button' class='btn btn-success' onclick='return saveFormName(this);return false;' title='" + '<asp:Literal runat="server" Text="<%$Resources: Label, save%>" />' + "'><i class='fa fa-check'></i></button>";
            var btnCancel = "<button type='button' class='btn btn-danger remove' onclick='return cancelEditFormName(this);' title='" + '<asp:Literal runat="server" Text="<%$Resources: Label, cancel%>" />' + "'><i class='fa fa-times'></i></button>";

            tdButtons.html(btnSave + "&nbsp;" + btnCancel);

            return false;
        }

        function cancelEditFormName(obj) {
            var ctrlFormTextId = "txtCurrentFormName";

            var tr = $(obj).parent().parent(); //tr
            var tdFormText = tr.children("td:nth-child(2)");
            var tdButtons = tr.children("td:nth-child(3)");

            tdFormText.html("<span id='formName'>" + $("#cur_" + ctrlFormTextId).val() + "</span>");

            var buttonEdit = "<button type='button' class='btn btn-edit' onclick='return editFormName(this);return false;' title='" + '<asp:Literal runat="server" Text="<%$Resources: Label, edit_form_name%>" />' + "'><i class='fa fa-edit'></i></button>";

            tdButtons.html(buttonEdit);

            return false;
        }

        function saveFormName(obj) {
            var periodId = $("#<%=ddlPeriods.ClientID %>").val();
            var ctrlFormTextId = "txtCurrentFormName";
            if ($('#' + ctrlFormTextId).val() == '') {
                showAlert('O nome do questionário não pode estar em branco.', 'warning');
                return false;
            }

            $.ajax({
                url: "/Handlers/BaseForm/Action.ashx",
                type: 'POST',
                data: { t: 'sfn', n: $('#' + ctrlFormTextId).val(), p: periodId },
                cache: false,
                success: function (data) {

                    var tr = $(obj).parent().parent(); //tr
                    var tdFormText = tr.children("td:nth-child(2)");
                    var tdButtons = tr.children("td:nth-child(3)");

                    tdFormText.html("<span id='formName'>" + $("#" + ctrlFormTextId).val() + "</span>");

                    var buttonEdit = "<button type='button' class='btn btn-edit' onclick='return editFormName(this);return false;' title='" + '<asp:Literal runat="server" Text="<%$Resources: Label, edit_form_name%>" />' + "'><i class='fa fa-edit'></i></button>";

                    tdButtons.html(buttonEdit);

                    showAlert('Nome atualizado com sucesso', 'success');

                    loadCurrentForm();
                },
                error: function (error) {
                    showAlert(error.responseText, 'error');
                }
            });

            return false;
        }

        function applyBoldOnText() {
            var textComponent = document.getElementById('xpto');
            var selectedText;
            // IE version
            if (document.selection != undefined) {
                textComponent.focus();
                var sel = document.selection.createRange();
                selectedText = sel.text;
            }
                // Mozilla version
            else if (textComponent.selectionStart != undefined) {
                var startPos = textComponent.selectionStart;
                var endPos = textComponent.selectionEnd;
                selectedText = textComponent.value.substring(startPos, endPos)
            }

            if (selectedText.length > 0) {
                textComponent.innerHTML = textComponent.innerHTML.replace(htmlEncode(selectedText), "<b>" + selectedText + "</b>");
            } else {
                textComponent.innerHTML = textComponent.innerHTML + "<b></b>";
            }
        }
    </script>
</asp:Content>
