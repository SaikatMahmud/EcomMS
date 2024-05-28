function LoadOrderHistory() {
    datatable = $('#tblData').DataTable({
        "processing": true,
        "serverSide": true,
        "searching": false,
        "cache": false,
        "ajax": {
            "url": "/Order/GetOrderHistoryCustomized",
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
                        return moment(data).format('YYYY-MM-DD');
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
                        buttons += `<button type="button" onclick="cancelOrderCustomer(${data})" class="btn btn-danger mx-1">Cancel</button>`;
                    }
                    else if (status == "Delivered") {
                        buttons += `<button type="button" onclick="" class="btn btn-success mx-1">Return</button>`;
                        buttons += `<button type="button" onclick="" class="btn btn-success mx-1">Review</button>`;
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

function cancelOrderCustomer(orderId) {
    $.ajax({
        url: '/Order/CancelOrderCustomer/' + orderId,
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