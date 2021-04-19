$(document).ready(function () {

    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    });
});
$(document).on("click", "#contentPager a[href]", function () {

    $.ajax({
        url: $(this).attr("href"),
        type: 'GET',
        cache: false,
        dataType: 'html',
        success: function (result) {

            $('#content').html(result);


        }
    });
    return false;
});
$(document).on('click', '#menu_toggle', function () {

    $("#link-toggle").slideToggle();

}).on('click', '#menu-toggle-admin', function () {

    $(".dropdown_menu").slideToggle();

});

//$(document).ready(function () {
//    marcar_ordenacao_campo($('#table_grid thead tr th:nth-child(1) span'));
//}).on('click', '.coluna-ordenacao', function () {
//    marcar_ordenacao_campo($(this));

//    var ordem = obter_ordem_grid(),
//        tamanhoPag = $("#tamanhoPag").val(),
//        pagina = viewBagPagina,
//        url = url_index,
//        param = { 'pesquisa': $("#pesquisa").val(), 'pagina': pagina, 'tamanhoPag': tamanhoPag, 'ordem':ordem };

//    $.get(url, param, function (resposta) {
//        if (resposta) {
//            var table = $("#table_grid").find('#tbody_grid');
//            table.empty();
//            if (resposta.length > 0) {

//                recarregarLista(resposta);
//            }
//            else {
//                $("#lista").addClass('hide');
//                $("#mensagem").removeClass('hide');
//            }

//        }
//    });
//});

function marcar_ordenacao_campo(coluna) {
    var ordem_crescente = true,
        ordem = coluna.find('i');

    if (ordem.length > 0) {
        ordem_crescente = ordem.hasClass('bi bi-arrow-down-short');
        if (ordem_crescente) {
            ordem.removeClass('bi bi-arrow-down-short');
            ordem.addClass('bi bi-arrow-up-short');
        }
        else {
            ordem.removeClass('bi bi-arrow-up-short');
            ordem.addClass('bi bi-arrow-down-short');
        }
    } 
    else {
        $('.coluna-ordenacao i').remove();
        coluna.append('&nbsp;<i class="bi bi-arrow-down-short" style="color:#000000"></i>');
    }
}


var formId = document.getElementById("formDados");
var send = $("#btnGravar");
$(formId).submit(function (event) {
    if (formId.checkValidity()) {
        send.attr('disabled', 'disabled');
        send.val("Enviando...");
    }
});

$(".nav li").click(function (e) {

    $(this).siblings('li').removeClass('active');
    $(this).addClass('active');

});

//$("#DataNascimento").mask("99/99/9999");


function mascara(i) {

    var v = i.value;

    if (isNaN(v[v.length - 1])) { // impede entrar outro caractere que não seja número
        i.value = v.substring(0, v.length - 1);
        return;
    }

    i.setAttribute("maxlength", "14");
    if (v.length == 3 || v.length == 7) i.value += ".";
    if (v.length == 11) i.value += "-";

}


function fecharModal() {
    $("#mostrarModal").parents('.bootbox').modal('hide');

}




