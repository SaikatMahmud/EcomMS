var datatable;
function LoadCategoryList() {
    datatable = $('#tblData').DataTable({
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
            //{
            //    data: 'id', width: '5%', render: function (data, type, row, meta) {
            //        return type === 'display' ?
            //            '<button type="button" onclick="Edit(' + data + ')" class="btn btn-warning">Edit</button>' +
            //            '<button type="button" onclick="Delete(' + data + ')" class="btn btn-danger"> Delete</button>' : data;
            //    }
            //},
            {
                data: 'id',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <button type="button" onclick="Edit(${data})" class="btn btn-warning mx-1"><i class="bi bi-pencil-square"></i></button>
                    <button type="button" onclick="Delete(${data})" class="btn btn-danger mx-1"><i class="bi bi-trash"></i></button>
                    </div>`
                },
                width: '2%'
            }
        ],
    });

}
function Edit(id) {
    console.log("executed");
    window.location.href = "/Category/Edit/" + id;
}

function Delete(id) {
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
                url: '/Category/Delete/' + id,
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