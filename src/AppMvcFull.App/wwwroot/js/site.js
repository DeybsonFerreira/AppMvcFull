function SetModal() {

    $(document).ready(function () {
        //load modal when click
        $(function () {
            $.ajaxSetup({ cache: false });
            $("a[data-modal]").on("click",
                function (e) {
                    $('#myModalContent').load(this.href,
                        function () {
                            $('#myModal').modal('show');
                            bindForm(this);
                        });
                    return false;
                });
        });
    });
}

function btnSearchAddress() {
    BuscaCep();
}

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    $('#EnderecoTarget').load(result.url); // Carrega o resultado HTML para a div demarcada
                } else {
                    $('#myModalContent').html(result);
                    bindForm(dialog);
                }
            }
        });

        SetModal();
        return false;
    });
}

function clenFormAddress() {
    // Limpa valores do formulário de cep.
    let formStreetName = $("#Address_StreetName");
    let formDistrict = $("#Address_District");
    let formCity = $("#Address_City");
    let formState = $("#Address_State");

    formStreetName.val("");
    formDistrict.val("");
    formCity.val("");
    formState.val("");
}

function BuscaCep() {
    
    let formZipCode = $("#Address_ZipCode");
    let formStreetName = $("#Address_StreetName");
    let formDistrict = $("#Address_District");
    let formCity = $("#Address_City");
    let formState = $("#Address_State");

    //Nova variável "cep" somente com dígitos.
    let cep = formZipCode.val().replace(/\D/g, '');

    //Verifica se campo cep possui valor informado.
    if (cep != "") {

        //Expressão regular para validar o CEP.
        let validacep = /^[0-9]{8}$/;

        //Valida o formato do CEP.
        if (validacep.test(cep)) {

            //Preenche os campos com "..." enquanto consulta webservice.
            formStreetName.val(".....");
            formDistrict.val(".....");
            formCity.val(".....");
            formState.val(".....");

            //Consulta o webservice viacep.com.br/
            $.getJSON("https://viacep.com.br/ws/" + cep + "/json",
                function (dados) {
                    if (!("erro" in dados)) {
                        //Atualiza os campos com os valores da consulta.
                        formStreetName.val(dados.logradouro);
                        formDistrict.val(dados.bairro);
                        formCity.val(dados.localidade);
                        formState.val(dados.uf);

                    } //end if.
                    else {
                        //CEP pesquisado não foi encontrado.
                        clenFormAddress();
                        alert("CEP não encontrado.");
                    }
                });
        } //end if.
        else {
            //cep é inválido.
            clenFormAddress();
            alert("Formato de CEP inválido.");
        }
    } //end if.
    else {
        //cep sem valor, limpa formulário.
        clenFormAddress();
    }


}

$(document).ready(function () {
    $("#msg_box").fadeOut(2500);
});