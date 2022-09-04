
    $(function()
    {
        function after_form_submitted(data) {
            if (data === "success") {
                var strMessage = "Mensaje enviado exitosamente.";
                var myDiv = document.getElementById("myModalSuccessAlertBody");
                myDiv.innerHTML = strMessage;
                $('#myModalSuccess').modal('show');
                $('#exampleModalCenter').modal('hide');
            }
            else {
                var strMessage = "Lo siento, hubo un error al enviar su formulario.";
                var myDiv = document.getElementById("MyModalErrorAlertBody");
                myDiv.innerHTML = strMessage;
                $('#myModalError').modal('show');
                $('#exampleModalCenter').modal('hide');
                $form = $('form#reused_form');

            }
        }


	$('#reused_form').submit(function(e)
    {
        e.preventDefault();

    $form = $(this);
    //show some response on the button
    $('button[type="submit"]', $form).each(function()
    {
        $btn = $('#send_email');
        $btn.prop('orig_label',$btn.text());
        $btn.text('Sending ...');
        });


    $.ajax({
        type: "POST",
        url: '/Home/StartRequestQuote',
        data: $form.serialize(),
        success: after_form_submitted,
        dataType: 'json'
            });

      });
    });

function prepareEmail() {  
    $form = $('form#reused_form').show();
    $('#send_email').text("Enviar");
    $form.trigger("reset");
}