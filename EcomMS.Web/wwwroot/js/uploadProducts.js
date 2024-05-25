function UploadProducts() {
    console.log("function called ");
    var file = $('#inputFile')[0].files[0];
    if (file == null) {
        alert("Select an excel file first!");
        return;
    }
    var formData = new FormData();
    formData.append("file", file);
    alert("Please click OK and wait for the success message. Do not close the tab.");
    $.ajax({
        url: "/Product/UploadCategoryBulk/",
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            alert("Products uploaded successfuly");
            window.location.href = "/Product/Index";
        },
        error: function (error) {
            alert("Internal server error!");
        }
    });
}