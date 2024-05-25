function UploadProductImage(id) {
    console.log("function called " + id);
    var file = $('#inputFile')[0].files[0];
    if (file == null) {
        alert("Select an image file first!");
        return;
    }
    var formData = new FormData();
    formData.append("file", file);

    $.ajax({
        url: "/Product/ImageUpload/" + id,
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            debugger;
            if (response.success) {
                toastr.success(response.msg);
            }
            else {
                toastr.error(response.msg);
            }
            setTimeout(function () {
                location.reload();
            }, 1500);
            
        },
        error: function (error) {
            toastr.error("Internal server error!");
        }
    });
}

function DeleteProductImage(imageId) {
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
                url: "/Product/DeleteImage/" + imageId,
                type: 'DELETE',
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.msg);
                    }
                    else {
                        toastr.error(response.msg);
                    }
                    setTimeout(function () {
                        location.reload();
                    }, 1500);

                },
                error: function (error) {
                    toastr.error("Internal server error!");
                }
            });
        }
    });
}