function ShowImagePreview(imageUploader, previewImage) {

    if (imageUploader.files && imageUploader.files[0]) {

        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}



//Scripts da Controller Encaminhamento
$(document).ready(function () {

    //$("#pesquisa").keyup(function () {
    //    var url = $("#form0").delay(1000).submit();
    //    $.ajax({
    //        type: "GET",
    //        url: url,
    //        cache: false,

    //        success: function (result) {

    //            $('#tbody').html(result);

    //        }
    //    });
    //    return false;
    //});

    $(function () {
        $('[data-toggle="tooltip"]').tooltip("show");
    });


}).on('change', '#tamanhoPag', function () {
    var selectTamPag = $(this),
        //ordem = obter_ordem_grid(),
        tamanhoPag = selectTamPag.val(),
        pagina = viewBagPagina,
        url = url_index,
        pesquisa = $("#pesquisa").val(),
        param = { 'pesquisa': pesquisa, 'pagina': pagina, 'tamanhoPag': tamanhoPag/*, 'ordem': ordem*/ };

    $.get(url, param, function (resposta) {
        if (resposta) {
            recarregarLista(resposta);
            selectTamPag.siblings().removeClass('active');
            selectTamPag.addClass('active');
        }
    });


}).on('keyup', '#pesquisa', function () {
    var pesquisa = $(this),
        //ordem = obter_ordem_grid(),
        selectTamPag = $("#tamanhoPag").val(),
        tamanhoPag = selectTamPag,
        pagina = viewBagPagina,
        url = url_index,
        param = { 'pesquisa': pesquisa.val(), 'pagina': pagina, 'tamanhoPag': tamanhoPag/*, 'ordem': ordem*/ };

    $.get(url, param, function (resposta) {
        if (resposta) {
            var table = $("#table_grid").find('tbody');
            table.empty();
            if (resposta.length > 0) {

                recarregarLista(resposta);
            }
            else {
                $("#lista").addClass('hide');
                $("#mensagem").removeClass('hide');
            }

        }
    });
});
//    .on('click', '.link', function (e) {
//    e.preventDefault();
//    var paginaRef = $(this).attr('href');

//    chamarPage(paginaRef);
//});

function chamarPage(pagRef) {

    data = { 'tamanhoPag': 5, 'pagina': 1, 'pesquisa': '' };

    $.ajax({
        url: pagRef,
        type: "GET",
        data:data,
        dataType: "html",
        cache: false,
        async: true,

        success: function (response) {
            $('.container_div').html(response);
        },
        error: function (error) {
            console.log('A Pagina não foi carregada', error);
        }
    })
}

function obter_ordem_grid() {
    var colunas_grid = $('.coluna-ordenacao'),
        ret = '';

    colunas_grid.each(function (index, item) {
        var coluna = $(item),
            ordem = coluna.find('i');

        if (ordem.length > 0) {
            ordem_crescente = ordem.hasClass('bi bi-arrow-down-short');
            ret = (ordem_crescente ? '' : 'desc');
            return true;
        }
    });
    return ret;
}

function abrir_form(dados, titulo, tamanho) {
    var mostrarModal = $("#mostrarModal");
    $("#msg_aviso").hide();
    $("#msg_erro").hide();
    $("#msg_mensagem_aviso").hide();
    $("#formDados").html(dados);

    bootbox.dialog({
        title: titulo,
        message: mostrarModal,
        size: tamanho
    })

        .on('shown.bs.modal', function () {
            mostrarModal.show(0, function () {
                $("#UserName").focus();
                $("#Nome").focus();
                $("#TipoEncaminhamento").focus();
            });

        })
        .on('hidden.bs.modal', function () {
            mostrarModal.hide().appendTo('body');
            $(this).removeData('bs.modal');
            $(".modal-body").empty();

        });
}
function abrir_form_detalhes(dados, titulo, tamanho) {
    var mostrarModal = $("#mostrarModal");
    $("#msg_aviso").hide();
    $("#msg_erro").hide();
    $("#msg_mensagem_aviso").hide();
    $("#formDados").html(dados);

    bootbox.dialog({
        title: titulo,
        message: mostrarModal,
        size: tamanho
    })

        .on('shown.bs.modal', function () {
            mostrarModal.show(0, function () {
                $("#btnGravar").hide();
                $("#cancelar").html('Sair');
            });

        })
        .on('hidden.bs.modal', function () {
            mostrarModal.hide().appendTo('body');
            $(this).removeData('bs.modal');
            $(".modal-body").empty();
            $("#btnGravar").show();
            $("#cancelar").html('Cancelar');
        });
}

function formatar_msg_aviso(mensagens) {
    var ret = '';

    for (var i = 0; i < mensagens.length; i++) {
        ret += '<li>' + mensagens[i] + '</li>';
    }

    return '<ul>' + ret + '</ul>';
}

function exibirAviso(Mensagens) {
    $("#msg_mensagem_aviso").html(formatar_msg_aviso(Mensagens));
    $("#msg_aviso").show();
    $("#msg_mensagem_aviso").show();
    $("#msg_erro").hide();
}
function exibirErro() {
    $("#msg_erro").show();
    $("#msg_aviso").hide();
    $("#msg_mensagem_aviso").hide();    
}


function preencherFormExcluir() {
    //$("#myModal2").modal("show");
    $(".modal-body").empty();
    $(".modal-title").html("Excluir Pessoa");
    $(".modal-body").html('<p>Tem certeza que deseja excluir?</p>');
    $(".modal-footer").html('<input type="submit" class="btn btn-success" id="btnExcluirPessoa" onclick="" value="Excluir">');
    $(".modal-footer").append('<button class="btn btn-default" data-dismiss="modal" id="cancelar">Cancelar</button>');
}

function limparForm() {
    $("#formDados").each(function () {
        this.reset();
    });
}

function Lista(link) {
    //$("#table_grid tbody tr").remove();
    var url = link,
        //ordem = obter_ordem_grid(),
        tamanhoPag = $("#tamanhoPag").val(),
        pagina = viewBagPagina,
        pesquisa = $("#pesquisa").val(),
        data = { 'tamanhoPag': tamanhoPag, 'pagina': pagina, 'pesquisa': pesquisa/*, 'ordem': ordem*/ };

    $.ajax({
        type: "Get",
        url: url,
        data: data,
        dataType: "html",
        cache: false,
        async: true,
        success: function (response) {
            console.log(response.Nome);
            $("#lista").html(response);
        },
        error: function () {
            $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
                hideDuration: 400,
            });
        }
    });

}

function recarregarLista(resposta) {
    //$("#tbody_grid").empty();
    $("#lista").html(resposta);
}

function add_anti_csrf(data) {
    data.__RequestVerificationToken = $('[name="__RequestVerificationToken"]').val();
    return data;
}

function verificarGrid() {
    var quant = $('#table_grid > tbody > tr').length;
    if (quant == 0) {
        $('#table_grid').addClass('hide');
        $('#mensagem').removeClass('hide');

    }
}

function unserialize(serializedData) {
    var urlParams = new URLSearchParams(serializedData); // get interface / iterator
    unserializedData = {}; // prepare result object
    for ([key, value] of urlParams) { // get pair > extract it to key/value
        unserializedData[key] = value;
    }

    return unserializedData;
}