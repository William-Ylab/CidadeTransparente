(function ($) {
    $.fn.yleditable = function (settings) {
        // settings
        var config = {
            url: "",
            tooltip: 'Clique aqui para editar',
            inputType: 'text',
            inputResizable: true,
            cols: 50,
            rows: 5,
            width: 300,
            height: 50,
            showSubmitButton: true,
            showCancelButton: true,
            submit: { type: 'button', src: '', url: '', action: function () { }, imgClass: '', text: 'Salvar' },
            cancel: { type: 'button', src: '', url: '', action: function () { }, imgClass: '', text: 'Cancelar' },
            beforeEditable: function () { },
            afterEditable: function () { }
        };

        settings = $.extend({}, config, settings);

        if (settings.tooltip != "") {
            $(this).attr('title', settings.tooltip);
        }

        $(this).css('cursor', 'pointer');

        return this.click(function (e) {
            var me = this;
            if (me.editing) return;
            if (!me.editable) this.editable = function () {
                if (settings.beforeEditable)
                    settings.beforeEditable();

                me.editing = true;
                me.orgHTML = $(me).html();
                me.innerHTML = "";
                me.title = settings.tooltip;

                

                //Cria o controle text/textarea com o valor do span
                var input = __createInput(settings, me.orgHTML);

                //Cria o formulário
                var form = document.createElement("form");

                var eSubmit = __createButton(settings.submit, form);
                var eCancel = __createButton(settings.cancel, form);
                form.appendChild(input);

                if (eSubmit && settings.showSubmitButton)
                    form.appendChild(eSubmit);

                if (eCancel && settings.showCancelButton) {
                    $(eCancel).click = __reset;
                    form.appendChild(eCancel);
                    
                }

                me.appendChild(form);
                input.focus();

                $(input).blur(__reset).keydown(function (e) {
                    if (e.keyCode == 27) {
                        e.preventDefault;
                        __reset();
                    }
                });

                $(f).submit(function (e) {
                    e.preventDefault();
                    var param = {};
                    param[i.name] = $(input).val();



                    $(me).html(options.saving).load(options.url, arrayMerge(options.extraParams, p), function () {
                        // Remove script tags
                        me.innerHTML = me.innerHTML.replace(/<\s*script\s*.*>.*<\/\s*script\s*.*>/gi, "");
                        // Callback if necessary
                        if (options.callback) options.callback(me);
                        // Release
                        me.editing = false;
                    });
                });


                //Método de reset do parametro
                function __reset() {
                    me.innerHTML = me.orgHTML;
                    me.editing = false;
                    if (settings.afterEditable)
                        settings.afterEditable();
                }
            };


            //Chama a função de editable.
            this.editable();
        });
    };

    function __createButton(buttonconfig, form) {
        var i = null;
        var text = buttonconfig.text == undefined ? "" : buttonconfig.text;
        if (buttonconfig) {
            switch (buttonconfig.type) {
                case 'i':
                    i = document.createElement("i");
                    $(i).addClass(buttonconfig.imgClass);
                    break;
                case 'img':
                    i = document.createElement("img");
                    $(i).addClass(buttonconfig.imgClass);
                    i.src = buttonconfig.src;
                    i.alt = buttonconfig.text;
                    break;
                case 'submit':
                    i = document.createElement("input");
                    i.type = "submit";
                    form.action = buttonconfig.url;
                    i.title = buttonconfig.text;
                    i.value = buttonconfig.text;
                    break;
                default: //button
                    i = document.createElement("input");
                    i.type = "button";
                    i.onclick = buttonconfig.action;
                    i.title = buttonconfig.text;
                    i.value = buttonconfig.text;
            }

        }

        $(i).css('cursor', 'pointer');
        return i;
    }

    function __createInput(settings, org) {
        if (settings.inputType == "textarea") {
            var i = document.createElement("textarea");
            i.cols = settings.cols;
            i.rows = settings.rows;
            if (!settings.inputResizable)
                $(i).css('resize', 'none');
        } else {
            var i = document.createElement("input");
            i.type = "text";
            $(i).css('width', settings.width + "px");
            $(i).css('height', settings.height + "px");
        }
        $(i).val(org);
        return i;
    }

    function __submit(settings) {
        //$.ajax({
        //    url: settings.url,
        //    type: 'POST',
        //    data: ,
        //    cache: false,
        //    success: function (data) {
        //        showAlert('Nome atualizado com sucesso', 'success');
        //    },
        //    complete: function (data) {
        //        hideLoadButton('txtFormName', 'btnSaveFormName');
        //    }
        //});
    }

})(jQuery);