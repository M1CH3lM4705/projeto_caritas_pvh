$(".btnCadastrarPessoa").click(function () {
    var data = { 'name': namePessoa }
    $.post(modal_url, add_anti_csrf(data), function (dados) {
        abrir_form(dados, 'Formulario de Cadastro', 'extra-large');
    }).fail(function () {

        $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
            hideDuration: 400,
        });
    });

});

$(document).on('click', '.btnAlterarPessoa', function () {
    var btn = $(this),
        id = btn.closest('tr').attr('data-id'),
        url = alterar_pessoa_url,
        data = { id: id };

    $.ajax({
        type: "POST",
        url: url,
        data: add_anti_csrf(data),
        success: function (response) {

            abrir_form(response, 'Alterar Cadastro', 'extra-large');
        },
        error: function () {
            $.notify("Ocorreu um erro ao processar a sua solicitaçaão. Contate o Administrador", "warn", {
                hideDuration: 400,
            });
        }
    });
}).on('click', '.btnExcluirPessoa', function () {
    var btn = $(this),
        tr = btn.closest('tr'),
        id = btn.closest('tr').attr('data-id'),
        url = "/PessoaCarente/ExcluirPessoa",
        data = { pessoaId: id }
    bootbox.confirm({
        message: "Tem certeza que deseja excluir essa pessoa?",
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
                            verificarGrid();
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
});

$(document).on('click', '#btnGravar', function () {
    var data = $("#formDados").serialize();
    var btn = $(this);
    var url = salvar_pessoa_url;
    var dados = unserialize(data);

    $.ajax({
        type: 'POST',
        url: url,
        data: data,
        success: function (response) {
            if (response.Resultado == "OK") {

                if (dados.PessoaCarenteId == 0) {

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

$(document).ready(function () {

    $("#Cpf").mask("999.999.999-99");

    $("#Endereco_Cep").mask("99.999-999");


});
$(document).on('blur', '.cpf', function () {
    $("#Cpf").blur(function () {
        var cpf = $('input#Cpf').val();

        var novoCPF = cpf.replace(/[\.-]/g, "");

        if (!validaCPF(novoCPF)) {
            alert('CPF Inválido');
        }
    });

});
$(document).on('keypress', '.cpf', function () {
    $('.cpf').mask('000.000.000-00', { reverse: true });

}).on('keypress', '.cep', function () {
    $('.cep').mask('00000-000');
}).on('keypress', '#Contato_Celular', function () {
    $("#Contato_Celular")
        .mask("(99) 99999-9999")
        .focusout(function (event) {
            var target, phone, element;
            target = (event.currentTarget) ? event.currentTarget : event.srcElement;
            phone = target.value.replace(/\D/g, '');
            element = $(target);
            element.unmask();
            if (phone.length > 10) {
                element.mask("(99) 99999-9999");
            } else {
                element.mask("(99) 9999-9999");
            }
        });
});
$(document).on('click', 'div#recolher1', function () {
    $('.card1').slideToggle();
})
    .on('click', 'div#recolher2', function () {
        $('.card2').slideToggle();
    })
    .on('click', 'div#recolher3', function () {
        $('.card3').slideToggle();
    })
    .on('click', 'div#recolher4', function () {
        $('.card4').slideToggle();
    });

$(document).on("click", "#addFamiliar", function () {
    $.ajax({
        cache: false,
        url: "/MembrosFamiliar/PartialMembroFamiliar",
        success: function (partialView) {
            $("#novoFamiliar").append(partialView);

        }

    });
    return false;
})
    .on("click", "#removerFamiliar", function () {
        $(this).parents("#addCampos").remove();
        return false;
    });

$(document).on('blur', '#Endereco_Cep', function () {

    var cep = $(this).val().replace(/\D/g, '');

    //Verifica se campo cep possui valor informado.
    if (cep != "") {

        //Expressão regular para validar o CEP.
        var validacep = /^[0-9]{8}$/;

        //Valida o formato do CEP.
        if (validacep.test(cep)) {

            //Preenche os campos com "..." enquanto consulta webservice.
            $("#Endereco_rua").val("...");
            $("#Endereco_bairro").val("...");
            $("#cidade").val("...");
            $("#uf").val("...");
            $("#ibge").val("...");

            //Consulta o webservice viacep.com.br/
            $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {

                if (!("erro" in dados)) {
                    //Atualiza os campos com os valores da consulta.
                    $("#Endereco_Rua").val(dados.logradouro);
                    $("#Endereco_Bairro").val(dados.bairro);
                    $("#Cidade").val(dados.localidade);

                } //end if.
                else {
                    //CEP pesquisado não foi encontrado.
                    limpa_formulário_cep();
                    alert("CEP não encontrado.");
                }
            });
        } //end if.
        else {
            //cep é inválido.
            limpa_formulário_cep();
            alert("Formato de CEP inválido.");
        }
    } //end if.
    else {
        //cep sem valor, limpa formulário.
        limpa_formulário_cep();
    }

    //Quando o campo cep perde o foco.
    $("#Endereco_Cep").blur(function () {

        //Nova variável "cep" somente com dígitos.

    });
});
function limpa_formulário_cep() {
    // Limpa valores do formulário de cep.
    $("#Endereco_rua").val("");
    $("#Endereco_bairro").val("");
    $("#cidade").val("");
    $("#uf").val("");
    $("#ibge").val("");
}
function validaCPF(cpf) {
    var numeros, digitos, soma, i, resultado, digitos_iguais;
    digitos_iguais = 1;
    if (cpf.length < 11)
        return false;
    for (i = 0; i < cpf.length - 1; i++)
        if (cpf.charAt(i) != cpf.charAt(i + 1)) {
            digitos_iguais = 0;
            break;
        }
    if (!digitos_iguais) {
        numeros = cpf.substring(0, 9);
        digitos = cpf.substring(9);
        soma = 0;
        for (i = 10; i > 1; i--)
            soma += numeros.charAt(10 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0))
            return false;
        numeros = cpf.substring(0, 10);
        soma = 0;
        for (i = 11; i > 1; i--)
            soma += numeros.charAt(11 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(1))
            return false;
        return true;
    }
    else
        return false;
}

var ativarInput = function () {
    var optionSelect = document.getElementById("Beneficio.SimNao").value;
    if (optionSelect == "true") {
        document.getElementById("Beneficio_TipoBeneficio").disabled = false;
        document.getElementById("Beneficio_TipoBeneficio").focus();
    } else {
        document.getElementById("Beneficio_TipoBeneficio").disabled = true;
    }
}

var AtivarOcupacaoAtual = function () {
    var optionSelect = document.getElementById("OcupacaoAtual"),
        perfil_ocup = document.getElementById("PerfilEconomico_OcupacaoAtual");
    if (optionSelect.value == "") {
        optionSelect.disabled = true;
        perfil_ocup.disabled = false;
        perfil_ocup.focus();
        
    } else {
        perfil_ocup.disabled = true;
        optionSelect.disabled = false;
    }
}

function criar_linha_grid(dados) {
    var ret =
        '<tr data-id=' + dados.PessoaCarenteId + '>' +
        '<td>' +

        '<a href="/PessoaCarente/Detalhes/' + dados.PessoaCarenteId + '" title="Clique, para inserir um encaminhamento">' + dados.Nome + '</a>' +
        '</td>' +
        '<td>' + dados.Voluntario.Nome + '</td>' +
        '<td>' + dados.Voluntario.Paroquia.Nome + '</td>' +
        + "<td>" + '< a href = "#"  class="btn btn-warning btnAlterarPessoa" > <span data-feather="edit"></span>Alterar</a >'
    '<a href="#" class="btn btn-danger btnExcluirPessoa"><span data-feather="trash"></span>Excluir</a>' + "</td>"
        + '</tr >';
    return ret;
}



