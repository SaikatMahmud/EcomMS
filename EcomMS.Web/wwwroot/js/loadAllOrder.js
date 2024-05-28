var datatable;
function LoadAllOrder() {
    datatable = $('#tblData').DataTable({
        "processing": true,
        "serverSide": true,
        "searching": false,
        "cache": false,
        "ajax": {
            "url": "/Order/GetAllOrderCustomized",
            "type": "GET",
            "data": function (d) {
                //d.search = d.search.value;
                d.orderColumn = d.order[0].column;
                d.orderDirection = d.order[0].dir;
            }
        },
        "columnDefs": [
            { "className": "dt-center", "targets": "_all" }
        ],
        //"language": {
        //    "searchPlaceholder": "search for product"
        //},
        "columns": [
            { data: 'id', width: '3%', "className": "dt-right" },
            { data: 'amount', width: '15%' },
            { data: function (order) { return getFirstStatus(order); }, width: '8%' },
            { data: 'paymentMethod', width: '5%' },
            {
                data: 'createdAt',
                render: function (data) {
                    if (data) {
                        return moment(data).format('YYYY-MM-DD, h:mm a');
                    } else {
                        return '';
                    }
                },
                width: '10%'
            },
            {
                data: 'id',
                render: function (data, type, row) {
                    var status = getFirstStatus(row);
                    var buttons = `<div class="w-75 btn-group" role="group">
                    <button type="button" onclick="viewOrderDeatils(${data})" class="btn btn-warning mx-1"><i class="bi bi-three-dots-vertical"></i></button>`;
                    if (status == "Placed") {
                        buttons += `<button type="button" onclick="processOrder(${data})" class="btn btn-success mx-1">Process</button>`;
                        buttons += `<button type="button" onclick="cancelOrderAdmin(${data})" class="btn btn-danger mx-1">Cancel</button>`;
                    }
                    else if (status == "Processing") {
                        buttons += `<button type="button" onclick="shipOrder(${data})" class="btn btn-success mx-1">Ship</button>`;
                    }
                    buttons += `</div>`;
                    return buttons;
                },
                width: '5%'
            }
        ],
    });

}

function getFirstStatus(order) {
    if (order.orderStatusHistories && order.orderStatusHistories.length > 0) {
        return order.orderStatusHistories[0].status;
    } else {
        return '';
    }
}
function processOrder(orderId) {
    $.ajax({
        url: '/Order/ProcessOrderAdmin/' + orderId,
        type: 'PUT',
        success: function (response) {
            debugger;
            if (response.success) {
                datatable.ajax.reload();
                toastr.success(response.msg);
            }
            else {
                toastr.error(response.msg);
            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });
}
function shipOrder(orderId) {
    $.ajax({
        url: '/Order/ShipOrderAdmin/' + orderId,
        type: 'PUT',
        success: function (response) {
            debugger;
            if (response.success) {
                datatable.ajax.reload();
                toastr.success(response.msg);
            }
            else {
                toastr.error(response.msg);
            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });
}
function viewOrderDeatils(orderId) {
    window.location.href = '/Order/OrderDetails/' + orderId;
}
function cancelOrderAdmin(orderId) {
    $.ajax({
        url: '/Order/CancelOrderAdmin/' + orderId,
        type: 'PUT',
        success: function (response) {
            debugger;
            if (response.success) {
                datatable.ajax.reload();
                toastr.success(response.msg);
            }
            else {
                toastr.error(response.msg);
            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });
}

