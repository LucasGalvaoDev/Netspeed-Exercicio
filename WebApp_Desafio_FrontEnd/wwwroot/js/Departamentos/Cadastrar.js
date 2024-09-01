$(document).ready(function () {
    $('.glyphicon-calendar').closest("div.date").datepicker({
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: false,
        format: 'dd/mm/yyyy',
        autoclose: true,
        language: 'pt-BR'
    });

    $('#btnCancelar').click(function () {
        Swal.fire({
            html: "Deseja cancelar essa operação? O registro não será salvo.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: 'Sim, cancelar',
            cancelButtonText: 'Não, continuar'
        }).then(function (result) {
            if (result.isConfirmed) {
                history.back();
            } else {
                console.log("Cancelou a inclusão.");
            }
        });
    });

    $('#btnSalvar').click(function () {
        if ($('#form').valid() != true) {
            Swal.fire({
                text: 'Por favor, corrija os erros antes de salvar.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
            return;
        }

        let chamado = SerializeForm($('#form'));
        let url = $('#form').attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: chamado,
            success: function (result) {
                Swal.fire({
                    icon: result.success ? 'success' : 'error',
                    title: result.success ? 'Sucesso' : 'Erro',
                    text: result.message
                }).then(function () {
                    if (result.success) {
                        window.location.href = result.redirectTo;
                    }
                });
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    text: 'Ocorreu um erro ao processar sua solicitação.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
    });

    function SerializeForm(form) {
        var data = {};
        $(form).find('input, select, textarea').each(function () {
            var input = $(this);
            var name = input.attr('name');
            var value = input.val();
            if (name) {
                data[name] = value;
            }
        });
        return data;
    }
});
