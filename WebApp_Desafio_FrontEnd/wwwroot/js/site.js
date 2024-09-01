/**
 * Busca um elemento em tela de campo invalido
 * Cria um alerta com a mensagem e tenta focar o campo
 * @param {jQueryObj} form
 */
function FormularioInvalidoAlert(form) {
    let mensagensDeErro = $("span.text-danger.field-validation-error");
    if (form) {
        mensagensDeErro = form.find("span.text-danger.field-validation-error");
    }
    let msg = "";
    let errElem = {};
    for (var i = 0; i < mensagensDeErro.length; i++) {
        if (mensagensDeErro[i].children.length > 0) {
            msg = mensagensDeErro[i].children[0].innerHTML;
            errElem = mensagensDeErro[i].children[0];
            break;
        } else if (mensagensDeErro[i].innerHTML) {
            msg = mensagensDeErro[i].innerHTML;
            errElem = mensagensDeErro[i];

        }
    }
    if (msg) {
        console.log(msg);
        Swal.fire({
            type: "warning",
            title: "Atenção",
            text: msg,
        }).then(function (result) {
            if (errElem) {
                //Padrão mvc
                let id = errElem.id.replace("-error", "");
                setTimeout(function () {
                    try {
                        $("#" + id).focus();
                    } catch (e) {
                        console.log(e);
                        console.log(errElem);
                        console.log("Não conseguiu focar elemento.");
                    }
                }, 500);
                //settimeout pois tem um focus no form ao clicar no ok do swal...
            }
        });
    }
}

/**
 * Serieliza o formulário em um objeto js
 * jquery obj do form => $('#form')
 * @param {jQueryObj} form
 */
function SerielizeForm(form) {

    let json = {};

    let serArr = form.serializeArray();

    $.each(serArr, function (i, field) {
        if (json[field.name] == undefined) {
            //strings enviadas para o backend apontando para propiedades decimal são automaticamente convertidas pelo asp
            //porem devem estar no formato correto. Com apenas uma separação...  virgula ou ponto 
            //#######,####### ou #######.#######

            //verifica se o campo no formulário/html possui determinada classe/mascara
            let element = $('#' + field.name);

            let fieldvalue = field.value || '';
            if (fieldvalue && (element.hasClass('money'))) {
                fieldvalue = ParseFloatDefault(fieldvalue);
            } else if (fieldvalue && (element[0] != undefined && element[0].className.includes('decimal'))) {
                fieldvalue = ParseFloatDefault(fieldvalue);
            }
            else if (element.prop('type') === 'checkbox') {
                fieldvalue = element.prop('checked');
            }

            json[field.name] = fieldvalue;

        } else {
            //console.warn(
            //    "SerielizeForm: Existe mais de um input com o mesmo nome => ",
            //    {
            //        ElementoOriginal: { Name: field.name, Value: json[field.name] },
            //        ElementoDuplicado: { Name: field.name, Value: field.value },
            //    });

            //throw exception?? 
            //se ocorrer um erro no servidor normalmente não troca a pagina então da pra diagnosticar pelo console...
        }
    });
    return json;
}
