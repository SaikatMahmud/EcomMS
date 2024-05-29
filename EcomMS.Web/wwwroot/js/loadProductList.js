var datatable;
$(document).ready(function () {
    $('#categoryId').change(function () {
        $('#customFilter').val("all");
        var selectedValue = $('#categoryId').val();
        datatable.destroy();
        LoadProductList(selectedValue);
    });
    $('#customFilter').change(function () {
        $("#categoryId").val(0);
        var selectedValue = $('#customFilter').val();
        datatable.destroy();
        var url = "";
        if (selectedValue == "all") {
            LoadProductList(0);
        }
        else if (selectedValue == "lowStock") {
            url = "/Product/GetAllLowStockCustomized";
            LoadByCustomFilter(url);
        }
        else if (selectedValue == "offline") {
            url = "/Product/GetAllOfflineCustomized";
            LoadByCustomFilter(url);
        }
    });
});


function LoadByCustomFilter(url) {
    datatable = $('#tblData').DataTable({
        "processing": true,
        "serverSide": true,
        "cache": false,
        "ajax": {
            "url": url,
            "type": "GET",
            "data": function (d) {
                d.search = d.search.value;
                d.orderColumn = d.order[0].column;
                d.orderDirection = d.order[0].dir;
            }
        },
        "columnDefs": [
            { "className": "dt-center", "targets": "_all" }
        ],
        "language": {
            "searchPlaceholder": "search for product"
        },
        "columns": [
            { data: 'id', width: '3%', "className": "dt-right" },
            { data: 'name', width: '15%' },
            { data: 'price', width: '8%' },
            { data: 'quantity', width: '5%' },
            { data: 'reorderQuantity', width: '5%' },
            { data: 'category.name', width: '10%' },
            { data: 'isLive', width: '10%' },
            {
                data: 'id',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <button type="button" onclick="EditProduct(${data})" class="btn btn-warning mx-1"><i class="bi bi-pencil-square"></i></button>
                    <button type="button" onclick="DeleteProduct(${data})" class="btn btn-danger mx-1"><i class="bi bi-trash"></i></button>
                    </div>`
                },
                width: '2%'
            }
        ],
    });

}

function LoadProductList(catId) {
    datatable = $('#tblData').DataTable({
        "processing": true,
        "serverSide": true,
        "cache": false,
        "ajax": {
            "url": `/Product/GetAllCustomized?filterbycat=${catId}`,
            "type": "GET",
            "data": function (d) {
                d.search = d.search.value;
                d.orderColumn = d.order[0].column;
                d.orderDirection = d.order[0].dir;
            }
        },
        "columnDefs": [
            { "className": "dt-center", "targets": "_all" }
        ],
        "language": {
            "searchPlaceholder": "search for product"
        },
        "columns": [
            { data: 'id', width: '3%', "className": "dt-right" },
            { data: 'name', width: '15%' },
            { data: 'price', width: '8%' },
            { data: 'quantity', width: '5%' },
            { data: 'reorderQuantity', width: '5%' },
            { data: 'category.name', width: '10%' },
            { data: 'isLive', width: '10%' },

            {
                data: 'id',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <button type="button" onclick="EditProduct(${data})" class="btn btn-warning mx-1"><i class="bi bi-pencil-square"></i></button>
                    <button type="button" onclick="DeleteProduct(${data})" class="btn btn-danger mx-1"><i class="bi bi-trash"></i></button>
                    </div>`
                },
                width: '2%'
            }
        ],
    });

}
function EditProduct(id) {
    console.log("executed");
    window.location.href = "/Product/Edit/" + id;
}

function DeleteProduct(id) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Product/Delete/' + id,
                type: 'DELETE',
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
    });
}

//$('#categoryId').on('change', function () {
//    alert(this.value);
//});
