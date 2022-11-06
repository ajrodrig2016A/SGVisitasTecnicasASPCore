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

    var VisibleRowsCount = 0;

    var x = document.querySelectorAll("[id*='valorTotal']");
    for (i = 0; i < x.length; i++) {
        if (x[i].value > 0) {
            VisibleRowsCount++;
        }
    }

    if (VisibleRowsCount <= 1) {
        alert("Esta fila no puede ser eliminada");
        return;

    }

    var btnIdx = btn.id.replaceAll('btnRemove-', '');

    var idOfValorTotal = btnIdx + "__valorTotal";

    var txtValorTotal = document.querySelector("[id$='" + idOfValorTotal + "']");

    txtValorTotal.value = 0.00;


    var idOfIsDeleted = btnIdx + "__IsDeleted";
    var txtIsDeleted = document.querySelector("[id$='" + idOfIsDeleted + "']");
    txtIsDeleted.value = "true";

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
    var detCtzCodigo = 'cmbProductos-' + lastRowIdx;
    document.getElementById(detCtzCodigo).setAttribute("disabled", "disabled");
    var detCtzDescripcion = 'DetallesCotizacion_' + lastRowIdx + '__descripcion';
    document.getElementById(detCtzDescripcion).readOnly = true;
    document.getElementsByClassName('detCtzCantidad')[lastRowIdx].readOnly = true;
    document.getElementsByClassName('detCtzDcto')[lastRowIdx].readOnly = true;
}

document.addEventListener('change', function (e) {
    if (e.target.id.indexOf('cmbProductos') >= 0) {
        var cmbProductosIdx = e.target.id;
        var idOfIsSelected = cmbProductosIdx.replaceAll('cmbProductos-', '') + "__IsSelected";
        var txtIsSelected = document.querySelector("[id$='" + idOfIsSelected + "']");
        txtIsSelected.value = "true";
        fillFieldsProductDetails(cmbProductosIdx);
    }
}, false);


