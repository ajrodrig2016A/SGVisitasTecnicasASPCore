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
    document.getElementById('subtotalCtz').value = stCotizacion.toFixed(4);
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
    document.getElementById('subtotalCtz').value = stCotizacion.toFixed(4);
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

    var lastRowIdx = rows.length - 2;

    var c = document.getElementsByClassName('detCtzCantidad')[lastRowIdx].value;
    var u = document.getElementsByClassName('detCtzValUnit')[lastRowIdx].value;
    var d = document.getElementsByClassName('detCtzDcto')[lastRowIdx].value;

    if (c > 0.00 && u > 0.00 && d >= 0.00) {
        document.getElementsByClassName('detCtzValTotal')[lastRowIdx].value = eval((c * u) * (1 - d)).toFixed(4);
        calcSubtotalCotizacion();
        bloquearCamposDetCtz(lastRowIdx);

        var nextRowIdx = eval(lastRowIdx) + 1;

        rowOuterHtml = rowOuterHtml.replaceAll('_' + lastRowIdx + '_', '_' + nextRowIdx + '_');
        rowOuterHtml = rowOuterHtml.replaceAll('[' + lastRowIdx + ']', '[' + nextRowIdx + ']');
        rowOuterHtml = rowOuterHtml.replaceAll('-' + lastRowIdx, '-' + nextRowIdx);

        var newRow = table.insertRow();
        newRow.innerHTML = rowOuterHtml;

        var x = table.getElementsByTagName("INPUT");

        for (var cnt = 0; cnt < x.length; cnt++) {
            if (x[cnt].type === "text" && x[cnt].id.indexOf('_' + nextRowIdx + '_') > 0) {
                x[cnt].value = '';
            }
            else if (x[cnt].type === "number" && x[cnt].id.indexOf('_' + nextRowIdx + '_') > 0) {
                x[cnt].value = "0.00";
            }
        }
    }
    else {
        alert("Favor ingrese valores numéricos de cantidad o porcentaje de descuento positivos");
        return;
    }

    rebindvalidatorsCotizacionForm();
}


function rebindvalidatorsCotizacionForm() {
    var $form = $("#CotizacionForm");
    $form.unbind();
    $form.data("validator", null);
    $.validator.unobtrusive.parse($form);
    $form.validate($form.data("unobtrusiveValidation").options);
}

function bloquearCamposDetCtz(lastRowIdx) {
    var detCtzCodigo = 'DetallesCotizacion_' + lastRowIdx + '__codigoProducto';
    document.getElementById(detCtzCodigo).readOnly = true;
    var detCtzUbicacion = 'DetallesCotizacion_' + lastRowIdx + '__ubicacion';
    document.getElementById(detCtzUbicacion).readOnly = true;
    document.getElementsByClassName('detCtzCantidad')[lastRowIdx].readOnly = true;
    document.getElementsByClassName('detCtzDcto')[lastRowIdx].readOnly = true;
}

document.addEventListener('keydown', ShowSearchableList);

function setSameWidth(srcElement, desElement) {
    desElement.style.width = "250px";
}

