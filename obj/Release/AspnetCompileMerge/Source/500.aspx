﻿<% Response.StatusCode = 500 %>

<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="utf-8" />
    <link href="/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/style.css" rel="stylesheet" />
    <link rel="shortcut icon" href="/Content/img/favicon.ico" />
    <title>Erro de servidor 500</title>
</head>
<body>
    <div class="container Erros">
        
        <div class="row">
            <div class="col-md-9 col-sm-9">
                <h2>Ocorreu um erro ao processar a sua solicitação.</h2>
                <h2>Por favor, em alguns instantes tente novamente</h2>
                <img src="/Content/img/Logo-Caritas.jpg" class="img-fluid"/>
            </div>
        </div>
     <div class="row">
            <div class="col col-md-12 col-sm-12">
                <button onclick="goBack()" class="btn btn-lg btn-dark">Voltar</button>
            </div>
        </div>
    </div>
    <script>
        function goBack() {
            window.history.back()
        }
    </script>
</body>
</html>