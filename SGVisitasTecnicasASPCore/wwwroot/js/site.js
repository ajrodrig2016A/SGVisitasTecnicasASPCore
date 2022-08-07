// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
    $(document).ready(function ($) {
        $('#headerVideoLink').magnificPopup({
            type: 'inline',
            midClick: true // Allow opening popup on middle mouse click. Always set it to true if you don't provide alternative source in href.
        });
    });

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

        rebindvalidators();
        var c = document.getElementsByClassName('detCtzCantidad')[lastRowIdx].value;
        var u = document.getElementsByClassName('detCtzValUnit')[lastRowIdx].value;

        if (c > 0.00 && u > 0.00) {
            document.getElementsByClassName('detCtzValTotal')[lastRowIdx].value = eval(c * u);
            calcSubtotalCotizacion();
        }

    }


    function rebindvalidators() {
        var $form = $("#CotizacionForm");
        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse($form);
        $form.validate($form.data("unobtrusiveValidation").options);
    }

    document.addEventListener('keydown', ShowSearchableList);

    function setSameWidth(srcElement, desElement) {
        desElement.style.width = "250px";
    }

    function ShowSearchableList(event) {
        if (event.target.id.indexOf('codigoProducto') < 0) {
            return;
        }

        var tid = event.target.id;
        var txtDescId = tid.replaceAll('codigoProducto', 'descripcion');
        var txtValue = document.getElementById('txtValue');
        var txtText = document.getElementById(txtDescId);
        var txtSearch = event.target;

        var lstBox = document.getElementById("mySelect");
        $(txtSearch).after($(lstBox).show('slow'));

        if (event.keyCode === 13 || event.keyCode == 9) {
            txtSearch.value = txtValue.value;
            lstBox.style.visibility = "hidden";

            var divLst = document.getElementById("HiddenDiv");
            $(divLst).after($(lstBox).show('slow'));

            if (event.keyCode === 13) {
                event.preventDefault();
                txtSearch.focus();
                return;
            }
            else {
                return;
            }
        }


        setSameWidth(txtSearch, lstBox);
        lstBox.style.visibility = "visible";

        txtValue.value = "";
        txtText.value = "";

        var items = lstBox.options;
        for (var i = items.length - 1; i >= 0; i--) {
            if (items[i].text.toLowerCase().indexOf(txtSearch.value.toLowerCase()) > -1) {
                items[i].style.display = 'block';
                items[i].selected = true;
                txtValue.value = items[i].value;
                txtText.value = items[i].text;
            }
            else {
                items[i].style.display = 'none';
                items[i].selected = false;
            }
        }
    }

function fillFieldsProductDetails(idProducto) {
        var table = document.getElementById('DetCtzTable');
        var rows = table.getElementsByTagName('tr');
        var lastRowIdx = rows.length - 2;


        var lstBoxBrands = document.getElementById("ddlBrandsIdAllProducts");
        var txtIdMarca = document.getElementById("txtIdMarca");

        var itemsB = lstBoxBrands.options;

        for (var i = itemsB.length - 1; i >= 0; i--) {
            if (itemsB[i].value === idProducto.value) {
                txtIdMarca.value = itemsB[i].text;
                SetBrandName(txtIdMarca.value);
                //return;
                var lstBoxUnits = document.getElementById("ddlUnitsIdAllProducts");
                var txtIdUnidad = document.getElementById("txtIdUnidad");

                var itemsU = lstBoxUnits.options;

                for (var i = itemsU.length - 1; i >= 0; i--) {
                    if (itemsU[i].value === idProducto.value) {
                        txtIdUnidad.value = itemsU[i].text;
                        SetUnitName(txtIdUnidad.value);
                        //return;
                        var lstBoxPrices = document.getElementById("ddlUnitPrAllProducts");
                        var txtValorUnitario = document.getElementById("txtValorUnitario");

                        var itemsP = lstBoxPrices.options;

                        for (var i = itemsU.length - 1; i >= 0; i--) {
                            if (itemsP[i].value === idProducto.value) {
                                document.getElementsByClassName('detCtzValUnit')[lastRowIdx].value = eval(itemsP[i].text);
                                //txtValorUnitario.value = itemsP[i].text;
                                //SetUnitPrices(txtValorUnitario.value);
                                return;
                            }
                        }
                    }
                }
            }
        }
        return;
    }

    function SetBrandName(txtIdMarca) {
        var txtBrandName = document.getElementById("txtNombreMarca");

        var lstbox = document.getElementById('ddlBrandNames');
        var items = lstbox.options;

        for (var i = items.length - 1; i >= 0; i--) {
            if (items[i].value === txtIdMarca) {
                txtBrandName.value = items[i].text;
                table = document.getElementById('DetCtzTable');
                var rows = table.getElementsByTagName('tr');
                var lastRowIdx = rows.length - 2;
                document.getElementsByClassName('detCtzNameBrand')[lastRowIdx].value = txtBrandName.value;
                return;
            }
        }
        return;
    }



    function SetUnitName(txtIdUnidad) {
        var txtUnitName = document.getElementById("txtNombreUnidad");

        var lstbox = document.getElementById('ddlUnitNames');
        var items = lstbox.options;

        for (var i = items.length - 1; i >= 0; i--) {
            if (items[i].value === txtIdUnidad) {
                txtUnitName.value = items[i].text;
                table = document.getElementById('DetCtzTable');
                var rows = table.getElementsByTagName('tr');
                var lastRowIdx = rows.length - 2; 
                document.getElementsByClassName('detCtzNameUnit')[lastRowIdx].value = txtUnitName.value;
                return;
            }
        }
        return;
    }

//function SetUnitPrices(txtValorUnitario) {
//    var txtUnitPrice = document.getElementById("txtValorUnitario");

//    var lstbox = document.getElementById('ddlUnitPrAllProducts');
//    var items = lstbox.options;

//    for (var i = items.length - 1; i >= 0; i--) {
//        if (items[i].value === txtIdMarca) {
//            txtUnitPrice.value = items[i].text;
//            table = document.getElementById('DetCtzTable');
//            var rows = table.getElementsByTagName('tr');
//            var lastRowIdx = rows.length - 2;
//            document.getElementsByClassName('detCtzValUnit')[lastRowIdx].value = txtUnitPrice.value;
//            return;
//        }
//    }
//    return;
//}

    //document.addEventListener('change', function (e) {
    //    if (event.target.id.indexOf('codigoProducto') >= 0) {
    //        SetUnitName(event.target);
    //    }
    //}, false);



