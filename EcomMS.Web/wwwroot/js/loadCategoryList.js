function LoadCategoryList() {
    $('#tblData').DataTable({
        "processing": true,
        "serverSide": true,
        "cache": false,
        "ajax": {
            "url": "/Category/GetAllCustomized",
            "type": "GET",
            "data": function (d) {
                d.search = d.search.value;
            }
        },
        "columnDefs": [
            { "className": "dt-center", "targets": "_all" }
        ],
        "language": {
            "searchPlaceholder": "Search with name"
        },
        "columns": [
            { data: 'id', width: '3%', "className": "dt-right" },
            { data: 'name', width: '15%' },
            {
                data: 'id', width: '5%', render: function (data, type, row, meta) {
                    return type === 'display' ?
                        '<button type="button" onclick="Edit(' + data + ')" class="btn btn-warning">Edit</button>' +
                        '<button type="button" onclick="Delete(' + data + ')" class="btn btn-danger"> Delete</button>' : data;
                }
            },
        ],
    });
}
function Edit(id) {
    console.log("executed");
    window.location.href = "/Category/Edit/" + id;
}