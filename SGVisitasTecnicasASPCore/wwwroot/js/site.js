// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    function calcSubtotalCotizacion() {
        var x = document.getElementsByClassName('detCtzValTotal');

        var stCotizacion = 0;
        var i;

        for (i = 0; i < x.length; i++) {
            if (eval(x[i].value) > 0.00 && eval(x[i].value) != '') {
                stCotizacion = stCotizacion + eval(x[i].value);
            }
        }
        document.getElementById('subtotalCtz').value = stCotizacion;
        return;
}

function reCalcSubtotalCotizacion() {
    var x = document.getElementsByClassName('detCtzValTotal');

    var stCotizacion = 0;
    var i;

    for (i = 0; i < x.length; i++) {
        if (eval(x[i].value) > 0.00 && eval(x[i].value) != '' && document.getElementById(x[i].id).value != "true") {
            stCotizacion = stCotizacion + eval(x[i].value);
        }
    }
    document.getElementById('subtotalCtz').value = stCotizacion;
    return;
}


    function DeleteItem(btn) {
        var table = document.getElementById('DetCtzTable');
        var rows = table.getElementsByTagName('tr');

        if (rows.length == 2) {
            alert("Esta fila no puede ser eliminada");
            return;

        }

        var btnIdx = btn.id.replaceAll('btnRemove-', '');

        var idOfValorTotal = btnIdx + "__valorTotal";

        var txtValorTotal = document.querySelector("[id$='" + idOfValorTotal + "']");

        txtValorTotal.value = 0.00;

        $(btn).closest('tr').hide();
        reCalcSubtotalCotizacion();

    }

    function AddItem(btn) {
        var table;
        table = document.getElementById('DetCtzTable');
        var rows = table.getElementsByTagName('tr');

        var rowOuterHtml = rows[rows.length - 1].outerHTML;

        var lastRowIdx = rows.length - 2; //document.getElementById('hdnLastIndex').value;

        var nextRowIdx = eval(lastRowIdx) + 1;

        rowOuterHtml = rowOuterHtml.replaceAll('_' + lastRowIdx + '_', '_' + nextRowIdx + '_');
        rowOuterHtml = rowOuterHtml.replaceAll('[' + lastRowIdx + ']', '[' + nextRowIdx + ']');
        rowOuterHtml = rowOuterHtml.replaceAll('-' + lastRowIdx, '-' + nextRowIdx);

        var newRow = table.insertRow();
        newRow.innerHTML = rowOuterHtml;

        var x = document.getElementsByTagName("INPUT");

        for (var cnt = 0; cnt < x.length; cnt++) {
            if (x[cnt].type == "text" && x[cnt].id.indexOf('_' + nextRowIdx + '_') > 0) {
                x[cnt].value = '';
            }
            else if (x[cnt].type == "number" && x[cnt].id.indexOf('_' + nextRowIdx + '_') > 0) {
                x[cnt].value = 0;
            }
        }


        var c = document.getElementsByClassName('detCtzCantidad')[lastRowIdx].value;
        var u = document.getElementsByClassName('detCtzValUnit')[lastRowIdx].value;

        if (c > 0.00 && u > 0.00) {
            document.getElementsByClassName('detCtzValTotal')[lastRowIdx].value = eval(c * u);
            calcSubtotalCotizacion();
        }

        //calcTotalDetCotizacion(lastRowIdx);

        rebindvalidators();

    }


    function rebindvalidators() {
        var $form = $("#CotizacionForm");
        $form.unbind();
        $form.data("validator", null);
        $.validator.unobstrusive.parse($form);
        $form.validate($form.data("unobstrusiveValidation").options);
    }

