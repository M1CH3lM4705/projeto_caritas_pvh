

$(document).on('click', '#btnEncNovo', function () {

    var data = {id:$("#pessoaId").attr('data-id')}
    $.post(url_novo_enc, add_anti_csrf(data), function (dados) {
        abrir_form(dados, 'Novo Encaminhamento', 'large');
    }).fail(function () {

        $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
            hideDuration: 400,
        });

    });

})
    .on('click', '.btnAlterarEnc', function () {
        var btn = $(this),
            id = btn.closest('tr').attr('data-id'),
            url = url_alterar_enc,
            data = { id: id };

        $.ajax({
            type: 'POST',
            url: url,
            data: add_anti_csrf(data),
            success: function (resposta) {
                abrir_form(resposta, 'Alterar Encaminhamento', 'large');
            },
            error: function () {
                $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
                    hideDuration: 400,
                });
            }
        });
    }).on('click', '.btnExcluirEnc', function () {
        var btn = $(this),
            tr = btn.closest('tr'),
            id = btn.closest('tr').attr('data-id'),
            url = "/Encaminhamento/ExcluirEncaminhamento",
            data = { EncaminhamentoId: id };

        bootbox.confirm({
            message: "Tem certeza que deseja excluir esse Encaminhamento?",
            buttons: {
                confirm: {
                    label: 'Sim',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Não',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: add_anti_csrf(data),
                        success: function (response) {
                            if (response.Resultado == true) {
                                tr.remove();
                                $.notify(response.Mensagem, "success");
                            } else {
                                $.notify(response.Mensagem, "error");
                            }
                        },
                        error: function () {
                            $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
                                hideDuration: 400,
                            });
                        }
                    });
                }
            }
        });
    }).on('click', '#btnGravar', function () {

        //$("#loaderDiv").show();

        var data = $("#formDados").serialize();
        var dados = unserialize(data);
        console.log(dados);

        $.ajax({
            type: "POST",
            url: "/Encaminhamento/SalvarEncaminhamento",
            data: data,

            success: function (resposta) {
                dados.PessoaCarenteId = parseInt(resposta.IdSalvo, 10);
                if (resposta.Resultado == "OK") {
                    if (dados.EncaminhamentoId == 0) {
                        
                        ListaEnc(dados.PessoaCarenteId);
                        $.notify("Encaminhamento salvo com sucesso!", "success");
                    } else {
                        $.notify("Dados alterado com sucesso", "success");
                        ListaEnc(dados.PessoaCarenteId);
                    }
                    fecharModal();
                } else if (resposta.Resultado == "AVISO") {
                    exibirAviso(resposta.Mensagens)
                } else if (resposta.Resultado == "ERRO") {
                    exibirErro();
                }

            },
            error: function () {
                $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
                    hideDuration: 400,
                });
            }
        });
    });

$(document).on('click', '.btnFinalizar', function (e) {


    e.preventDefault;
    var id = $(this).closest('tr').attr('data-id');
    var tipo = $(this).closest('tr').find('td[data-tipo]').data('tipo');
    var dataSolicitacao = $(this).closest('tr').find('td[data-solicitacao]').data('solicitacao');
    var statuS = $(this).closest('tr').find('td[data-status]').data('status');
    var dataEntrega = $(this).closest('tr').find('td[data-entrega]').data('entrega');
    var idPessoa = $(this).data('value');
    //alert(id);

    var data = {
        EncaminhamentoId: id,
        TipoEncaminhamento: tipo,
        DataSolicitacao: dataSolicitacao,
        status: statuS,
        DataEntrega: dataEntrega,
        PessoaCarenteId: idPessoa
    };

    $.ajax({
        url: "/Encaminhamento/FinalizarEntrega",
        type: "POST",
        data: add_anti_csrf(data),
        success: function (data) {
            if (data.Resultado > 0) {
                //debugger;

                ListaEnc(data.Resultado);
                $.notify(data.Mensagem, "success", {
                    globalPosition: "top center"
                });
            }
        },
        error: function () {
            $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
                hideDuration: 400,
            });
        }
    });

}).on('change', '#tamanhoPage', function () {
    var selectTamPag = $(this),
        tamanhoPag = selectTamPag.val(),
        pagina = 1,
        id = $("#pessoaId").attr('data-id'),
        url = url_lista_paginacao,
        param = { 'id': id, 'pagina': pagina, 'tamanhoPag': tamanhoPag };

    $.get(url, param, function (resposta) {
        if (resposta) {
            ListaEnc(param.id);
        }
    }).fail(function () {

        $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
            hideDuration: 400,
        });

    });


});

function ListaEnc(idPessoa) {
    var url = "/Encaminhamento/ListaEncaminhamento";
    var
        tamanhoPag = $("#tamanhoPag").val(),
        pagina = viewBagPagina;
    $.ajax({
        url: url,
        type: "GET",
        data: { id: idPessoa, tamanhoPag: tamanhoPag, pagina: pagina },
        dataType: "html",
        cache: false,
        async: true,
        success: function (data) {
            if (data) {
                $(".btnFinalizar").html("Finalizado");
                $("#divEncaminhar").html(data);
                atualizarData(idPessoa);
            }
        },
        error: function () {
            $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
                hideDuration: 400,
            });
        }
    });
}

function atualizarData(id) {
    var url = "/Encaminhamento/AtualizaData",
        data = { 'id': id };

    $.ajax({
        type: "POST",
        data: add_anti_csrf(data),
        url: url,
        dataType: "json",
        cache: false,
        async: true,
        success: function (resposta) {
            if (resposta) {
                var Data = '<h5>Data da proxima entrega: </h5>' +
                    '<p class="h3"><i class="bi bi-calendar-date" style="margin-right:2.3rem;"></i>' + resposta.dataEntrega + '</p>'

                $("#dataEntrega").html(Data);

                console.log(resposta.dataEntrega);
            }
        }
    })
}