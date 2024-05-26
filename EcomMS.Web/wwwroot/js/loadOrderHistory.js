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
                render: function (data, type, row) {
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
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <button type="button" onclick="" class="btn btn-warning mx-1"></button>
                    <button type="button" onclick="" class="btn btn-danger mx-1"></button>
                    </div>`
                },
                width: '2%'
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