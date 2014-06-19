/*Modal feita por Yellow Lab*/
//seleciona os elementos a com atributo name="modal"
$('a[name=modal]').click(function (e) {
    //cancela o comportamento padrão do link, que é o redirecionamento
    e.preventDefault();

    //Recupera a url para abrir
    var url = $(this).attr('href');

    if (url == '#')
        return; //Não abre nenhum modal

    //Recupera a largura e a altura da tela
    var winH = $(window).height();
    var winW = $(window).width();
    var docHeight = $(document).height();

    //Define largura e altura do div#mask iguais ás dimensões da tela
    $('#yl_modal_bg').css({ 'width': winW, 'height': docHeight });

    //Transição do BackGround
    $('#yl_modal_bg').fadeIn(200);
    $('#yl_modal_bg').fadeTo("slow", 0.8);

    $('#yl_modal .yl-modal-window .yl-modal-content').load(url, function () {
        //centraliza na tela a janela popup
        $('#yl_modal .yl-modal-window').css('top', winH / 2 - $('#yl_modal .yl-modal-window').height() / 2);
        $('#yl_modal .yl-modal-window').css('left', winW / 2 - $('#yl_modal .yl-modal-window').width() / 2);
    });

    //efeito de transição
    $('#yl_modal .yl-modal-window').fadeIn(2000);
});

$('#yl_modal .yl-modal-close').click(function (e) {
    //cancela o comportamento padrão do link, que é o redirecionamento
    e.preventDefault();
    $('#yl_modal .yl-modal-window .yl-modal-content').empty();
    $('#yl_modal .yl-modal-window').hide();
    $('#yl_modal_bg').removeAttr('style');
    $('#yl_modal_bg').hide();
});

$('#yl_modal_bg').click(function () {
    $(this).removeAttr('style');
    $(this).hide();
    $('#yl_modal .yl-modal-window .yl-modal-content').html('');
    $('#yl_modal .yl-modal-window').hide();
});