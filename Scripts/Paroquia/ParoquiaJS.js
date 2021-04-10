$(".btnParoquia").click(function () {
    url = "/Admin/Paroquias/FormParoquia"
    param = { 'id': 0 };
    $.post(url, add_anti_csrf(param), function (dados) {
        abrir_form(dados, 'Formulario de Cadastro', 'large');
    }).fail(function () {

        $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
            hideDuration: 400,
        });

    });

});

$(document).on('click', '.btnAlterarParoquia', function () {
    var btn = $(this),
        id = btn.closest('tr').attr('data-id'),
        url = "/Admin/Paroquias/FormParoquia",
        data = { id: id };

    $.ajax({
        type: "Post",
        url: url,
        data: add_anti_csrf(data),
        success: function (response) {

            abrir_form(response, 'Alterar Cadastro', 'large');
        },
        error: function () {
            $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
                hideDuration: 400,
            });
        }
    });
}).on('click', '.btnExcluirParoquia', function () {
    var btn = $(this),
        tr = btn.closest('tr'),
        id = btn.closest('tr').attr('data-id'),
        url = "/Admin/Paroquias/DeletarParoquia",
        data = { paroquiaId: id };

    bootbox.confirm({
        message: "Tem certeza que deseja excluir essa Paróquia?",
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
}).on('click', '.btnDetalhes', function () {
    var btn = $(this),
        id = btn.closest('tr').attr('data-id'),
        url = "/Admin/Paroquias/Detalhes",
        data = { id: id };

    $.ajax({
        type: "Post",
        url: url,
        data: add_anti_csrf(data),
        success: function (response) {

            abrir_form_detalhes(response, 'Detalhes da Paróquia', 'large');
        },
        error: function () {
            $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
                hideDuration: 400,
            });
        }
    });
});

$(document).on('click', '#btnGravar', function () {
    var data = $("#formDados").serialize();
    var btn = $(this);
    var url = "/Admin/Paroquias/SalvarParoquia";
    var dados = unserialize(data);

    $.ajax({
        type: 'POST',
        url: url,
        data: data,
        success: function (response) {
            if (response.Resultado == "OK") {
                if (dados.ParoquiaId == 0) {

                    //var table = $("#table_grid").find('tbody'),
                    //    linha = criar_linha_grid(response.pessoa);

                    //table.append(linha);
                    //$("#myModal").modal('hide');
                    $.notify("Dados salvos com sucesso", "success");
                    Lista(url_index);


                } else {
                    //var linha = $("#table_grid")
                    //    .find('tr[data-id=' + data.PessoaCarenteId + ']')
                    //    .find('td');
                    //linha
                    //    .eq(0).html(data.Nome).end()
                    //    .eq(1).html(data.Voluntario.Nome).end()
                    //    .eq(2).html(data.Voluntario.Paroquia.Nome);
                    $.notify("Dados alterado com sucesso", "success");
                    Lista(url_index);
                }
                fecharModal();
            } else if (response.Resultado == "AVISO") {
                exibirAviso(response.Mensagens);


            } else if (response.Resultado == "ERRO") {
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