function fillFieldsProductDetails(cmbProductosIdx) {
    var table = document.getElementById('DetCtzTable');

    if (table !== null) {
        var cmbIdx = cmbProductosIdx.replaceAll('cmbProductos-', '')
        var idProducto = document.getElementById(cmbProductosIdx).value;
        document.getElementsByClassName('detCtzCodeProduct')[cmbIdx].value = idProducto;

        var lstBoxProducts = document.getElementById("ddlProductNames");
        var itemsP = lstBoxProducts.options;

        for (var i = itemsP.length - 1; i >= 0; i--) {
            if (itemsP[i].value === idProducto) {
                document.getElementsByClassName('detCtzNameProduct')[cmbIdx].value = itemsP[i].text;

                var lstBoxBrands = document.getElementById("ddlBrandsIdAllProducts");
                var itemsB = lstBoxBrands.options;

                for (var i = itemsB.length - 1; i >= 0; i--) {
                    if (itemsB[i].value === idProducto) {
                        SetBrandName(itemsB[i].text, cmbIdx);
                        //return;
                        var lstBoxUnits = document.getElementById("ddlUnitsIdAllProducts");
                        var itemsU = lstBoxUnits.options;

                        for (var i = itemsU.length - 1; i >= 0; i--) {
                            if (itemsU[i].value === idProducto) {
                                SetUnitName(itemsU[i].text, cmbIdx);
                                //return;
                                var lstBoxPrices = document.getElementById("ddlUnitPrAllProducts");
                                var itemsP = lstBoxPrices.options;

                                for (var i = itemsU.length - 1; i >= 0; i--) {
                                    if (itemsP[i].value === idProducto) {
                                        document.getElementsByClassName('detCtzValUnit')[cmbIdx].value = eval(itemsP[i].text);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    return;
}

function SetBrandName(txtIdMarca, cmbIdx) {
    var lstbox = document.getElementById('ddlBrandNames');
    var items = lstbox.options;

    for (var i = items.length - 1; i >= 0; i--) {
        if (items[i].value === txtIdMarca) {
            document.getElementsByClassName('detCtzNameBrand')[cmbIdx].value = items[i].text;
            return;
        }
    }
    return;
}


function SetUnitName(txtIdUnidad, cmbIdx) {
    var lstbox = document.getElementById('ddlUnitNames');
    var items = lstbox.options;

    for (var i = items.length - 1; i >= 0; i--) {
        if (items[i].value === txtIdUnidad) {
            document.getElementsByClassName('detCtzNameUnit')[cmbIdx].value = items[i].text;
            return;
        }
    }
    return;
}

document.addEventListener('change', function (e) {
    var idElementFired = e.target.id;

    if (e.target.id.match('DetallesCotizacion') !== null && e.target.id.match('cantidad') !== null && idElementFired !== null) {
        var indexCantidad = idElementFired.replaceAll('DetallesCotizacion_', '').replaceAll('__cantidad', '');
        CalcTotalQuoteDetail(indexCantidad);
        calcSubtotalCotizacion();
        return;
    }

    if (e.target.id.match('DetallesCotizacion') !== null && e.target.id.match('descuento') !== null && idElementFired !== null) {
        var indexDescuento = idElementFired.replaceAll('DetallesCotizacion_', '').replaceAll('__descuento', '');
        CalcTotalQuoteDetail(indexDescuento);
        calcSubtotalCotizacion();
        return;
    }

}, false);


document.addEventListener('change', function (e) {
    var idHtmlElemFired = e.target.id;

    if (e.target.id.match('DetallesVenta') !== null && e.target.id.match('cantidad') !== null && idHtmlElemFired !== null) {
        var indexCantidad = idHtmlElemFired.replaceAll('DetallesVenta_', '').replaceAll('__cantidad', '');
        CalcTotalSaleDetail(indexCantidad);
        calcSubtotalVenta();
        calcIvaVenta();
        calcTotalVenta();
        return;
    }

    if (e.target.id.match('DetallesVenta') !== null && e.target.id.match('descuento') !== null && idHtmlElemFired !== null) {
        var indexDescuento = idHtmlElemFired.replaceAll('DetallesVenta_', '').replaceAll('__descuento', '');
        CalcTotalSaleDetail(indexDescuento);
        calcSubtotalVenta();
        calcIvaVenta();
        calcTotalVenta();
        return;
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

    var VisibleRowsCount = 0;

    var x = document.querySelectorAll("[id*='valorTotal']");
    for (i = 0; i < x.length; i++) {
        if (x[i].value > 0) {
            VisibleRowsCount++;
        }
    }

    if (VisibleRowsCount <= 1) {
        alert("Esta fila no puede ser eliminada");
        return;

    }

    var btnIdx = btn.id.replaceAll('btnRemove-', '');

    var idOfValorTotal = btnIdx + "__valorTotal";

    var txtValorTotal = document.querySelector("[id$='" + idOfValorTotal + "']");

    txtValorTotal.value = 0.00;

    var idOfIsDeleted = btnIdx + "__IsDeleted";
    var txtIsDeleted = document.querySelector("[id$='" + idOfIsDeleted + "']");
    txtIsDeleted.value = "true";

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
    var detCtzDescripcion = 'cmbVtasProductos-' + lastRowIdx;
    document.getElementById(detCtzDescripcion).setAttribute("disabled", "disabled");
    document.getElementsByClassName('detVtaCantidad')[lastRowIdx].readOnly = true;
    document.getElementsByClassName('detVtaDcto')[lastRowIdx].readOnly = true;
}

document.addEventListener('change', function (e) {
    if (e.target.id.indexOf('cmbVtasProductos') >= 0) {
        var cmbVtasProductosIdx = e.target.id;
        var idOfIsSelected = cmbVtasProductosIdx.replaceAll('cmbVtasProductos-', '') + "__IsSelected";
        var txtIsSelected = document.querySelector("[id$='" + idOfIsSelected + "']");
        txtIsSelected.value = "true";
        fillFieldsSaleDetails(cmbVtasProductosIdx);
    }
}, false);

function fillFieldsSaleDetails(cmbProductosIdx) {
    var cmbIdx = cmbProductosIdx.replaceAll('cmbVtasProductos-', '')
    var IdNameProduct = cmbProductosIdx;
    var idProducto = document.getElementById(IdNameProduct).value;
    document.getElementsByClassName('detVtaCodeProduct')[cmbIdx].value = idProducto;

    var lstBoxPrices = document.getElementById("ddlUnitPrAllProducts");
    var itemsP = lstBoxPrices.options;

    for (var i = itemsP.length - 1; i >= 0; i--) {
        if (itemsP[i].value === idProducto) {
            document.getElementsByClassName('detVtaValUnit')[cmbIdx].value = eval(itemsP[i].text);
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