function ShowSearchableList(event) {
    if (event.target.id.indexOf('codigoProducto') < 0) {
        return;
    }

    if (document.getElementById('DetCtzTable') !== null) {
        table = document.getElementById('DetCtzTable');
        var rows = table.getElementsByTagName('tr');
        var lastRowIdx = rows.length - 2;

        var detCtzUbicacion = 'DetallesCotizacion_' + lastRowIdx + '__ubicacion';
        document.getElementById(detCtzUbicacion).value = "";
        document.getElementsByClassName('detCtzCantidad')[lastRowIdx].value = "";
        document.getElementsByClassName('detCtzValUnit')[lastRowIdx].value = "";
        document.getElementsByClassName('detCtzDcto')[lastRowIdx].value = "0.00";
        document.getElementsByClassName('detCtzValTotal')[lastRowIdx].value = "";

        var IdCodigoProducto = 'DetallesCotizacion_' + lastRowIdx + '__codigoProducto'
        if (document.getElementById(IdCodigoProducto).value === "") {
            document.getElementById('subtotalCtz').value = "0.00";
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
    else {
        table = document.getElementById('DetVtaTable');
        var rows = table.getElementsByTagName('tr');
        var lastRowIdx = rows.length - 2;

        document.getElementsByClassName('detVtaCantidad')[lastRowIdx].value = "";
        document.getElementsByClassName('detVtaValUnit')[lastRowIdx].value = "";
        document.getElementsByClassName('detVtaDcto')[lastRowIdx].value = "0.00";
        document.getElementsByClassName('detVtaValTotal')[lastRowIdx].value = "";

        var IdCodigoProductoVta = 'DetallesVenta_' + lastRowIdx + '__codigoProductoVta'
        if (document.getElementById(IdCodigoProductoVta).value === "") {
            document.getElementById('subtotalVta').value = "0.00";
            document.getElementById('IvaVta').value = "0.00";
            document.getElementById('totalVta').value = "0.00";
        }

        var tid = event.target.id;
        var txtDescId = tid.replaceAll('codigoProductoVta', 'descripcion');
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
}

document.addEventListener('change', function (e) {
    if (e.target.id.indexOf('codigoProducto') >= 0) {
        fillFieldsProductDetails(this);
    }
}, false);


function fillFieldsProductDetails(doc) {
    var table = document.getElementById('DetCtzTable');

    if (table !== null) {
        var rows = table.getElementsByTagName('tr');
        var lastRowIdx = rows.length - 2;
        var IdCodeProduct = 'DetallesCotizacion_' + lastRowIdx + '__codigoProducto';
        var idProducto = doc.getElementById(IdCodeProduct).value;

        var lstBoxBrands = document.getElementById("ddlBrandsIdAllProducts");
        var txtIdMarca = document.getElementById("txtIdMarca");

        var itemsB = lstBoxBrands.options;

        for (var i = itemsB.length - 1; i >= 0; i--) {
            if (itemsB[i].value === idProducto) {
                txtIdMarca.value = itemsB[i].text;
                SetBrandName(txtIdMarca.value);
                //return;
                var lstBoxUnits = document.getElementById("ddlUnitsIdAllProducts");
                var txtIdUnidad = document.getElementById("txtIdUnidad");

                var itemsU = lstBoxUnits.options;

                for (var i = itemsU.length - 1; i >= 0; i--) {
                    if (itemsU[i].value === idProducto) {
                        txtIdUnidad.value = itemsU[i].text;
                        SetUnitName(txtIdUnidad.value);
                        //return;
                        var lstBoxPrices = document.getElementById("ddlUnitPrAllProducts");

                        var itemsP = lstBoxPrices.options;

                        for (var i = itemsU.length - 1; i >= 0; i--) {
                            if (itemsP[i].value === idProducto) {
                                document.getElementsByClassName('detCtzValUnit')[lastRowIdx].value = eval(itemsP[i].text);
                                return;
                            }
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

document.addEventListener('change', function (e) {
    if (e.target.classList.contains('detCtzCantidad') || e.target.classList.contains('detCtzDcto') || e.target.classList.contains('detCtzValTotal')) {
        table = document.getElementById('DetCtzTable');
        var rows = table.getElementsByTagName('tr');
        var lastRowIdx = rows.length - 2;

        CalcTotalQuoteDetail(lastRowIdx);
        calcSubtotalCotizacion();
    }
}, false);


document.addEventListener('change', function (e) {
if (e.target.classList.contains('detVtaCantidad') || e.target.classList.contains('detVtaDcto') || e.target.classList.contains('detVtaValTotal')) {
        table = document.getElementById('DetVtaTable');
        var rows = table.getElementsByTagName('tr');
        var lastRowIdx = rows.length - 2;

        CalcTotalSaleDetail(lastRowIdx);
        calcSubtotalVenta();
        calcIvaVenta();
        calcTotalVenta();
    }
}, false);

function CalcTotalQuoteDetail(lastRowIdx) {
    var c = document.getElementsByClassName('detCtzCantidad')[lastRowIdx].value;
    var u = document.getElementsByClassName('detCtzValUnit')[lastRowIdx].value;
    var d = document.getElementsByClassName('detCtzDcto')[lastRowIdx].value;

    if (c > 0.00 && u > 0.00 && d >= 0.00) {
        document.getElementsByClassName('detCtzValTotal')[lastRowIdx].value = eval((c * u) * (1 - d)).toFixed(4);
    }
}



function calcSubtotalVenta() {
    var x = document.getElementsByClassName('detVtaValTotal');

    var stVenta = 0;
    var i;

    for (i = 0; i < x.length; i++) {
        if (eval(x[i].value) > 0.00 && eval(x[i].value) != '') {
            stVenta = stVenta + eval(x[i].value);
        }
    }
    document.getElementById('subtotalVta').value = stVenta.toFixed(4);
    return;
}

function reCalcSubtotalVenta() {
    var x = document.getElementsByClassName('detVtaValTotal');

    var stVenta = 0;
    var i;

    for (i = 0; i < x.length; i++) {
        if (eval(x[i].value) > 0.00 && eval(x[i].value) != '' && document.getElementById(x[i].id).value != "true") {
            stVenta = stVenta + eval(x[i].value);
        }
    }
    document.getElementById('subtotalVta').value = stVenta.toFixed(4);
    return;
}

function calcIvaVenta() {
    var x = document.getElementsByClassName('detVtaValTotal');

    var ivaVenta = 0;
    var i;

    for (i = 0; i < x.length; i++) {
        if (eval(x[i].value) > 0.00 && eval(x[i].value) != '') {
            ivaVenta = ivaVenta + eval(0.12 * (x[i].value));
        }
    }
    document.getElementById('IvaVta').value = ivaVenta.toFixed(4);
    return;
}

function reCalcIvaVenta() {
    var x = document.getElementsByClassName('detVtaValTotal');

    var ivaVenta = 0;
    var i;

    for (i = 0; i < x.length; i++) {
        if (eval(x[i].value) > 0.00 && eval(x[i].value) != '' && document.getElementById(x[i].id).value != "true") {
            ivaVenta = ivaVenta + eval(0.12 * (x[i].value));
        }
    }
    document.getElementById('IvaVta').value = ivaVenta.toFixed(4);
    return;
}

function calcTotalVenta() {
    var x = document.getElementsByClassName('detVtaValTotal');

    var totalVenta = 0;
    var i;

    for (i = 0; i < x.length; i++) {
        if (eval(x[i].value) > 0.00 && eval(x[i].value) != '') {
            totalVenta = totalVenta + eval(1.12 * (x[i].value));
        }
    }
    document.getElementById('totalVta').value = totalVenta.toFixed(4);
    return;
}

function reCalcTotalVenta() {
    var x = document.getElementsByClassName('detVtaValTotal');

    var totalVenta = 0;
    var i;

    for (i = 0; i < x.length; i++) {
        if (eval(x[i].value) > 0.00 && eval(x[i].value) != '' && document.getElementById(x[i].id).value != "true") {
            totalVenta = totalVenta + eval(1.12 * (x[i].value));
        }
    }
    document.getElementById('totalVta').value = totalVenta.toFixed(4);
    return;
}

function DeleteItemVta(btn) {
    var table = document.getElementById('DetVtaTable');
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
    reCalcSubtotalVenta();
    reCalcIvaVenta();
    reCalcTotalVenta();

}

function AddItemVta(btn) {
    var table;
    table = document.getElementById('DetVtaTable');
    var rows = table.getElementsByTagName('tr');

    var rowOuterHtml = rows[rows.length - 1].outerHTML;

    var lastRowIdx = rows.length - 2;

    var c = document.getElementsByClassName('detVtaCantidad')[lastRowIdx].value;
    var u = document.getElementsByClassName('detVtaValUnit')[lastRowIdx].value;
    var d = document.getElementsByClassName('detVtaDcto')[lastRowIdx].value;

    if (c > 0.00 && u > 0.00 && d >= 0.00) {
        document.getElementsByClassName('detVtaValTotal')[lastRowIdx].value = eval((c * u) * (1 - d)).toFixed(4);
        calcSubtotalVenta();
        calcIvaVenta();
        calcTotalVenta();
        bloquearCamposDetVta(lastRowIdx);

        var nextRowIdx = eval(lastRowIdx) + 1;

        rowOuterHtml = rowOuterHtml.replaceAll('_' + lastRowIdx + '_', '_' + nextRowIdx + '_');
        rowOuterHtml = rowOuterHtml.replaceAll('[' + lastRowIdx + ']', '[' + nextRowIdx + ']');
        rowOuterHtml = rowOuterHtml.replaceAll('-' + lastRowIdx, '-' + nextRowIdx);

        var newRow = table.insertRow();
        newRow.innerHTML = rowOuterHtml;

        var x = table.getElementsByTagName("INPUT");

        for (var cnt = 0; cnt < x.length; cnt++) {
            if (x[cnt].type === "text" && x[cnt].id.indexOf('_' + nextRowIdx + '_') > 0) {
                x[cnt].value = '';
            }
            else if (x[cnt].type === "number" && x[cnt].id.indexOf('_' + nextRowIdx + '_') > 0) {
                x[cnt].value = "0.00";
            }
        }
    }
    else {
        alert("Favor ingrese valores numéricos de cantidad o porcentaje de descuento positivos");
        return;
    }

    rebindvalidatorsVentaForm();
}


function rebindvalidatorsVentaForm() {
    var $form = $("#VentaForm");
    $form.unbind();
    $form.data("validator", null);
    $.validator.unobtrusive.parse($form);
    $form.validate($form.data("unobtrusiveValidation").options);
}

function bloquearCamposDetVta(lastRowIdx) {
    var detVtaCodigo = 'DetallesVenta_' + lastRowIdx + '__codigoProductoVta';
    document.getElementById(detVtaCodigo).readOnly = true;
    document.getElementsByClassName('detVtaCantidad')[lastRowIdx].readOnly = true;
    document.getElementsByClassName('detVtaDcto')[lastRowIdx].readOnly = true;
}



function fillFieldsSaleDetails(idProducto) {
    var table = document.getElementById('DetVtaTable');
    var rows = table.getElementsByTagName('tr');
    var lastRowIdx = rows.length - 2;
    //var IdCodeProduct = 'DetallesVenta_' + lastRowIdx + '__codigoProductoVta';
    //var idProducto = doc.getElementById(IdCodeProduct).value;

    var lstBoxPrices = document.getElementById("ddlUnitPrAllProducts");
    var itemsP = lstBoxPrices.options;

    for (var i = itemsP.length - 1; i >= 0; i--) {
        if (itemsP[i].value === idProducto.value) {
            document.getElementsByClassName('detVtaValUnit')[lastRowIdx].value = eval(itemsP[i].text);
            return;
        }
    }
    return;
}



function CalcTotalSaleDetail(lastRowIdx) {
    var c = document.getElementsByClassName('detVtaCantidad')[lastRowIdx].value;
    var u = document.getElementsByClassName('detVtaValUnit')[lastRowIdx].value;
    var d = document.getElementsByClassName('detVtaDcto')[lastRowIdx].value;

    if (c > 0.00 && u > 0.00 && d >= 0.00) {
        document.getElementsByClassName('detVtaValTotal')[lastRowIdx].value = eval((c * u) * (1 - d)).toFixed(4);
    }
}

$(function () {
    //Remove the style attributes.
    $(".navbar-nav li, .navbar-nav a, .navbar-nav ul").removeAttr('style');

    //Apply the Bootstrap class to the Submenu.
    $(".dropdown-menu").closest("li").removeClass().addClass("dropdown-toggle");

    //Apply the Bootstrap properties to the Submenu.
    $(".dropdown-toggle").find("a").eq(0).attr("data-toggle", "dropdown").attr("aria-haspopup", "true").attr("aria-expanded", "false").append("<span class='caret'></span>");

    //Apply the Bootstrap "active" class to the selected Menu item.
    //$("a.selected").closest("li").addClass("active");
    //$("a.selected").closest(".dropdown-toggle").addClass("active");
});
